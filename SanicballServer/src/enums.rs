use serde::{Deserialize, Serialize};

use crate::network::buffer::{sharpize, Buffer};
use crate::network::stream::Stream;
use crate::network::traits::Packet;
use crate::structs::Settings;

pub enum CharacterTier {
    Normal = 0,
    Odd = 1,
    Hypersonic = 2,
}

#[derive(Debug, Deserialize, Serialize)]
pub enum ChatMessageType {
    System,
    User,
}

#[derive(Debug, Deserialize, Serialize, Clone)]
pub enum CtrlType {
    None = -1,
    Keyboard = 0,
    Joystick1 = 1,
    Joystick2 = 2,
    Joystick3 = 3,
    Joystick4 = 4,
}

impl Packet for CtrlType {
    fn read_from(stream: &mut Stream) -> Self {
        match stream.read_byte() {
            0 => CtrlType::Keyboard,
            1 => CtrlType::Joystick1,
            2 => CtrlType::Joystick2,
            3 => CtrlType::Joystick3,
            4 => CtrlType::Joystick4,
            _ => panic!("That's not a control type"),
        }
    }

    fn write_into(&self, _buffer: &mut Buffer) {
        todo!()
    }
}

pub enum GameHeader {
    MatchMessage = 0,
    InitMessage = 1,
    PlayerMovementMessage = 2,
}

impl Packet for GameHeader {
    fn read_from(stream: &mut Stream) -> Self {
        let byte = stream.read_byte();
        match byte {
            0 => GameHeader::MatchMessage,
            1 => GameHeader::InitMessage,
            2 => GameHeader::PlayerMovementMessage,
            _ => panic!("You done goofed {byte}"),
        }
    }

    fn write_into(&self, buffer: &mut Buffer) {
        match self {
            GameHeader::MatchMessage => buffer.write_byte(0),
            GameHeader::InitMessage => buffer.write_byte(1),
            GameHeader::PlayerMovementMessage => buffer.write_byte(2),
        };
    }
}

#[derive(Debug, Deserialize, Serialize)]
#[serde(tag = "$type")]
pub enum MessageTypes {
    #[serde(rename_all = "PascalCase")]
    AutoStartTimerMessage { enabled: bool },
    #[serde(rename_all = "PascalCase")]
    ChangedReadyMessage {
        client_guid: String,
        ctrl_type: i32,
        ready: bool,
    },
    #[serde(rename_all = "PascalCase")]
    CharacterChangedMessage {
        client_guid: String,
        ctrl_type: i32,
        new_character: i32,
    },
    #[serde(rename_all = "PascalCase")]
    ChatMessage {
        from: String,
        r#type: u8,
        text: String,
    },
    #[serde(rename_all = "PascalCase")]
    CheckpointPassedMessage {
        client_guid: String,
        ctrl_type: i32,
        lap_time: f32,
    },
    #[serde(rename_all = "PascalCase")]
    ClientJoinedMessage {
        client_guid: String,
        client_name: String,
    },
    #[serde(rename_all = "PascalCase")]
    ClientLeftMessage { client_guid: String },
    #[serde(rename_all = "PascalCase")]
    DoneRacingMessage {
        client_guid: String,
        ctrl_type: i32,
        race_time: f64,
        disqualified: bool,
    },
    #[serde(rename_all = "PascalCase")]
    LoadLobbyMessage {},
    #[serde(rename_all = "PascalCase")]
    LoadRaceMessage {},
    #[serde(rename_all = "PascalCase")]
    PlayerJoinedMessage {
        client_guid: String,
        ctrl_type: i32,
        initial_character: i32,
    },
    #[serde(rename_all = "PascalCase")]
    PlayerLeftMessage { client_guid: String, ctrl_type: i32 },
    #[serde(rename_all = "PascalCase")]
    RaceFinishedMessage {
        client_guid: String,
        ctrl_type: i32,
        race_time: f32,
        race_position: i32,
    },
    #[serde(rename_all = "PascalCase")]
    RaceTimeoutMessage {
        client_guid: String,
        ctrl_type: i32,
        time: f32,
    },
    #[serde(rename_all = "PascalCase")]
    SettingsChanged { new_match_settings: Settings },
    #[serde(rename_all = "PascalCase")]
    StartRaceMessage {},
}

impl MessageTypes {
    pub fn as_json(&self) -> String {
        let json = serde_json::to_string(&self).unwrap();
        let substring = self.as_string();
        sharpize(json, substring)
    }

