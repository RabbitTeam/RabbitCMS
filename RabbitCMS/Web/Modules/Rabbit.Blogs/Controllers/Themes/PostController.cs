using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services.Themes;
using Rabbit.Blogs.ViewModels.Themes;
using Rabbit.Infrastructures.Data;
using Rabbit.Web.Mvc.Themes;
using System;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class PostController : Controller
    {
        #region Field

        private readonly IThemePostService _postService;

        #endregion Field

        #region Constructor

        public PostController(IThemePostService postService)
        {
            _postService = postService;
        }

        #endregion Constructor

        #region Action

        public ActionResult ListHome(int pageIndex)
        {
            return List(pageIndex, _postService.GetHomeList());
        }

        public ActionResult ListCategorys(string routePath, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByCategory(routePath));
        }

        public ActionResult ListTags(string tag, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByTag(tag));
        }

        public ActionResult ListAuthor(string author, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByAuthor(author));
        }

        public ActionResult ListSearch(string titleKeywords, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByTitleKeywords(titleKeywords), true);
        }

        public ActionResult Detailed(string categoryRoutePath, string routePath)
        {
            if (string.IsNullOrEmpty(routePath))
                return HttpNotFound();

            if (string.Equals(categoryRoutePath, "unclassified", StringComparison.OrdinalIgnoreCase))
                categoryRoutePath = null;

            var post = _postService.Read(routePath, categoryRoutePath);
            if (post == null)
                return HttpNotFound();

            return
                View(new PostDetailedViewModel
                {
                    CurrentPost = post,
                    AfterPost = new Lazy<PostRecord>(() => _postService.GetAfterPost(post)),
                    BeforePost = new Lazy<PostRecord>(() => _postService.GetBeforePost(post))
                });
        }

        #endregion Action

        #region Private Method

        private ActionResult List(int pageIndex, IQueryable<PostRecord> queryable, bool allowNotData = false)
        {
            if (pageIndex <= 0)
                return HttpNotFound();

            var posts = queryable;
            if (posts == null)
                return HttpNotFound();

            if (!posts.Any() && allowNotData)
                return View("List");

            dynamic model = new ExpandoObject();

            var pageParameter = new PageParameter(pageIndex - 1, 10);
            posts = pageParameter.Paged(posts);

            model.PostPage = new ExpandoObject();
            model.PostPage.PageCount = pageParameter.PageCount;
            model.PostPage.PageIndex = pageParameter.PageIndex;
            model.PostPage.PageSize = pageParameter.PageSize;
            model.PostPage.Posts = posts;

            if (pageIndex > pageParameter.PageCount || !posts.Any())
                return HttpNotFound();

            return View("List", model);
        }

        #endregion Private Method
    }
}