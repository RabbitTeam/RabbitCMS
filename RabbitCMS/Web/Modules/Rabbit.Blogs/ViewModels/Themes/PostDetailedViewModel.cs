using Rabbit.Blogs.Models;
using System;

namespace Rabbit.Blogs.ViewModels.Themes
{
    public class PostDetailedViewModel
    {
        public Lazy<PostRecord> BeforePost { get; set; }

        public PostRecord CurrentPost { get; set; }

        public Lazy<PostRecord> AfterPost { get; set; }
    }
}