using Orleans.Runtime;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseOrleans(siloBuilder =>
        {
            //siloBuilder.UseKubernetesHosting();

            //var redisConnectionString = $"redis-master.default.svc.cluster.local:6379";
            //siloBuilder.UseRedisClustering(options => options.ConnectionString = redisConnectionString);
            //siloBuilder.AddRedisGrainStorage("statestore", options => options.ConnectionString = redisConnectionString);

            siloBuilder.UseLocalhostClustering();
            siloBuilder.AddMemoryGrainStorage("statestore");
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