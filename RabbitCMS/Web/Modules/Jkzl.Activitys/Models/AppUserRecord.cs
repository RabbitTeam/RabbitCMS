using Rabbit.Components.Data;
using System.ComponentModel.DataAnnotations;

namespace Jkzl.Activitys.Models
{
    [Entity]
    public class AppUserRecord
    {
        public long Id { get; set; }

        [Required, StringLength(11)]
        public string MobileNumber { get; set; }

        public int LotteryCount { get; set; }
    }
}