using Rabbit.Components.Data;
using System;
using System.Collections.Generic;

namespace Jkzl.Activitys.Models
{
    [Entity]
    public class ActivityRecord
    {
        public long Id { get; set; }
        public virtual ICollection<PrizeRecord> Prizes { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }

        public int? MaxLotteryCount { get; set; }
    }
}