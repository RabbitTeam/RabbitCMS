using Rabbit.Components.Data;
using Rabbit.Components.Data.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Contents.Models
{
    [Entity]
    public class SiteSettingsRecord
    {
        [Required]
        public string Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }

        [Required, StringLength(50)]
        public string ThemeName { get; set; }

        [StringLength(500)]
        public string Copyright { get; set; }

        public SeoModelFull Seo { get; set; }
    }

    [ComplexType]
    public sealed class SeoModelFull
    {
        [DisplayName("站点标题"), StringLength(255)]
        public string Title { get; set; }

        [DisplayName("站点关键词"), StringLength(255)]
        public string Keywords { get; set; }

        [DisplayName("站点描述"), StringLength(255)]
        public string Description { get; set; }
    }
}