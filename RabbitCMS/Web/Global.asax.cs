using Autofac;
using Rabbit.Components.Data;
using Rabbit.Components.Data.EntityFramework;
using Rabbit.Components.Data.Migrators;
using Rabbit.Components.Data.Mvc;
using Rabbit.Components.Logging.NLog;
using Rabbit.Components.Security.Web;
using Rabbit.Kernel;
using Rabbit.Kernel.Caching;
using Rabbit.Kernel.Caching.Impl;
using Rabbit.Kernel.Logging;
using Rabbit.Resources;
using Rabbit.Web;
using Rabbit.Web.Environment;
using Rabbit.Web.Mvc;
using Rabbit.Web.WarmupStarter;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class Global : HttpApplication
    {
        private static Starter<IWebHost> _starter;

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            _starter = new Starter<IWebHost>(HostInitialization, HostBeginRequest, HostEndRequest);
            _starter.OnApplicationStart(this);
        }

        #region Event

        protected void Application_BeginRequest()
        {
            _starter.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            _starter.OnEndRequest(this);
        }

        protected void Session_Start()
        {
            Session["temp"] = null;
        }

        #endregion Event

        #region Private Method

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        private static void HostBeginRequest(HttpApplication application, IWebHost host)
        {
            application.Context.Items["originalHttpContext"] = application.Context;
            host.BeginRequest();
        }

        private static void HostEndRequest(HttpApplication application, IWebHost host)
        {
            host.EndRequest();
        }

        private static IWebHost HostInitialization(HttpApplication application)
        {
            var kernelBuilder = new KernelBuilder();

            kernelBuilder.UseCaching(c => c.UseMemoryCache());
            kernelBuilder.UseLogging(c => c.UseNLog());
            kernelBuilder.UseWeb(c => c.EnableMvc().EnableSecurity());
            kernelBuilder.UseData(c =>
            {
                c.UseEntityFramework();
                c.EnableMvcFilterTransaction();

                //如果开启了EntityFramework的自动迁移则不启动数据迁移。
                var automaticMigrationsEnabled = ConfigurationManager.AppSettings["Data.EntityFramework.AutomaticMigrationsEnabled"];
                if (!string.Equals(automaticMigrationsEnabled, "true", StringComparison.OrdinalIgnoreCase))
                    c.EnableDataMigrators();
            });

            kernelBuilder.UseResources();

            var container = kernelBuilder.Build();

            var host = container.Resolve<IWebHost>();

            host.Initialize();
            host.BeginRequest();
            host.EndRequest();

            return host;
        }

        #endregion Private Method
    }
}