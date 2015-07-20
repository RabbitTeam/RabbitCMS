using Rabbit.Blogs.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rabbit.Blogs.ViewModels
{
    public class PostListViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public PostStatus Status { get; set; }

        public int CommentCount { get; set; }

        public int ReadingCount { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class PostEditViewModel
    {
        public PostEditViewModel()
        {
            Seo = new SeoModel();
        }

        public string Id { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [DisplayName("标题"), Required, StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 内容。
        /// </summary>
        [DisplayName("内容"), Required]
        public string Content { get; set; }

        /// <summary>
        /// 摘要。
        /// </summary>
        [DisplayName("摘要"), StringLength(5000)]
        public string Summary { get; set; }

        public bool IsPublish { get; set; }

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
        [DisplayName("Tag标签"), StringLength(200)]
        public string Tags { get; set; }

        /// <summary>
        /// 推荐数。
        /// </summary>
        [DisplayName("推荐数"), Range(0, int.MaxValue)]
        public int RecommendationCount { get; set; }

        /// <summary>
        /// 反对数。
        /// </summary>
        [DisplayName("反对数"), Range(0, int.MaxValue)]
        public int OppositionCount { get; set; }

        /// <summary>
        /// 阅读数。
        /// </summary>
        [DisplayName("阅读数"), Range(0, int.MaxValue)]
        public int ReadingCount { get; set; }

        public string Categorys { get; set; }

        public SeoModel Seo { get; set; }

        public static explicit operator PostEditViewModel(PostRecord record)
        {
            return new PostEditViewModel
            {
                Id = record.Id,
                AllowComment = record.AllowComment,
                Categorys = string.Join(",", record.Categorys.Select(i => i.Id).ToArray()),
                Content = record.Content,
                OppositionCount = record.OppositionCount,
                ReadingCount = record.ReadingCount,
                RecommendationCount = record.RecommendationCount,
                ShowInIndex = record.ShowInIndex,
                IsPublish = record.Status == PostStatus.Publish,
                Summary = record.Summary,
                Tags = record.Tags,
                Title = record.Title,
                Seo = record.Seo
            };
        }
    }
}