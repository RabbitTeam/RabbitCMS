using Rabbit.Web.Mvc.Themes;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            return View();
        }
    }
}