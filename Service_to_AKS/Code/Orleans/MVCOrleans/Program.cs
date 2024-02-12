using Orleans.Runtime;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseOrleans(siloBuilder =>
        {
            siloBuilder
                .UseLocalhostClustering()
                .AddMemoryGrainStorageAsDefault()
                .UseTransactions();
        });

        // Add services to the container.

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}