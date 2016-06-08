using Rabbit.Components.Data;
using System;

namespace Jkzl.Activitys.Models
{
    [Entity]
    public class OrderRecord
    {
        public long Id { get; set; }
        public virtual AppUserRecord User { get; set; }
        public virtual PrizeRecord Prize { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsReceive { get; set; }
    }
}