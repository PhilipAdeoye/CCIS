using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCMvc.ViewModels
{
    public class SplitViewModel
    {
        public long RunnerRaceRecordSegmentId { get; set; }
        public int ElapsedTimeInSeconds { get; set; }
        public int IntervalFromPriorSplit { get; set; }

        public string ElapsedTimeDisplay
        {
            get
            {
                return (ElapsedTimeInSeconds / 60).ToString("D2") + ":" + (ElapsedTimeInSeconds % 60).ToString("D2");
            }
        }

        public string IntervalDisplay
        {
            get
            {
                return (IntervalFromPriorSplit / 60).ToString("D2") + ":" + (IntervalFromPriorSplit % 60).ToString("D2");
            }
        }
    }
}