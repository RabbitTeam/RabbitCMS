using Rabbit.Blogs.Services.Themes;
using Rabbit.Web.Mvc.Themes;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class PostController : Controller
    {
        private readonly IThemePostService _postService;

        public PostController(IThemePostService postService)
        {
            _postService = postService;
        }

        public ActionResult Index(string categoryRoutePath, string routePath)
        {
            if (string.IsNullOrEmpty(routePath))
                return HttpNotFound();

            var model = _postService.Read(routePath, categoryRoutePath);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }
    }
}