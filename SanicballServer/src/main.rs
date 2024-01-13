//? Data types not special/big enough to deserve a file
pub mod enums;
pub mod structs;

pub mod headers;
pub mod version;

pub mod application;
pub mod network;
pub mod server;

#[cfg(test)]
pub mod tests;

use crate::server::Server;
use network::connection::UdpConnection;
use serde::de::DeserializeOwned;
use std::{fs::File, io::BufReader};

//TODO Make this into an executable app
//TODO Turn into CLI
fn main() {
    let server_config = load_file("config.json").unwrap();
    let match_config = load_file("match.json").unwrap();
    let motd = load_file("motd.json").unwrap();

    let mut server: Server<UdpConnection> = Server::new(server_config, match_config, motd);

    loop {
        server.update();
        //sleep(Duration::from_secs(1))
    }
}

pub fn to_byte(num: usize) -> u8 {
    (num & 0xFF).try_into().unwrap()
}

///Turn a C# JSON into a Rust JSON
pub fn oxidize(mut json: String) -> String {
    json = json.replace("SanicballCore.MatchMessages.", "");
    json.replace(", SanicballCore", "")
}

fn load_file<T: DeserializeOwned>(filename: &str) -> Result<T, FileOpenError>
{
    let file = File::open(filename)?;
    let reader = BufReader::new(file);
    let json = serde_json::from_reader(reader)?;

    Ok(json)
}

#[derive(Debug)]
enum FileOpenError {
    FileDoesNotExistExeption(std::io::Error),
    JsonException(serde_json::Error)
}

impl From<std::io::Error> for FileOpenError {
    fn from(value: std::io::Error) -> Self {
        FileOpenError::FileDoesNotExistExeption(value)
    }
}

impl From<serde_json::Error> for FileOpenError {
    fn from(value: serde_json::Error) -> Self {
        FileOpenError::JsonException(value)
    }
}

trait How {
    fn generic_thing(&self);
}

impl<T> How for T {
    fn generic_thing(&self) {
        println!("Whaaaa...")
    }
}