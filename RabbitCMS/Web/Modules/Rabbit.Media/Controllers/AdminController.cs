using Rabbit.Web.Mvc.UI.Admin;
using System.Web.Mvc;

namespace Rabbit.Media.Controllers
{
    [Admin]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}