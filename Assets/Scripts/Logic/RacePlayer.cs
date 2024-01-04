using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Data;
using Sanicball.Gameplay;
using Sanicball.UI;

namespace Sanicball.Logic
{
    public interface IRaceReport
    {
        public static readonly Color finishedColor = new(0f, 0.5f, 1f);
        
        string AsTime();
        string AsPosition();
        string AsLap();
        Color ToColor();
        IEnumerable<IRaceReport> Sort();
    }

    public class RaceFinishReport : IRaceReport
    {
        /// <summary>
        /// Finishing with this position means the player has been disqualified.
        /// </summary>
        public const int DISQUALIFIED_POS = -1;

        private readonly TimeSpan time;
        public readonly int position;

        public bool Disqualified => position == DISQUALIFIED_POS;

        public RaceFinishReport(int position, TimeSpan time)
        {
            this.position = position;
            this.time = time;
        }

        public string AsTime()
        {
            return $"{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds:000}";
        }

        public string AsPosition()
        {
            if (Disqualified) return "Disqualified";
            return position + (position % 10, position % 100) switch
            {
                (1, not 11) => "st",
                (2, not 12) => "nd",
                (3, not 13) => "rd",
                _ => "th"
            };
        }

        public string AsLap()
        {
            if (Disqualified) return "Disqualified";
            return "Race Finished";
        }

        public Color ToColor()
        {
            if (Disqualified) return Color.red;
            return IRaceReport.finishedColor;
        }

        public IEnumerable<IRaceReport> Sort()
        {
            throw new NotImplementedException();
        }
    }

    

    //! Figure out to to make PlayerUI not need most of this
    [Serializable] //This is so the list of race players can be viewed in the inspector
    public class RacePlayer
    {
        //Checkpoint related stuff
        private static readonly Checkpoint[] checkpoints = StageReferences.Checkpoints;
        private int currentCheckpointIndex;
        public int Lap => (int)Math.Floor((double)(currentCheckpointIndex / checkpoints.Length)) + 1; // PlayerUI

        //Race progress properties
        public RaceFinishReport FinishReport { get; private set; }
        public bool RaceFinished => FinishReport != null; //PlayerUI + RaceManager
        
        //Readonly properties that get stuff from the player's ball
        public Rigidbody Ball { get; private set; }

        public float Speed => Ball.GetComponent<Rigidbody>().velocity.magnitude; // Speed counter
    }
}