use std::net::{SocketAddr, UdpSocket};

pub trait Connection {
    type Id: Eq;

    fn new(addr: &'static str) -> Self;

    fn send_to(&self, buffer: &mut [u8], address: SocketAddr);
    fn send_all(&self, buffer: &mut [u8]);
    fn all_except(&self, buffer: &mut [u8], id: Self::Id);
    fn all_where<F: Fn(Self::Id) -> bool>(&self, buffer: &mut [u8], predicate: F);

    fn recv_from(&self, buffer: &mut [u8]) -> std::io::Result<(usize, SocketAddr)>;
}

pub struct UdpConnection {
    listener: UdpSocket,
    connections: Vec<Client<String>>,
}

pub struct Client<T> {
    address: SocketAddr,
    id: T,
    pub seq: u16,
}

impl Connection for UdpConnection {
    type Id = String;
    
    fn new(addr: &'static str) -> Self {
        UdpConnection {
            listener: UdpSocket::bind(addr).unwrap(),
            connections: vec![],
        }
    }

    fn send_to(&self, buffer: &mut [u8], address: SocketAddr) {
        match self.listener.send_to(buffer, address) {
            Ok(_value) => {}
            Err(err) => println!("Became unable to send_to {}", err),
        }
    }

    fn send_all(&self, buffer: &mut [u8]) {
        for client in self.connections.iter() {
            buffer[1] = (client.seq << 1) as u8;
            buffer[2] = (client.seq >> 7) as u8;

            if let Err(err) = self.listener.send_to(buffer, client.address) {
                println!("Something {err}");
            };
        }
    }

    fn all_except(&self, buffer: &mut [u8], id: String) {
        for client in self.connections.iter() {
            if client.id == id {
                return;
            }
            
            if let Err(err) = self.listener.send_to(buffer, client.address) {
                println!("Something {err}");
            };
        }
    }

    fn all_where<F: Fn(String) -> bool>(&self, buffer: &mut [u8], predicate: F) {
        let filtered = self.connections.iter().filter(|e| predicate(e.id.clone()));

        for client in filtered {
            if let Err(err) = self.listener.send_to(buffer, client.address) {
                println!("Something {err}");
            };
        }
    }

    fn recv_from(&self, buffer: &mut [u8]) -> std::io::Result<(usize, SocketAddr)> {
        self.listener.recv_from(buffer)
    }
}
