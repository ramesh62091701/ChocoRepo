using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddActors(options =>
{
    // Register actor types and configure actor settings
    options.Actors.RegisterActor<MVC.Actors.ControllerActor>();
    options.Actors.RegisterActor<MVC.Actors.SmokeDetectorActor>();
    options.ReentrancyConfig = new Dapr.Actors.ActorReentrancyConfig()
    {
        Enabled = true,
        MaxStackDepth = 32,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.MapControllers();
app.MapActorsHandlers();


app.Run();
