use serde::{Deserialize, Serialize};
use std::{
    net::SocketAddr,
    time::{Duration, Instant},
};

use crate::enums::CtrlType;
use crate::network::buffer::Buffer;
use crate::network::stream::Stream;
use crate::network::traits::Packet;

#[derive(Deserialize, Debug)]
pub struct ServerConfig {
    pub public: bool,
    pub servers: Vec<String>,
    pub ip: String,
    pub port: i32,
    pub max_players: u8,
    pub enabled_connections: Vec<u8>,
}

#[derive(Deserialize, Debug)]
pub struct Motd {
    pub text: String,
}

impl Buffer {
    pub fn write_time(&mut self, timer: &Timer) -> &mut Self {
        self.write_f32(&timer.now().as_secs_f32());
        self
    }
}

pub struct Timer {
    pub start: Instant,
    pub running_time: Duration,
    pub running: bool,
}

impl Timer {
    pub fn now(&self) -> Duration {
        if !self.running {
            return self.running_time;
        }
        Instant::now().duration_since(self.start) + self.running_time
    }
    pub fn start(&mut self) {
        if !self.running {
            self.start = Instant::now();
            self.running = true;
        }
    }
    pub fn stop(&mut self) {
        if self.running {
            self.running_time += Instant::now().duration_since(self.start);
            self.running = false
        }
    }
    pub fn reset(&mut self) {
        self.start = Instant::now();
        self.running_time = Duration::from_secs(0);
        self.running = false;
    }
    pub fn timeout(&self, seconds: u64) -> bool {
        self.running && self.now() > Duration::from_secs(seconds)
    }
}

impl Default for Timer {
    fn default() -> Self {
        Timer {
            start: Instant::now(),
            running_time: Duration::from_secs(0),
            running: false,
        }
    }
}

#[derive(Deserialize, Serialize, Debug)]
pub struct Settings {
    pub stage_id: i32,
    pub laps: i32,
    pub ai_count: i32,
    pub ai_skill: i32,
    pub auto_start_time: i32,
    pub auto_start_min_players: i32,
    pub auto_return_time: i32,
    pub vote_ratio: f32,
    pub stage_rotation_mode: i32,
}

impl Packet for Settings {
    fn read_from(stream: &mut Stream) -> Self {
        Settings {
            stage_id: stream.read_i32(),
            laps: stream.read_i32(),
            ai_count: stream.read_i32(),
            ai_skill: stream.read_i32(),
            auto_start_time: stream.read_i32(),
            auto_start_min_players: stream.read_i32(),
            auto_return_time: stream.read_i32(),
            vote_ratio: stream.read_f32(),
            stage_rotation_mode: stream.read_i32(),
        }
    }

    fn write_into(&self, buffer: &mut Buffer) {
        //Match settings properties, written in the order they appear in code
        buffer.write_i32(&self.stage_id); //Int32
        buffer.write_i32(&self.laps); //Int32
        buffer.write_i32(&self.ai_count); //Int32
        buffer.write_i32(&self.ai_skill); //Int32 (Cast to AISkillLevel)
        buffer.write_i32(&self.auto_start_time); //Int32
        buffer.write_i32(&self.auto_start_min_players); //Int32
        buffer.write_i32(&self.auto_return_time); //Int32
        buffer.write_f32(&self.vote_ratio); //Float
        buffer.write_i32(&self.stage_rotation_mode); //Int32 (Cast to StageRotationMode)
    }
}

#[derive(Debug, Clone, PartialEq)]
pub struct Stopwatch {}

#[derive(Clone)]
pub struct Client {
    pub guid: Vec<u8>,
    pub name: String,
    pub connection: SocketAddr,
    pub is_loading: bool,
    pub wants_lobby: bool,
    pub counter: usize,
}

impl Packet for Client {
    fn read_from(stream: &mut Stream) -> Self {
        Client {
            guid: stream.read_guid(),
            name: stream.read_string(),
            connection: stream.origin,
            is_loading: false,
            wants_lobby: false,
            counter: 0,
        }
    }

    fn write_into(&self, buffer: &mut Buffer) {
        buffer.write_guid(self.guid.clone());
        buffer.write_string(&self.name);
    }
}

#[derive(Debug, Clone, PartialEq)]
pub struct Player {
    pub guid: Vec<u8>,
    pub ctrl_type: i32,
    pub char_id: i32,
    pub ready_to_race: bool,
    pub is_racing: bool,
    pub race_timeout: Stopwatch,
    pub has_timed_out: bool,
}

impl Packet for Player {
    fn read_from(stream: &mut Stream) -> Self {
        Player {
            guid: stream.read_guid(),
            ctrl_type: stream.read_i32(),
            ready_to_race: stream.read_bool(),
            char_id: stream.read_i32(),
            is_racing: false,
            race_timeout: Stopwatch {},
            has_timed_out: false,
        }
    }

    fn write_into(&self, buffer: &mut Buffer) {
        buffer.write_guid(self.guid.clone());
        buffer.write_i32(&self.ctrl_type);
        buffer.write_bool(false);
        buffer.write_i32(&self.char_id);
    }
}

pub struct PlayerPosition {
    pub guid: Vec<u8>,
    pub ctrl_type: CtrlType,
    pub position: [f32; 3],
    pub rotation: [f32; 4],
    pub velocity: [f32; 3],
    pub angular_velocity: [f32; 3],
    pub direction: [f32; 3],
}

impl Packet for PlayerPosition {
    fn read_from(stream: &mut Stream) -> Self {
        PlayerPosition {
            guid: stream.read_guid(),
            ctrl_type: CtrlType::read_from(stream),
            position: stream.read_vec3(),
            rotation: stream.read_vec4(),
            velocity: stream.read_vec3(),
            angular_velocity: stream.read_vec3(),
            direction: stream.read_vec3(),
        }
    }

    fn write_into(&self, buffer: &mut Buffer) {
        buffer.write_guid(self.guid.clone());
        buffer.write_byte(self.ctrl_type.clone() as u8);
        buffer.write_vec3(&self.position);
        buffer.write_vec4(&self.rotation);
        buffer.write_vec3(&self.velocity);
        buffer.write_vec3(&self.angular_velocity);
        buffer.write_vec3(&self.direction);
    }
}
