using Rabbit.Contents.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Contents.ViewModels
{
    public class SiteSettingsViewModel
    {
        [DisplayName("站点名称"), Required, StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Logo"), StringLength(500)]
        public string Logo { get; set; }

        [DisplayName("站点主题"), Required, StringLength(50)]
        public string ThemeName { get; set; }

        [DisplayName("版权信息"), StringLength(500)]
        public string Copyright { get; set; }

        public SeoModelFull Seo { get; set; }

        public static explicit operator SiteSettingsViewModel(SiteSettingsRecord record)
        {
            return new SiteSettingsViewModel
            {
                Copyright = record.Copyright,
                Name = record.Name,
                Seo = record.Seo,
                ThemeName = record.ThemeName,
                Logo = record.Logo
            };
        }

        public SiteSettingsRecord Set(SiteSettingsRecord record)
        {
            record.Seo = Seo;
            record.Copyright = Copyright;
            record.Name = Name;
            record.ThemeName = ThemeName;
            record.Logo = Logo;

            return record;
        }
    }
}