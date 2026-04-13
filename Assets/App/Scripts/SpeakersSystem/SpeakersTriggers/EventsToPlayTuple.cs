using System;

namespace App.SpeakersSystem.SpeakersTriggers
{
    [Serializable]
    public struct EventsToPlayTuple
    {
        public string EventName;
        public bool State;
    }
}