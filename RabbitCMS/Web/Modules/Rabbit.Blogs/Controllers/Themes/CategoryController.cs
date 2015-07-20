using Rabbit.Web.Mvc.Themes;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
    }
}