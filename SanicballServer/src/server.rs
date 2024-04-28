use std::net::SocketAddr;

use crate::enums::{GameHeader, MessageTypes};
use crate::network::connection::Connection;
use crate::network::traits::Packet;
use crate::network::{buffer::Buffer, stream::Stream};
use crate::oxidize;
use crate::structs::Timer;
use crate::structs::{Motd, ServerConfig, Settings, Stopwatch};
use crate::{
    headers::Header,
    structs::{Client, Player},
};

pub struct Server<C: Connection> {
    app_id: &'static str,

    connection: C,

    config: ServerConfig,
    match_settings: Settings,
    motd: Motd,

    start_time: Timer,
    server_list_ping: Timer,
    lobby: Timer,
    auto_start: Timer,
    stage_load_timeout: Timer,
    back_to_lobby_timer: Timer,

    clients: Vec<Client>,
    players: Vec<Player>,

    stream: Stream,
}

impl<C: Connection> Server<C> {
    // ? Helper functions unrelated to most logic handling
    fn write_new(&self, json: &str) -> Vec<u8> {
        Buffer::default()
            .write_struct(&GameHeader::MatchMessage)
            .write_time(&self.start_time)
            .write(&json)
            .finish(Header::UserUnreliable) // ! Figure out how sequence works
    }

    ///Create a new message and send it to an address' chat
    fn write_chat(&self, message: &str) -> MessageTypes {
        MessageTypes::ChatMessage {
            from: "Server".to_owned(),
            r#type: 0,
            text: message.to_owned(),
        }
    }

    fn chat_to(&self, message: &str, addr: SocketAddr) {
        let object = &self.write_chat(message).as_json();
        let mut data = self.write_new(object);
        self.connection.send_to(&mut data, addr);
    }

    fn chat_all(&self, message: &str) {
        let object = &self.write_chat(message).as_json();
        let mut data = self.write_new(object);
        self.connection.send_all(&mut data);
    }

    fn send_new(&self, message: MessageTypes) {
        let object = &message.as_json();
        let mut data = self.write_new(object);
        self.connection.send_all(&mut data);
    }

    fn sending_client(&self) -> Option<usize> {
        self.clients
            .iter()
            .position(|e| e.connection == self.stream.origin)
    }

    fn current_client(&self, guid: Vec<u8>) -> Option<usize> {
        self.clients.iter().position(|e| e.guid == guid)
    }

    fn current_player(&self, guid: Vec<u8>, control: &i32) -> Option<usize> {
        self.players
            .iter()
            .position(|e| e.guid == guid && &e.ctrl_type == control)
    }

    fn verify_player(&self, _player: Player) -> bool {
        true
    }

    fn timers(&mut self) {
        if self.lobby.timeout(3) {
            self.lobby.reset();
            self.stage_load_timeout.start();

            self.send_new(MessageTypes::LoadRaceMessage {});
        }

        if self.stage_load_timeout.timeout(10) {
            self.stage_load_timeout.reset();

            self.send_new(MessageTypes::StartRaceMessage {});
        }
    }

    // ? Start of Logic handling
    // ? If you read this from top to bottom you should get a pretty good grasp of what's going on

    ///Start a new Server
    pub fn new(config: ServerConfig, matches: Settings, motd: Motd) -> Server<C> {
        println!("Hello there welcome your stay");
        println!("Starting server on ip: [129.0.0.1], port: [7878]");

        Server {
            app_id: "Sanicball",
            connection: C::new("0.0.0.0::7878"),
            config,
            match_settings: matches,
            motd,
            clients: vec![],
            players: vec![],
            stream: Stream::default(),
            start_time: Timer::default(),
            server_list_ping: Timer::default(),
            lobby: Timer::default(),
            auto_start: Timer::default(),
            stage_load_timeout: Timer::default(),
            back_to_lobby_timer: Timer::default(),
        }
    }

