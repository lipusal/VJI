using System;
using FrameLord;

namespace Game.Events
{
    public class PointEvent : GameEvent
    {
        public const string NAME = "Game.Events.PointEvent";
        
        private string[] newCurrentGamePoints;
        private string[] newFullPoints;

        public PointEvent(string[] newCurrentGamePoints, string[] newFullPoints) : base(NAME)
        {
            this.newCurrentGamePoints = newCurrentGamePoints;
            this.newFullPoints = newFullPoints;
        }

        public string[] NewCurrentGamePoints => newCurrentGamePoints;
        public string[] NewFullPoints => newFullPoints;
    }
}