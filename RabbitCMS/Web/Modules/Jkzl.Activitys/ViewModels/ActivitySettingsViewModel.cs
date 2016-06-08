using Jkzl.Activitys.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jkzl.Activitys.ViewModels
{
    public class ActivitySettingsViewModel
    {
        [DisplayName("开始时间")]
        public DateTime StartTime { get; set; }

        [DisplayName("结束时间（留空为永不结束）")]
        public DateTime? EndTime { get; set; }

        [DisplayName("活动说明"), Required]
        public string Description { get; set; }

        [DisplayName("每人最大抽奖次数（留空为无限）"), Range(1, 10000)]
        public int? MaxLotteryCount { get; set; }

        public static explicit operator ActivitySettingsViewModel(ActivityRecord record)
        {
            return new ActivitySettingsViewModel
            {
                Description = record.Description,
                EndTime = record.EndTime,
                MaxLotteryCount = record.MaxLotteryCount,
                StartTime = record.StartTime
            };
        }

        public void Update(ActivityRecord record)
        {
            record.StartTime = StartTime;
            record.Description = Description;
            record.EndTime = EndTime;
            record.MaxLotteryCount = MaxLotteryCount;
        }
    }
}