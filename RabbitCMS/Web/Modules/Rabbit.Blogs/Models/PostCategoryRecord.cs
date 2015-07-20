using Rabbit.Components.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Blogs.Models
{
    /// <summary>
    /// 分类记录。
    /// </summary>
    [Entity]
    public class PostCategoryRecord : Infrastructures.Data.IEntity
    {
        #region Implementation of IEntity

        /// <summary>
        /// 标识。
        /// </summary>
        [Required, StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        #endregion Implementation of IEntity

        /// <summary>
        /// 标题。
        /// </summary>
        [Required, StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 说明。
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 是否显示。
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 文章集合。
        /// </summary>
        public virtual ICollection<PostRecord> Posts { get; set; }

        /// <summary>
        /// Seo信息。
        /// </summary>
        public SeoModel Seo { get; set; }

        #region Public Method

        /// <summary>
        /// 创建一个分类。
        /// </summary>
        /// <returns>文章分类。</returns>
        public static PostCategoryRecord Create()
        {
            return Create(null);
        }

        /// <summary>
        /// 创建一个分类。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="description">说明。</param>
        /// <param name="visible">是否显示。</param>
        /// <returns>文章分类。</returns>
        public static PostCategoryRecord Create(string title, string description = null, bool visible = true)
        {
            return new PostCategoryRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = title,
                Description = description,
                Posts = new List<PostRecord>(),
                CreateTime = DateTime.Now,
                Seo = new SeoModel()
            }.SetVisible(visible);
        }

        /// <summary>
        /// 设置分类是否可见。
        /// </summary>
        /// <param name="visible">可见为true，否则为false。</param>
        public PostCategoryRecord SetVisible(bool visible)
        {
            Visible = visible;

            return this;
        }

        #endregion Public Method
    }
}