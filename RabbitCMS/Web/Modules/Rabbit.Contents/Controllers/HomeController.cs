using System;
using System.Web.Mvc;

namespace Rabbit.Contents.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}