using Rabbit.Web.Mvc.Themes;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers.Themes
{
    [Themed]
    public class TagController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}