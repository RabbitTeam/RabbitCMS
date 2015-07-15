using Rabbit.Web.Mvc.UI.Admin;
using System.Web.Mvc;

namespace Rabbit.Blogs.Controllers
{
    [Admin]
    public class PostAdminController : Controller
    {
        // GET: PostAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}