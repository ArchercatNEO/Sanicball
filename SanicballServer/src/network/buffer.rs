use std::{fmt::Display, collections::HashMap, ops::{DerefMut, Deref}, rc::Rc};
use std::sync::{Arc, Mutex};
use regex::Regex;

use super::traits::Packet;
use crate::{headers::Header, to_byte};

///A write-only struct built with builder patterns in mind
#[derive(Debug)]
pub struct Buffer {
    payload: [u8; 1500],
    ptr: usize,
}

impl Buffer {
    pub fn write<T: Writable>(&mut self, object: &T) -> &Self {
        object.write(self);
        self
    }

    ///Write a single byte into the buffer and shift the pointer to the right.
    pub fn write_byte(&mut self, byte: u8) -> &mut Self {
        self.payload[self.ptr] = byte;
        self.ptr += 1;
        self
    }

    pub fn write_bytes(&mut self, bytes: &[u8]) -> &mut Self {
        for byte in bytes.iter() {
            self.payload[self.ptr] = *byte;
            self.ptr += 1;
        }

        self
    }

    pub fn write_string(&mut self, string: &str) -> &mut Self {
        let bytes = string.as_bytes();

        let mut size = bytes.len();
        while size >= 0x80 {
            self.write_byte(to_byte(size | 0x80));
            size >>= 7;
        }
        self.write_byte(to_byte(size));

        for byte in bytes.iter() {
            self.write_byte(*byte);
        }

        self
    }

    pub fn write_i32(&mut self, num: &i32) -> &mut Self {
        for byte in num.to_le_bytes().iter() {
            self.write_byte(*byte);
        }

        self
    }

    pub fn write_f32(&mut self, num: &f32) -> &mut Self {
        for byte in num.to_le_bytes().iter() {
            self.write_byte(*byte);
        }

        self
    }

    pub fn write_bool(&mut self, statement: bool) -> &mut Self {
        self.write_byte(statement.into());

        self
    }

    pub fn write_guid(&mut self, guid: Vec<u8>) -> &mut Self {
        let size: i32 = guid.len().try_into().unwrap();

        self.write_i32(&size);
        for byte in guid {
            self.write_byte(byte);
        }

        self
    }

    pub fn write_array<T: Packet>(&mut self, array: &[T]) -> &mut Self {
        self.write_i32(&array.len().try_into().unwrap());
        for packet in array.iter() {
            packet.write_into(self);
        }

        self
    }

    pub fn write_struct<T: Packet>(&mut self, object: &T) -> &mut Self
    {
        object.write_into(self);
        self
    }

    pub fn write_vec3(&mut self, vector: &[f32; 3]) -> &mut Self {
        self.write_f32(&vector[0]);
        self.write_f32(&vector[1]);
        self.write_f32(&vector[2]);

        self
    }

    pub fn write_vec4(&mut self, vector: &[f32; 4]) -> &mut Self {
        self.write_f32(&vector[0]);
        self.write_f32(&vector[1]);
        self.write_f32(&vector[2]);
        self.write_f32(&vector[3]);

        self
    }

    pub fn finish(&mut self, header: Header) -> Vec<u8> {
        // special number that tells unity how to respond
        self.payload[0] = header as u8;

        // remove the assumed 5 bytes then convert to bits
        let bits = (self.ptr - 5) * 8;

        // message size in bits, split in two for int support
        self.payload[3] = to_byte(bits); // low bits
        self.payload[4] = to_byte(bits >> 8); // high bits

        self.payload[..self.ptr].to_owned()
    }
}

impl Default for Buffer {
    fn default() -> Self {
        Buffer {
            payload: [0; 1500],
            ptr: 5,
        }
    }
}

impl Display for Buffer {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "Length: {}", self.ptr)
    }
}

///Turn a Rust JSON into a C# JSON
pub fn sharpize(json: String, substring: &'static str) -> String {
    json.replace(
        substring,
        &format!("SanicballCore.MatchMessages.{substring}, SanicballCore"),
    )
}

pub trait Writable {
    fn write(&self, buffer: &mut Buffer);
}

impl Writable for u32 {
    fn write(&self, buffer: &mut Buffer) {
        buffer.write_bytes(&self.to_le_bytes());
    }
}

impl Writable for i32 {
    fn write(&self, buffer: &mut Buffer) {
        buffer.write_bytes(&self.to_le_bytes());
    }
}

impl Writable for usize {
    fn write(&self, buffer: &mut Buffer) {
        buffer.write_bytes(&self.to_le_bytes());
    }
}

impl Writable for &str {
    fn write(&self, buffer: &mut Buffer) {
        let bytes = self.as_bytes();

        let mut size = bytes.len();
        while size >= 0x80 {
            buffer.write_byte(to_byte(size | 0x80));
            size >>= 7;
        }
        buffer.write_byte(to_byte(size));

        for byte in bytes.iter() {
            buffer.write_byte(*byte);
        }
    }
}

impl<T: Writable> Writable for [T] {
    fn write(&self, buffer: &mut Buffer) {
        let size = self.len();
        size.write(buffer);
        for item in self.iter() {
            item.write(buffer);
        };
    }
}

impl<T: Writable> Writable for Vec<T> {
    fn write(&self, buffer: &mut Buffer) {
        let size = self.len();
        size.write(buffer);
        for item in self.iter() {
            item.write(buffer);
        };
    }
}

// Simple                     Fully Qualified                                     Assembly
// {"AutoStartTimerMessage" : "SanicballCore.MatchMessages.AutoStartTimerMessage, SanicballCore"}
trait JsonTransformer {
    ///Left to right
    fn oxidize(&mut self, json: String) -> Result<String, JsonTransformError>;
    ///Right to left
    fn sharpize(&mut self, json: String) -> Result<String, JsonTransformError>;
}

impl JsonTransformer for HashMap<String, String> {
    fn oxidize(&mut self, json: String) -> Result<String, JsonTransformError> {
        let regex = Regex::new(r"SanicballCore\..*\..*, SanicballCore")?;
        let fully_qualified = regex.find(&json).unwrap().as_str();
        let simple = fully_qualified.replace(
            Regex::new(r"SanicballCore\..*\.")?.find(&json).unwrap().as_str(),
            ""
        ).replace(
            ", SanicballCore",
            ""
        );

        Ok(self.entry(simple.clone()).or_insert(fully_qualified.to_string()).to_string())
    }

    fn sharpize(&mut self, json: String) -> Result<String, JsonTransformError> {
        match self.get(&json) {
            Some(value) => Ok(value.clone()),
            None => Err(JsonTransformError::PatternNotFound),
        }
    }
}

enum JsonTransformError {
    PatternNotFound,
    RegexSyntax(String),
    RegexCompiledTooBig(usize),
    RegexUnkown
}

impl From<regex::Error> for JsonTransformError {
    fn from(value: regex::Error) -> Self {
        match value {
            regex::Error::Syntax(err) => JsonTransformError::RegexSyntax(err),
            regex::Error::CompiledTooBig(err) => JsonTransformError::RegexCompiledTooBig(err),
            _ => JsonTransformError::RegexUnkown
        }
    }
}

impl<T> From<Option<T>> for JsonTransformError {
    fn from(_: Option<T>) -> Self {
        JsonTransformError::PatternNotFound
    }
}
