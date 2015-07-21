using Rabbit.Blogs.Services.Themes;
using Rabbit.Infrastructures.Data;
using Rabbit.Web.Mvc.Themes;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class CategoryController : Controller
    {
        private readonly IThemeCategoryService _categoryService;
        private readonly IThemePostService _postService;

        public CategoryController(IThemeCategoryService categoryService, IThemePostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        public ActionResult Index(string routePath, int pageIndex)
        {
            if (pageIndex <= 0)
                return HttpNotFound();

            var posts = routePath == null ? _postService.GetHomeList() : _postService.GetListByCategory(routePath);
            if (posts == null)
                return HttpNotFound();
            dynamic model = new ExpandoObject();

            model.Categorys = _categoryService.GetList();

            var pageParameter = new PageParameter(pageIndex - 1, 10);
            posts = pageParameter.Paged(posts);

            model.PostPage = new ExpandoObject();
            model.PostPage.PageCount = pageParameter.PageCount;
            model.PostPage.PageIndex = pageParameter.PageIndex;
            model.PostPage.PageSize = pageParameter.PageSize;
            model.PostPage.Posts = posts;

            if (pageIndex > pageParameter.PageCount || !posts.Any())
                return HttpNotFound();

            return View(model);
        }
    }
}