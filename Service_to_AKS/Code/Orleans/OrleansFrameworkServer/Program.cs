using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OServer.Grains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace OrleansFrameworkServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new SiloHostBuilder()
           .UseLocalhostClustering()
   
           .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
           .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
           .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "OrleansBasics";
            })
           .ConfigureServices(services =>
           {
               services.Configure<ConsoleLifetimeOptions>(options =>
               {
                   options.SuppressStatusMessages = true;
               });
           })
             .ConfigureLogging(builder =>
             {
                 builder.AddConsole();
             })
           .Build();
            host.StartAsync().Wait();
            Console.ReadLine();
        }
    }
}
