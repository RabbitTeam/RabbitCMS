using Rabbit.Web;
using Rabbit.Web.Routes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rabbit.Blogs
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
            const string area = "Rabbit.Blogs";

            routes.MapRabbitRoute("Rabbit.Blogs_Home", string.Empty,
                area, new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRabbitRoute("Rabbit.Blogs_Categorys", "category/{RoutePath}",
                area, new { controller = "Category", action = "Index" });

            routes.MapRabbitRoute("Rabbit.Blogs_Authors", "author/{author}",
                area, new { controller = "Author", action = "Index" });

            routes.MapRabbitRoute("Rabbit.Blogs_Tags", "tag/{tag}",
                area, new { controller = "Tag", action = "Index", tag = UrlParameter.Optional });

            routes.MapRabbitRoute("Rabbit.Blogs_Posts", "{CategoryRoutePath}/{RoutePath}",
                area, new { controller = "Post", action = "Index", CategoryRoutePath = UrlParameter.Optional });

            routes.MapRabbitRoute("Rabbit.Blogs_Default", "Admin/Blogs/{controller}/{action}/{id}",
                area, new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion Implementation of IRouteProvider
    }
}