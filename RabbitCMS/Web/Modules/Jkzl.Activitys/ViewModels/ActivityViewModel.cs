using System;

namespace Jkzl.Activitys.ViewModels
{
    public class ActivityViewModel
    {
        public string Description { get; set; }
        public PrizeViewModel[] Prizes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}