using System.Web.Mvc;

namespace Rabbit.Infrastructures.Mvc
{
    public static class AjaxResultExtensions
    {
        public static JsonResult Error(this IController controller, string errorMessage = null)
        {
            return new JsonResult { Data = new { success = false, message = errorMessage } };
        }

        public static JsonResult Success(this IController controller, object result = null)
        {
            return new JsonResult { Data = new { success = true, result } };
        }
    }
}