using Rabbit.Blogs.Models;
using Rabbit.Blogs.Services.Themes;
using Rabbit.Blogs.ViewModels.Themes;
using Rabbit.Contents.Models;
using Rabbit.Contents.Services;
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
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly IThemeCategoryService _categoryService;

        #endregion Field

        #region Constructor

        public PostController(IThemePostService postService, ISiteSettingsService siteSettingsService, IThemeCategoryService categoryService)
        {
            _postService = postService;
            _siteSettingsService = siteSettingsService;
            _categoryService = categoryService;
        }

        #endregion Constructor

        #region Action

        public ActionResult ListHome(int pageIndex)
        {
            ViewBag.AppendTenantName = false;

            var siteSettings = _siteSettingsService.Get();
            return List(pageIndex, _postService.GetHomeList(), siteSettings.Seo);
        }

        public ActionResult ListCategorys(string routePath, int pageIndex)
        {
            var category = _categoryService.Get(routePath);
            if (category == null)
                return HttpNotFound();

            return List(pageIndex, _postService.GetListByCategory(routePath),
                new SeoModelFull
                {
                    Description = category.Seo.Description,
                    Keywords = category.Seo.Keywords,
                    Title = category.Title
                });
        }

        public ActionResult ListTags(string tag, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByTag(tag), new SeoModelFull
            {
                Description = $"与标签 {tag} 相关联的文章列表",
                Keywords = tag,
                Title = "标签 " + tag
            });
        }

        public ActionResult ListAuthor(string author, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByAuthor(author), new SeoModelFull
            {
                Description = $"与作者 {author} 相关联的文章列表",
                Keywords = author,
                Title = "作者 " + author
            });
        }

        public ActionResult ListSearch(string titleKeywords, int pageIndex)
        {
            return List(pageIndex, _postService.GetListByTitleKeywords(titleKeywords), new SeoModelFull
            {
                Title = $"搜索结果 {titleKeywords}",
                Keywords = titleKeywords,
                Description = $"搜索结果 {titleKeywords}"
            }, true);
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

        private ActionResult List(int pageIndex, IQueryable<PostRecord> queryable, SeoModelFull seo, bool allowNotData = false)
        {
            if (pageIndex <= 0)
                return HttpNotFound();

            var posts = queryable;
            if (posts == null)
                return HttpNotFound();

            dynamic model = new ExpandoObject();
            model.Seo = seo;
            model.PostPage = null;

            if (!posts.Any() && allowNotData)
                return View("List", model);

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