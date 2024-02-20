using Orleans.Configuration;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Shared.Interface;

namespace OrleansMVCFramework
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IClusterClient grainFactory;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            grainFactory = StartClientWithRetries().Result;
        }


        private static async Task<IClusterClient> StartClientWithRetries()
        {
            Orleans.IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .Build();

            await client.Connect();

            Console.WriteLine("Client successfully connect to silo host");
            return client;
        }
    }
}