    ///Update the Server's incoming requests
    pub fn update(&mut self) {
        self.timers();
        let mut buffer = [0; 1500];
        let raw = self.connection.recv_from(&mut buffer);

        if let Ok((size, addr)) = raw {
            self.stream = Stream::new(&buffer, size, addr);

            match self.stream.header {
                Ok(header) => match header {
                    // ! This is not correct
                    Header::Acknowledge => {}
                    //* Correct as far as I can tell
                    Header::Ping => {
                        let ping_number = self.stream.read_byte();

                        let mut data = Buffer::default()
                            .write_byte(ping_number)
                            .write_time(&self.start_time)
                            .finish(Header::Pong);

                        self.connection.send_to(&mut data, self.stream.origin);
                    }
                    //* Correct as far as I can tell
                    Header::Connect => {
                        //Initialize App ID on connect
                        //TODO Check version is valid
                        let _app_id = self.stream.read_string();

                        //TODO Figure out what this does
                        // Don't ask because I don't know
                        self.stream.read_f32();
                        self.stream.read_f32();
                        self.stream.read_f32();

                        let mut data = Buffer::default()
                            .write_string(self.app_id)
                            .finish(Header::ConnectResponse);

                        self.connection.send_to(&mut data, addr);
                    }
                    //* Correct as far as I can tell
                    Header::ConnectionEstablished => {
                        //TODO Figure out what this does
                        self.stream.read_f32();

                        let mut data = Buffer::default()
                            .write_struct(&GameHeader::InitMessage)
                            .write_array(&self.clients)
                            .write_array(&self.players)
                            .write_struct(&self.match_settings)
                            .write_bool(false) //In race
                            .write_f32(&67.0) //Cur auto start time
                            .finish(Header::UserReliableOrdered1);

                        self.connection.send_to(&mut data, addr);
                    }
                    Header::UserReliableOrdered1 | Header::UserUnreliable => {
                        let mut data = Buffer::default()
                            .write_byte(self.stream.header_byte)
                            .write_byte((self.stream.sequence & 0xFF).try_into().unwrap())
                            .write_byte((self.stream.sequence >> 8).try_into().unwrap())
                            .finish(Header::Acknowledge);

                        self.connection.send_to(&mut data, self.stream.origin);
                        self.relay_data();
                    },
                    _ => panic!(
                        "Tried to respond to a header that is not implamented {} (disable it)",
                        self.stream
                    ),
                },
                Err(_) => {
                    println!("Ignoring {}", self.stream);
                }
            };
        }
    }

    ///Relay and react to a change in the game's state
    fn relay_data(&mut self) {
        match GameHeader::read_from(&mut self.stream) {
            GameHeader::MatchMessage => {
                let _time = self.stream.read_f32();
                
                let json = self.stream.read_string();
                let mut data = self.write_new(&json);
                self.connection.send_all(&mut data);

                let json = oxidize(json);
                self.update_server_state(&json);

                // ! Handle this inside connection
                match self.sending_client() {
                    Some(index) => {
                        self.clients[index].counter += 1;
                    }
                    None => println!("Client is joining, sequence ignored"),
                }
            }
            GameHeader::InitMessage => panic!(), // The game itself never sends this
            GameHeader::PlayerMovementMessage => {
                let _time = self.stream.read_f32();
                let guid = match self.sending_client() {
                    Some(index) => self.clients[index].guid.clone(),
                    None => vec![],
                };

                let mut data = Buffer::default()
                    .write_struct(&GameHeader::PlayerMovementMessage)
                    .write_time(&self.start_time)
                    .write_bytes(&self.stream.data[5..])
                    .finish(Header::UserUnreliable);

                //? self.connection.all_where(&mut data, |id| id != guid);
            }
        }
    }

