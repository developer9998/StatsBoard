using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace stats
{
    [System.Serializable]
    public class statsData
    {
        public int Tags;
        public int Tagged;
        public int huntwins;
        public double TodleTimne;
  
        public statsData(conpontes.statsbored stats) { 
        Tags = stats.Tags;
        Tagged = stats.Tagged;
        huntwins = stats.roundswon;
            TodleTimne = stats.Timelifetime;
        }
    }

}
