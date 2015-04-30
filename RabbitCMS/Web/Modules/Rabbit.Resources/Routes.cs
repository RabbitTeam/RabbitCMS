using Rabbit.Web.Routes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Rabbit.Resources
{
    internal class RouteHandler : IRouteHandler
    {
        private static Dictionary<string, string[]> _mappingFiles;
        private static readonly ConcurrentDictionary<string, string> MappingDictionary = new ConcurrentDictionary<string, string>();

        #region Implementation of IRouteHandler

        /// <summary>
        /// 提供处理请求的对象。
        /// </summary>
        /// <returns>
        /// 一个处理请求的对象。
        /// </returns>
        /// <param name="requestContext">一个对象，封装有关请求的信息。</param>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (_mappingFiles == null)
            {
                lock (this)
                {
                    if (_mappingFiles == null)
                    {
                        var paths = new[]
                            {
                                "/Modules/Rabbit.Resources/Content/font-awesome/fonts",
                                "/Modules/Rabbit.Resources/Content/fonts"
                            };
                        _mappingFiles = paths.ToDictionary(i => i, i =>
                        {
                            var p = requestContext.HttpContext.Server.MapPath(i);
                            return Directory.Exists(p) ? Directory.GetFiles(p) : null;
                        }).Where(i => i.Value != null).ToDictionary(i => i.Key, i => i.Value);
                    }
                }
            }
            var dataKey = "resource";
            var resource = !requestContext.RouteData.Values.ContainsKey(dataKey)
                ? null
                : requestContext.RouteData.GetRequiredString(dataKey);

            var path = string.IsNullOrWhiteSpace(resource) ? null : MappingDictionary.GetOrAdd(resource, k =>
            {
                var item = _mappingFiles.Select(i =>
                {
                    var file = i.Value.FirstOrDefault(
                        z => string.Equals(Path.GetFileName(z), resource, StringComparison.OrdinalIgnoreCase));
                    return new KeyValuePair<string, string>(i.Key, file);
                }).FirstOrDefault(i => !string.IsNullOrEmpty(i.Value));

                return item.Key + "/" + resource;
            });
            return new RedirectHandler(path);
        }

        #endregion Implementation of IRouteHandler

        private class RedirectHandler : IHttpHandler
        {
            private readonly string _path;

            public RedirectHandler(string path)
            {
                _path = path;
            }

            #region Implementation of IHttpHandler

            /// <summary>
            /// 通过实现 <see cref="T:System.Web.IHttpHandler"/> 接口的自定义 HttpHandler 启用 HTTP Web 请求的处理。
            /// </summary>
            /// <param name="context"><see cref="T:System.Web.HttpContext"/> 对象，它提供对用于为 HTTP 请求提供服务的内部服务器对象（如 Request、Response、Session 和 Server）的引用。</param>
            public void ProcessRequest(HttpContext context)
            {
                var response = context.Response;
                if (string.IsNullOrWhiteSpace(_path))
                {
                    response.Clear();
                    response.StatusCode = 404;
                }
                else
                {
                    context.Response.Redirect(_path);
                }
                response.End();
            }

            /// <summary>
            /// 获取一个值，该值指示其他请求是否可以使用 <see cref="T:System.Web.IHttpHandler"/> 实例。
            /// </summary>
            /// <returns>
            /// 如果 <see cref="T:System.Web.IHttpHandler"/> 实例可再次使用，则为 true；否则为 false。
            /// </returns>
            public bool IsReusable { get; private set; }

            #endregion Implementation of IHttpHandler
        }
    }

    internal sealed class Routes : IRouteProvider
    {
        #region Implementation of IRouteProvider

        /// <summary>
        /// 获取路由信息。
        /// </summary>
        /// <param name="routes">路由集合。</param>
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            routes.Add(new RouteDescriptor
            {
                Route = new Route("fonts/{*resource}", new RouteHandler())
                {
                    RouteExistingFiles = true
                }
            });
        }

        #endregion Implementation of IRouteProvider
    }
}