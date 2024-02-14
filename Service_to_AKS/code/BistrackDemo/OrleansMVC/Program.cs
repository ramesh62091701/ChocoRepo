using Orleans.Runtime;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using OrleansMVC;
using Orleans.Hosting;
using Microsoft.AspNetCore.Internal;
using Orleans;
using Microsoft.AspNetCore.Hosting.Internal;

internal class Program
{
    public static void Main(string[] args)
    {
        var silobuilder = new SiloHostBuilder()
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("statestore")
            .UseTransactions();

        var silo = silobuilder.Build();
        silo.StartAsync().Wait();

        var host = new WebHostBuilder()
            .UseKestrel() // Use Kestrel server
            .ConfigureServices(servicesCollection =>
            {
                servicesCollection.AddSingleton<IClusterClient>(silo.Services.GetRequiredService<IClusterClient>());
                servicesCollection.AddControllers();
            })
            .Configure(app =>
            {
                app.UseRouting(); // Use routing

                app.UseAuthorization(); // Use authorization

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers(); // Map controllers
                });
            })
            .Build();

        host.Run();
       
    }


}