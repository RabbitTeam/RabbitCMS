using Rabbit.Components.Data.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Blogs.Models
{
    [ComplexType]
    public sealed class SeoModel
    {
        /// <summary>
        /// 关键词。
        /// </summary>
        [DisplayName("页面关键词"), StringLength(255)]
        public string Keywords { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [DisplayName("页面描述"), StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// 友好的路由地址。
        /// </summary>
        [DisplayName("路由路径"), Required, StringLength(50)]
        public string RoutePath { get; set; }
    }
}