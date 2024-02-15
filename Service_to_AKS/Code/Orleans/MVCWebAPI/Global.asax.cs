using MVCWebAPI.Grains;
using MVCWebAPI.Interface;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MVCWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            var client = new HostBuilder()
            .UseOrleansClient((context, clientBuilder) =>
            {
                clientBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "my-first-cluster";
            options.ServiceId = "MyOrleansService";
        })
        .UseAzureStorageClustering(
            options => options.ConfigureTableServiceClient(
                context.Configuration["ORLEANS_AZURE_STORAGE_CONNECTION_STRING"]));
    })
    .Build();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var host = new SiloHostBuilder()
            .UseLocalhostClustering()
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
            .ConfigureApplicationParts(parts => parts.AddApplicationPart(this.GetType().Assembly).WithReferences())
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "OrleansBasics";
            })
            .Build();

            await host.StartAsync();

        }

    }
}
