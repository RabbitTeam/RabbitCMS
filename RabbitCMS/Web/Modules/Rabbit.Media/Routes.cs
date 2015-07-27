using Rabbit.Web;
using Rabbit.Web.Routes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rabbit.Media
{
    internal sealed class Routes : IRouteProvider
    {
        #region Implementation of IRouteProvider

        /// <summary>
        /// 获取路由信息。
        /// </summary>
        /// <param name="routes">路由集合。</param>
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            const string area = "Rabbit.Media";

            routes.MapRabbitRoute("Rabbit.Media_Default", "Admin/Media/{controller}/{action}/{id}",
                area, new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRabbitRoute("Rabbit.Media_Home", "Admin",
                area, new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion Implementation of IRouteProvider
    }
}