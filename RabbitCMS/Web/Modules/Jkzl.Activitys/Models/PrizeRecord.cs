using Rabbit.Components.Data;
using System.ComponentModel.DataAnnotations;

namespace Jkzl.Activitys.Models
{
    [Entity]
    public class PrizeRecord
    {
        public long Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ImagePath { get; set; }

        [Required, Range(0, 100)]
        public double Probability { get; set; }

        [Range(0, 100000)]
        public int? Total { get; set; }

        public virtual ActivityRecord Activity { get; set; }

        public int Remain { get; set; }
    }
}