using Rabbit.Components.Data;
using Rabbit.Users.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Blogs.Models
{
    /// <summary>
    /// 文章记录。
    /// </summary>
    [Entity]
    public class PostRecord : Infrastructures.Data.IEntity
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
        [Required]
        public DateTime CreateTime { get; set; }

        #endregion Implementation of IEntity

        /// <summary>
        /// 标题。
        /// </summary>
        [Required, StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 内容。
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 摘要。
        /// </summary>
        [StringLength(5000)]
        public string Summary { get; set; }

        /// <summary>
        /// 分类。
        /// </summary>
        public virtual ICollection<PostCategoryRecord> Categorys { get; set; }

        /// <summary>
        /// 评论。
        /// </summary>
        public virtual ICollection<PostCommentRecord> Comments { get; set; }

        /// <summary>
        /// 发布者。
        /// </summary>
        public virtual UserRecord User { get; set; }

        /// <summary>
        /// 文章状态。
        /// </summary>
        public PostStatus Status { get; set; }

        /// <summary>
        /// 是否显示在首页。
        /// </summary>
        public bool ShowInIndex { get; set; }

        /// <summary>
        /// 是否允许评论。
        /// </summary>
        public bool AllowComment { get; set; }

        /// <summary>
        /// 标签。
        /// </summary>
        [StringLength(200)]
        public string Tags { get; set; }

        /// <summary>
        /// 推荐数。
        /// </summary>
        [Range(0, int.MaxValue)]
        public int RecommendationCount { get; set; }

        /// <summary>
        /// 反对数。
        /// </summary>
        [Range(0, int.MaxValue)]
        public int OppositionCount { get; set; }

        /// <summary>
        /// 阅读数。
        /// </summary>
        [Range(0, int.MaxValue)]
        public int ReadingCount { get; set; }

        /// <summary>
        /// Seo信息。
        /// </summary>
        public SeoModel Seo { get; set; }

        #region Public Method

        public static PostRecord Create(UserRecord user)
        {
            return new PostRecord
            {
                AllowComment = true,
                Categorys = new List<PostCategoryRecord>(),
                Comments = new List<PostCommentRecord>(),
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid().ToString("N"),
                ShowInIndex = true,
                User = user,
                Seo = new SeoModel()
            };
        }

        /// <summary>
        /// 发布文章。
        /// </summary>
        /// <returns>文章。</returns>
        public PostRecord Publish()
        {
            Status = PostStatus.Publish;

            return this;
        }

        /// <summary>
        /// 未发布文章。
        /// </summary>
        /// <returns>文章。</returns>
        public PostRecord UnPublished()
        {
            Status = PostStatus.UnPublished;

            return this;
        }

        public PostRecord Read()
        {
            ReadingCount = ReadingCount + 1;

            return this;
        }

        public PostRecord Recommend()
        {
            RecommendationCount = RecommendationCount + 1;

            return this;
        }

        public PostRecord Oppose()
        {
            OppositionCount = OppositionCount + 1;

            return this;
        }

        #endregion Public Method
    }

    /// <summary>
    /// 文章状态。
    /// </summary>
    public enum PostStatus
    {
        /// <summary>
        /// 未发布。
        /// </summary>
        UnPublished = 0,

        /// <summary>
        /// 已发布。
        /// </summary>
        Publish = 1
    }
}