use std::net::{SocketAddr, UdpSocket};

pub trait Connection<T> {
    fn send_to(&self, buffer: &mut [u8], address: SocketAddr);
    fn send_all(&self, buffer: &mut [u8]);
    fn all_except(&self, buffer: &mut [u8], id: T);
    fn all_where(&self, buffer: &mut [u8], predicate: Box<dyn Fn(T) -> bool>);
}

pub struct UdpConnection {
    listener: UdpSocket,
    connections: Vec<Client<String>>,
}

impl UdpConnection {
    pub fn new(addr: &'static str) -> Self {
        UdpConnection {
            listener: UdpSocket::bind(addr).unwrap(),
            connections: vec![],
        }
    }
}

pub struct Client<T> {
    address: SocketAddr,
    id: T,
    pub seq: u16,
}

impl Connection<String> for UdpConnection {
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

            match self.listener.send_to(buffer, client.address) {
                Ok(_) => {}
                Err(err) => println!("Something {err}"),
            }
        }
    }

    fn all_except(&self, buffer: &mut [u8], id: String) {
        for client in self.connections.iter() {
            if client.id == id {
                return;
            }
            match self.listener.send_to(buffer, client.address) {
                Ok(_) => {}
                Err(err) => println!("Something {err}"),
            }
        }
    }

    fn all_where(&self, buffer: &mut [u8], predicate: Box<dyn Fn(String) -> bool>) {
        let filtered = self.connections.iter().filter(|e| predicate(e.id.clone()));

        for client in filtered {
            match self.listener.send_to(buffer, client.address) {
                Ok(_) => {}
                Err(err) => println!("Something {err}"),
            }
        }
    }
}