    pub fn as_string(&self) -> &'static str {
        match self {
            MessageTypes::AutoStartTimerMessage { enabled: _ } => "AutoStartTimerMessage",
            MessageTypes::ChangedReadyMessage {
                client_guid: _,
                ctrl_type: _,
                ready: _,
            } => "ChangedReadyMessage",
            MessageTypes::CharacterChangedMessage {
                client_guid: _,
                ctrl_type: _,
                new_character: _,
            } => "CharacterChangedMessage",
            MessageTypes::ChatMessage {
                from: _,
                r#type: _,
                text: _,
            } => "ChatMessage",
            MessageTypes::CheckpointPassedMessage {
                client_guid: _,
                ctrl_type: _,
                lap_time: _,
            } => "CheckpointPassedMessage",
            MessageTypes::ClientJoinedMessage {
                client_guid: _,
                client_name: _,
            } => "ClientJoinedMessage",
            MessageTypes::ClientLeftMessage { client_guid: _ } => "ClientLeftMessage",
            MessageTypes::DoneRacingMessage {
                client_guid: _,
                ctrl_type: _,
                race_time: _,
                disqualified: _,
            } => "DoneRacingMessage",
            MessageTypes::LoadLobbyMessage {} => "LoadLobbyMessage",
            MessageTypes::LoadRaceMessage {} => "LoadRaceMessage",
            MessageTypes::PlayerJoinedMessage {
                client_guid: _,
                ctrl_type: _,
                initial_character: _,
            } => "PlayerJoinedMessage",
            MessageTypes::PlayerLeftMessage {
                client_guid: _,
                ctrl_type: _,
            } => "PlayerLeftMessage",
            MessageTypes::RaceFinishedMessage {
                client_guid: _,
                ctrl_type: _,
                race_time: _,
                race_position: _,
            } => "RaceFinishedMessage",
            MessageTypes::RaceTimeoutMessage {
                client_guid: _,
                ctrl_type: _,
                time: _,
            } => "RaceTimeoutMessage",
            MessageTypes::SettingsChanged {
                new_match_settings: _,
            } => "SettingsChanged",
            MessageTypes::StartRaceMessage {} => "StartRaceMessage",
        }
    }
}

impl From<MessageTypes> for String {
    fn from(value: MessageTypes) -> Self {
        match value {
            MessageTypes::AutoStartTimerMessage { enabled: _ } => "AutoStartTimerMessage",
            MessageTypes::ChangedReadyMessage {
                client_guid: _,
                ctrl_type: _,
                ready: _,
            } => "ChangedReadyMessage",
            MessageTypes::CharacterChangedMessage {
                client_guid: _,
                ctrl_type: _,
                new_character: _,
            } => "CharacterChangedMessage",
            MessageTypes::ChatMessage {
                from: _,
                r#type: _,
                text: _,
            } => "ChatMessage",
            MessageTypes::CheckpointPassedMessage {
                client_guid: _,
                ctrl_type: _,
                lap_time: _,
            } => "CheckpointPassedMessage",
            MessageTypes::ClientJoinedMessage {
                client_guid: _,
                client_name: _,
            } => "ClientJoinedMessage",
            MessageTypes::ClientLeftMessage { client_guid: _ } => "ClientLeftMessage",
            MessageTypes::DoneRacingMessage {
                client_guid: _,
                ctrl_type: _,
                race_time: _,
                disqualified: _,
            } => "DoneRacingMessage",
            MessageTypes::LoadLobbyMessage {} => "LoadLobbyMessage",
            MessageTypes::LoadRaceMessage {} => "LoadRaceMessage",
            MessageTypes::PlayerJoinedMessage {
                client_guid: _,
                ctrl_type: _,
                initial_character: _,
            } => "PlayerJoinedMessage",
            MessageTypes::PlayerLeftMessage {
                client_guid: _,
                ctrl_type: _,
            } => "PlayerLeftMessage",
            MessageTypes::RaceFinishedMessage {
                client_guid: _,
                ctrl_type: _,
                race_time: _,
                race_position: _,
            } => "RaceFinishedMessage",
            MessageTypes::RaceTimeoutMessage {
                client_guid: _,
                ctrl_type: _,
                time: _,
            } => "RaceTimeoutMessage",
            MessageTypes::SettingsChanged {
                new_match_settings: _,
            } => "SettingsChanged",
            MessageTypes::StartRaceMessage {} => "StartRaceMessage",
        }.to_string()
    }
}
