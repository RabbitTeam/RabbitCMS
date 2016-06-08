using Jkzl.Activitys.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jkzl.Activitys.ViewModels
{
    public class PrizeViewModel
    {
        public long Id { get; set; }

        [DisplayName("奖品名称"), Required, StringLength(20)]
        public string Name { get; set; }

        [DisplayName("奖品图片"), StringLength(500)]
        public string ImagePath { get; set; }

        [DisplayName("命中概率"), Required, Range(0, 100)]
        public double Probability { get; set; }

        [DisplayName("奖品总数"), Range(0, 100000)]
        public int? Total { get; set; }

        [DisplayName("剩余数"), Range(0, 100000)]
        public int Remain { get; set; }

        public static explicit operator PrizeViewModel(PrizeRecord record)
        {
            return new PrizeViewModel
            {
                Id = record.Id,
                ImagePath = record.ImagePath,
                Name = record.Name,
                Probability = record.Probability,
                Total = record.Total,
                Remain = record.Remain
            };
        }

        public void Update(PrizeRecord record)
        {
            record.Total = Total;
            record.ImagePath = ImagePath;
            record.Name = Name;
            record.Probability = Probability;
            record.Remain = Remain;
        }
    }
}