using System;
using Godot;
using Sanicball.Ball;

namespace Sanicball.GameMechanics;


//TODO consider implementing builder
public class CheckpointReciever(Checkpoint initialCheckpoint, int maxLaps)
{
    public event EventHandler<Checkpoint>? NextCheckpoint;
    public event EventHandler<int>? NextLap;
    public event EventHandler? RaceFinished;

    //null when we have finished the race
    public Checkpoint? CurrentCheckpoint { get; private set; } = initialCheckpoint;

    private int currentLap = 1;

    public void OnCheckpointCollion(Checkpoint checkpoint)
    {
        if (CurrentCheckpoint is null || currentLap == maxLaps)
        {
            return;
        }

        if (CurrentCheckpoint.next != checkpoint)
        {
            return;
        }

        CurrentCheckpoint = checkpoint;
        NextCheckpoint?.Invoke(this, checkpoint);

        if (!CurrentCheckpoint.isFinishLine)
        {
            return;
        }

        currentLap++;
        NextLap?.Invoke(this, currentLap);

        if (currentLap == maxLaps)
        {
            CurrentCheckpoint = null;
            RaceFinished?.Invoke(this, EventArgs.Empty);
        }
    }

    #region Marker Tracking
    

    //null if tracking has not been added OR we finished the race
    private ObjectMarker? checkpointMarker;

    public ObjectMarker? AddTracking(Camera3D origin)
    {
        if (CurrentCheckpoint == null)
        {
            return null;
        }

        checkpointMarker = ObjectMarker.Create(origin, CurrentCheckpoint, new(0, 0, 1));
        
        NextCheckpoint += (sender, checkpoint) =>
        {
            if (checkpointMarker == null)
            {
                GD.Print("Passed through a checkpoint after finishing race");
                return;
            }

            if (!checkpointMarker.IsInsideTree())
            {
                GD.PushError("Error: checkpoint marker has not been added to a tree");
                return;
            }

            ObjectMarker nextMarker = ObjectMarker.Create(origin, CurrentCheckpoint, new(0, 0, 1));
            checkpointMarker.ReplaceBy(nextMarker);
            checkpointMarker.QueueFree();
            checkpointMarker = nextMarker;
        };

        RaceFinished += (sender, e) =>
        {
            checkpointMarker.QueueFree();
            checkpointMarker = null;
        };
        
        return checkpointMarker;
    }

    #endregion Marker Tracking

    #region Lap counters

    public void AddCounter(Label display)
    {
        display.Text = $"Lap {currentLap}/{maxLaps}";

        NextLap += (sender, lap) =>
        {
            display.Text = $"Lap {currentLap}/{maxLaps}";
        };

        RaceFinished += (sender, e) =>
        {
            display.AddThemeColorOverride(new StringName("font_color"), new(0, 0, 1));
        };
    }

    #endregion Lap counters
}