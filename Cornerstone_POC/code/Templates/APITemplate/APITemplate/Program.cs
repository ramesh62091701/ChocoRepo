using APITemplate.BFF;
using APITemplate.DataRepo;
using APITemplate.DataServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBFFService , BFFService>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
@*#if(EnableSwaggerSupport)
builder.Services.AddSwaggerGen();
#endif*@
var app = builder.Build();

// Configure the HTTP request pipeline.
@*#if(EnableSwaggerSupport)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endif*@
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
