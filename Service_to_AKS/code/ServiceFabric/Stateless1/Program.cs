using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Stateless1
{
    internal static class Program
    {
        static Program()
        {
            SFBinaryLoader.Initialize();
        }
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static async Task Main()
        {
            var actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IMyactor>(actorId, new Uri("fabric:/Stateless1/Stateless1Type"));

            var result = await actor.GetNameAsync();
            Console.WriteLine(result);
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                ServiceRuntime.RegisterServiceAsync("Stateless1Type",
                    context => new Stateless1(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Stateless1).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
