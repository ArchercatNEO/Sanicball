use std::{
    fmt::{Display, Formatter},
    net::{IpAddr, Ipv4Addr, SocketAddr},
    str, usize,
};

use super::traits::Packet;
use crate::headers::{Header, Result};

#[derive(Clone)]
pub struct Stream {
    pub header: Result,
    pub header_byte: u8,
    pub sequence: u16,
    pub size: u16,
    pub origin: SocketAddr,
    pub data: Vec<u8>,
    ptr: usize,
}

impl Stream {
    pub fn new(stream: &[u8; 1500], size: usize, socket: SocketAddr) -> Self {
        let sequence1: u16 = stream[1].into();
        let sequence1: u16 = sequence1 >> 1;
        let sequence2: u16 = stream[2].into();
        let sequence2: u16 = sequence2 << 7;

        // In bits
        let size1: u16 = stream[3].into();
        let size2: u16 = stream[4].into();
        let size2: u16 = size2 << 8;

        Stream {
            header: stream[0].try_into(),
            header_byte: stream[0],
            sequence: sequence1 | sequence2,
            size: size1 | size2,
            origin: socket,
            data: (stream[5..size]).to_vec(),
            ptr: 0,
        }
    }

    pub fn read_byte(&mut self) -> u8 {
        self.ptr += 1;
        self.data[self.ptr - 1]
    }

    pub fn read_bytes(&mut self) -> &[u8] {
        self.ptr += 1;
        let size: usize = self.data[self.ptr - 1].into();
        self.ptr += size;
        &self.data[(self.ptr - size)..self.ptr]
    }

    pub fn read_string(&mut self) -> String {
        let mut size: usize = 0;
        let mut shift = 0;
        let mut byte: usize = self.read_byte().into();
        while byte & 0x80 == 0x80 {
            size |= (byte & 0x7F) << shift;
            byte = self.read_byte().into();
            shift += 7;
        }
        size |= (byte & 0x7F) << shift;

        self.ptr += size;
        let byte_string = &self.data[(self.ptr - size)..self.ptr];

        str::from_utf8(byte_string).unwrap().to_owned()
    }

    pub fn read_i32(&mut self) -> i32 {
        self.ptr += 4;
        let bytes: [u8; 4] = self.data[self.ptr - 4..self.ptr].try_into().unwrap();
        i32::from_le_bytes(bytes)
    }

    pub fn read_f32(&mut self) -> f32 {
        self.ptr += 4;
        let bytes: [u8; 4] = self.data[self.ptr - 4..self.ptr].try_into().unwrap();
        f32::from_le_bytes(bytes)
    }

    pub fn read_bool(&mut self) -> bool {
        self.read_byte() == 0
    }

    pub fn read_guid(&mut self) -> Vec<u8> {
        let size: usize = self.read_i32().try_into().unwrap();

        self.ptr += size;
        self.data[(self.ptr - size)..self.ptr].to_vec()
    }

    pub fn read_array<T: Packet>(&mut self) -> Vec<T>
    {
        let mut array = vec![];

        let mut size = self.read_i32();
        while size > 0 {
            size -= 1;
            array.push(T::read_from(self));
        }

        array
    }

    pub fn read_vec3(&mut self) -> [f32; 3] {
        [self.read_f32(), self.read_f32(), self.read_f32()]
    }

    pub fn read_vec4(&mut self) -> [f32; 4] {
        [
            self.read_f32(),
            self.read_f32(),
            self.read_f32(),
            self.read_f32(),
        ]
    }

    pub fn dump(&self) {
        println!("{:?}", self.data)
    }
}

impl Display for Stream {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        let clone = self.clone();
        match clone.header_byte {
            0 => write!(f, "Should not happen (unconnected)"),
            1 => write!(f, "User Unreliable (disabled)"),
            2..=33 => write!(f, "User Sequenced (disabled)"),
            34 => write!(f, "User Reliable Unordered (disabled)"),
            35..=66 => write!(f, "User Reliable Sequenced (disabled)"),
            67..=98 => write!(f, "User Reliable Ordered {}", clone.header_byte),
            99..=127 => write!(f, "Unused 1-28"),
            128 => write!(f, "Library error (disabled)"),
            129 => write!(f, "Ping"),
            130 => write!(f, "Pong (disabled)"),
            131 => write!(f, "Connect"),
            132 => write!(f, "Connect Response (disabled)"),
            133 => write!(f, "Connection Established"),
            134 => write!(f, "Ackwoledge"),
            135 => write!(f, "Disconnect (disabled)"),
            136 => write!(f, "Discovery (disabled)"),
            137 => write!(f, "Discovery Response (disabled)"),
            138 => write!(f, "Nat Punch Message (disabled)"),
            139 => write!(f, "Nat Intoduction (disabled)"),
            140 => write!(f, "Expand MTU Request (disabled)"),
            141 => write!(f, "Expand MTU Succsess (disabled)"),
            142 => write!(f, "Nat Introduction Confirm Request (disabled)"),
            143 => write!(f, "Nat Introduction Confirmed (disabled)"),
            144..=255 => write!(f, "Out of Bounds"),
        }
    }
}

impl Default for Stream {
    fn default() -> Self {
        Self {
            header: Ok(Header::Unconnected),
            header_byte: 0,
            sequence: 0,
            size: 0,
            origin: SocketAddr::new(IpAddr::V4(Ipv4Addr::new(0, 0, 0, 0)), 0),
            data: Default::default(),
            ptr: 0,
        }
    }
}
