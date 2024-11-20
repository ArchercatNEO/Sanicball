use crate::structs::{Player, Stopwatch};

#[test]
fn struct_read_write() {
    let player = Player {
        guid: vec![0],
        ctrl_type: 10,
        char_id: 10,
        ready_to_race: true,
        is_racing: true,
        race_timeout: Stopwatch {},
        has_timed_out: false,
    };

    assert_eq!(player, player.clone());
}
