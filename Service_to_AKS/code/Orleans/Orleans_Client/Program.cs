using Orleans.Hosting;
using Orleans.Runtime;
using SharedFiles.Interface;

internal class Program
{
    const int initializeAttemptsBeforeFailing = 5;
    private static int attempt = 0;
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseOrleansClient(client =>
            {
                client.UseLocalhostClustering();
                client.UseConnectionRetryFilter(RetryFilter);
                client.UseTransactions();
            })
            .UseConsoleLifetime();

        //using IHost host = Host.CreateDefaultBuilder(args)
        //.UseOrleansClient(client =>
        //{
        //    client.UseLocalhostClustering()
        //        .UseTransactions();
        //})
        //.UseConsoleLifetime()
        //.Build();

        //await host.StartAsync();
        //var client = host.Services.GetRequiredService<IClusterClient>();
        //var fromAccount = client.GetGrain<IUserGrain>(1);
        //Console.WriteLine(fromAccount);


        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static async Task<bool> RetryFilter(Exception exception, CancellationToken token)
    {
        if (exception.GetType() != typeof(SiloUnavailableException))
        {
            Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
            return false;
        }
        attempt++;
        Console.WriteLine($"Cluster client attempt {attempt} of {initializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
        if (attempt > initializeAttemptsBeforeFailing)
        {
            return false;
        }
        await Task.Delay(TimeSpan.FromSeconds(4));
        return true;
    }
}