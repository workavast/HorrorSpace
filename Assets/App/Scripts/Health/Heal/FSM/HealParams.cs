using System;

namespace App.Health.Heal.FSM
{
    [Serializable]
    public class HealParams
    {
        public float HealPerSecond = 1f;
        public float FrequencyInSeconds = 10f;
        public float DelayBeforeHealing = 10f;
    }
}