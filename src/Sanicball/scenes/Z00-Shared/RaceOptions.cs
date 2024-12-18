using System.Collections.Generic;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public class RaceOptions
{
    /// <summary>
    ///     All local + remote players, described by their character and controller
    /// </summary>
    public List<Character> Players { get; init; }

    /// <summary>
    ///     The stage that has been selected for the race
    /// </summary>
    public TrackResource SelectedStage { get; init; }
}
