use super::buffer::Buffer;
use super::stream::Stream;

pub trait Packet {
    fn read_from(stream: &mut Stream) -> Self;
    fn write_into(&self, buffer: &mut Buffer);
}
