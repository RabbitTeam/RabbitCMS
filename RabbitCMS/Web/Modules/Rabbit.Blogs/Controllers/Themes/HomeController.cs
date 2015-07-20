using Rabbit.Blogs.Services.Themes;
using Rabbit.Infrastructures.Data;
using Rabbit.Web.Mvc.Themes;
using System.Dynamic;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class HomeController : Controller
    {
        private readonly IThemeCategoryService _categoryService;
        private readonly IThemePostService _postService;

        public HomeController(IThemeCategoryService categoryService, IThemePostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();

            model.Categorys = _categoryService.GetList();

            var pageParameter = new PageParameter(0, 10);
            var posts = pageParameter.Paged(_postService.GetHomeList());

            model.PostPage = new ExpandoObject();
            model.PostPage.PageCount = pageParameter.PageCount;
            model.PostPage.PageIndex = pageParameter.PageIndex;
            model.PostPage.PageSize = pageParameter.PageSize;
            model.PostPage.Posts = posts;

            return View(model);
        }
    }
}