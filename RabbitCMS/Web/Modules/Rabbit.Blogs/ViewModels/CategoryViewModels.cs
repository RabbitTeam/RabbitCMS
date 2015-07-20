using Rabbit.Blogs.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Blogs.ViewModels
{
    public sealed class CategoryListViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool Visible { get; set; }

        public int PostCount { get; set; }

        public SeoModel Seo { get; set; }
    }

    public sealed class CategoryEditViewModel
    {
        public CategoryEditViewModel()
        {
            Seo = new SeoModel();
        }

        /// <summary>
        /// 标识。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [DisplayName("标题"), Required, StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 说明。
        /// </summary>
        [DisplayName("说明"), StringLength(500)]
        public string Description
        {
            get { return Seo.Description; }
            set { Seo.Description = value; }
        }

        /// <summary>
        /// 是否显示。
        /// </summary>
        [DisplayName("是否显示")]
        public bool Visible { get; set; }

        public SeoModel Seo { get; set; }
    }
}