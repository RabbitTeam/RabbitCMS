using Rabbit.Blogs.Models;
using System;

namespace Rabbit.Blogs.ViewModels
{
    public class CommentListViewModel
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string PostTitle { get; set; }

        public static explicit operator CommentListViewModel(PostCommentRecord record)
        {
            return new CommentListViewModel
            {
                Id = record.Id,
                NickName = record.NickName,
                Content = record.Content,
                CreateTime = record.CreateTime,
                PostTitle = record.Post.Title
            };
        }
    }
}