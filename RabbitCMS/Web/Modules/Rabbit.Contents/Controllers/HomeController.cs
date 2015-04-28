using Rabbit.Web.Mvc.Themes;
using Rabbit.Web.Mvc.UI.Admin;
using System;
using System.Web.Mvc;

namespace Rabbit.Contents.Controllers
{
    [Admin]
    [Themed]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}