    ///I don't think I need to explain why this isn't inlined
    fn update_server_state(&mut self, json: &str) {
        let message: MessageTypes = serde_json::from_str(json).unwrap();
        match message {
            MessageTypes::AutoStartTimerMessage { enabled } => todo!(),
            MessageTypes::ChangedReadyMessage {
                client_guid,
                ctrl_type,
                ready,
            } => {
                self.lobby.start();
                let vecter = guid_to_vec(client_guid);
                match self.current_player(vecter, &ctrl_type) {
                    Some(index) => {
                        self.players[index].ready_to_race = ready;
                    }
                    None => println!("No such player"),
                }
            }
            MessageTypes::CharacterChangedMessage {
                client_guid,
                ctrl_type,
                new_character,
            } => {
                let vecter = guid_to_vec(client_guid);
                match self.current_player(vecter, &ctrl_type) {
                    Some(index) => {
                        // ! validate player
                        self.players[index].char_id = new_character;
                    }
                    None => println!("Player does not exist"),
                }
            }
            // TODO meme everyone into shrek
            MessageTypes::ChatMessage { from, r#type, text } => {}
            MessageTypes::CheckpointPassedMessage {
                client_guid,
                ctrl_type,
                lap_time,
            } => {}
            MessageTypes::ClientJoinedMessage {
                client_guid,
                client_name,
            } => {
                let socket = self.stream.origin;
                let vecter = guid_to_vec(client_guid);

                self.chat_all(&format!("{:?}, Has Joined The Match", client_name));
                self.chat_to("Welcome", socket);
                self.chat_to(
                    &format!("Our Message of the day is {}", self.motd.text),
                    socket,
                );
                // ! valid characters
                // ! Add support to send what characters are allowed

                self.clients.push(Client {
                    guid: vecter,
                    name: client_name,
                    connection: self.stream.origin,
                    is_loading: false,
                    wants_lobby: false,
                    counter: 0,
                });
            }
            MessageTypes::ClientLeftMessage { client_guid } => {}
            MessageTypes::DoneRacingMessage {
                client_guid,
                ctrl_type,
                race_time,
                disqualified,
            } => {
                let vecter = guid_to_vec(client_guid);
                match self.current_player(vecter, &ctrl_type) {
                    Some(index) => {
                        self.players[index].is_racing = false;
                        // ! reset race timeout
                        // ! kick everyone
                    }
                    None => println!("Who are you talking about"),
                }
            }
            MessageTypes::LoadLobbyMessage {} => {}
            MessageTypes::LoadRaceMessage {} => {}
            MessageTypes::PlayerJoinedMessage {
                client_guid,
                ctrl_type,
                initial_character,
            } => {
                let socket = self.stream.origin;

                let vecter = guid_to_vec(client_guid);
                println!("{json}");
                match self.current_client(vecter.clone()) {
                    None => println!("A Player that is not a Client attempted to join"),
                    Some(_) => {
                        println!("New player");
                        // ! Verify player is valid
                        //self.chat_to("You can't join", socket);
                        self.players.push(Player {
                            guid: vecter,
                            ctrl_type,
                            char_id: initial_character,
                            ready_to_race: false,
                            is_racing: false,
                            race_timeout: Stopwatch {},
                            has_timed_out: false,
                        })
                    }
                }
            }
            MessageTypes::PlayerLeftMessage {
                client_guid,
                ctrl_type,
            } => {
                let vecter = guid_to_vec(client_guid);
                match self.current_player(vecter, &ctrl_type) {
                    Some(index) => {
                        self.players.remove(index);
                    }
                    None => println!("This guy didn't exist anyway"),
                }
            }
            MessageTypes::RaceFinishedMessage {
                client_guid,
                ctrl_type,
                race_time,
                race_position,
            } => {}
            MessageTypes::RaceTimeoutMessage {
                client_guid,
                ctrl_type,
                time,
            } => {}
            MessageTypes::SettingsChanged { new_match_settings } => {
                self.match_settings = new_match_settings;
            }
            MessageTypes::StartRaceMessage {} => {
                self.stage_load_timeout.stop();
                match self.sending_client() {
                    Some(index) => {
                        // ! smth smth timers
                        for player in &mut self.players {
                            player.is_racing = true;
                            //player.race_timeout
                        }
                    }
                    None => println!("What?"),
                }
            }
        }
    }
}

fn guid_to_vec(guid: String) -> Vec<u8> {
    let mut sum: Vec<u8> = Vec::with_capacity(16);

    let mut first: Vec<u8> = (0..8)
        .step_by(2)
        .map(|i| u8::from_str_radix(&guid[i..i + 2], 16).unwrap())
        .collect();
    first.reverse();

    let mut second: Vec<u8> = (9..13)
        .step_by(2)
        .map(|i| u8::from_str_radix(&guid[i..i + 2], 16).unwrap())
        .collect();
    second.reverse();

    let mut third: Vec<u8> = (14..18)
        .step_by(2)
        .map(|i| u8::from_str_radix(&guid[i..i + 2], 16).unwrap())
        .collect();
    third.reverse();

    let mut forth: Vec<u8> = (19..23)
        .step_by(2)
        .map(|i| u8::from_str_radix(&guid[i..i + 2], 16).unwrap())
        .collect();

    let mut fifth: Vec<u8> = (24..36)
        .step_by(2)
        .map(|i| u8::from_str_radix(&guid[i..i + 2], 16).unwrap())
        .collect();

    sum.append(&mut first);
    sum.append(&mut second);
    sum.append(&mut third);
    sum.append(&mut forth);
    sum.append(&mut fifth);
    sum
}
