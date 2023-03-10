using System;

namespace stats.Components
{
    [Serializable]
    public class StatsData
    {
        public int Tags;
        public int Tagged;
        public int HuntWins;
        public float Lifetime;

        public StatsData(MainBoard stats)
        {
            Tags = stats.Tags;
            Tagged = stats.Tagged;
            HuntWins = stats.roundswon;
            Lifetime = (float)stats.Timelifetime;
        }
    }
}
