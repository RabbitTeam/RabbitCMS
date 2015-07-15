using Rabbit.Components.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Blogs.Models
{
    /// <summary>
    /// 文章评论。
    /// </summary>
    [Entity]
    public class PostCommentRecord : Infrastructures.Data.IEntity
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
        /// 昵称。
        /// </summary>
        [Required, StringLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 内容。
        /// </summary>
        [Required, StringLength(8000)]
        public string Content { get; set; }

        /// <summary>
        /// 文章。
        /// </summary>
        public virtual PostRecord Post { get; set; }

        public static PostCommentRecord Create(string nickName, string content, PostRecord post)
        {
            return new PostCommentRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                NickName = nickName,
                Content = content,
                CreateTime = DateTime.Now,
                Post = post
            };
        }
    }
}