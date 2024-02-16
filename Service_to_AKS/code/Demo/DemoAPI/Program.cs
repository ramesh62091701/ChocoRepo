using Orleans.Runtime;

internal class Program
{
    const int initializeAttemptsBeforeFailing = 5;
    private static int attempt = 0;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.UseOrleansClient(client =>
        {
            client.UseLocalhostClustering();
            client.UseConnectionRetryFilter(RetryFilter);
            client.UseTransactions();
        })
                    .UseConsoleLifetime();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

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