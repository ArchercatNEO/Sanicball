using System.Collections.Generic;
using Sanicball.Ball;

namespace Sanicball.Scenes;

public class RaceOptions
{
    /// <summary>
    /// All local + remote players, described by their character and controller
    /// </summary>
    public required List<SanicBallDescriptor> Players { get; init; }

    /// <summary>
    /// The stage that has been selected for the race
    /// </summary>
    public required TrackResource SelectedStage { get; init; }

}