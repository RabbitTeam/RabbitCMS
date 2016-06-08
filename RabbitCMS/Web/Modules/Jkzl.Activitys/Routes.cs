using Rabbit.Web;
using Rabbit.Web.Routes;
using System.Collections.Generic;

namespace Jkzl.Activitys
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
            const string area = "Jkzl.Activitys";

            /*            routes.Add(new HttpRouteDescriptor
                        {
                            Name = "Rabbit.Blogs_CommentsApi",
                            RouteTemplate = "blogs/api/comments/{id}",
                            Defaults = new { area, controller = "Comment", pageIndex = 1, pageSize = 10 }
                        });

                        routes.Add(new HttpRouteDescriptor
                        {
                            Name = "Rabbit.Blogs_PostCommentsApi",
                            RouteTemplate = "blogs/api/post/{postId}/comments/{pageIndex}/{pageSize}",
                            Defaults = new { area, controller = "Comment", pageIndex = 1, pageSize = 10 }
                        });*/

            routes.MapRabbitRoute("Jkzl.Activitys_Home", string.Empty,
                area, new { controller = "DaZhuanPan", action = "Index" });
            routes.MapRabbitRoute("Jkzl.Activitys_Default", "activitys/{controller}/{action}/{id}",
                area, new { controller = "Home", action = "Index", id = string.Empty });
        }

        #endregion Implementation of IRouteProvider
    }
}