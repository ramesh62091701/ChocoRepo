using Akka.Actor;
using AkkaDemo2.Actor;
using AkkaDemo2.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AkkaDemo2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainActor>();
            var serviceProvider = services.BuildServiceProvider();

            Trace.Listeners.Add(new TextWriterTraceListener(Server.MapPath("~/App_Data/trace.log"))); // Log to a file
            Trace.AutoFlush = true;


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
