using Azure.Data.Tables;
using Functions.Interfaces;
using Functions.Models;
using Functions.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<ISettingService, SettingService>();
        builder.Services.AddSingleton<IServiceBusService, ServiceBusService>();

        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.AddTransient(provider =>
        {
            return new Func<ITableStorageService<OrderEntity>>(
                () =>
                {
                    var config = provider.GetRequiredService<IConfiguration>();
                    var serviceClient = new TableServiceClient(config[SettingPropertyNames.AzureWebJobsStorage]);
                    return new TableStorageService<OrderEntity>(serviceClient, config[SettingPropertyNames.AzureStorageTableName], new Microsoft.Extensions.Logging.LoggerFactory());
                }
            );
        });

        builder.Services.AddTransient(provider =>
        {
            return new Func<ITableStorageService<OrderEnrichedEntity>>(
                () =>
                {
                    var config = provider.GetRequiredService<IConfiguration>();
                    var serviceClient = new TableServiceClient(config[SettingPropertyNames.AzureWebJobsStorage]);
                    return new TableStorageService<OrderEnrichedEntity>(serviceClient, config[SettingPropertyNames.AzureStorageEnrichedTableName], new Microsoft.Extensions.Logging.LoggerFactory());
                }
            );
        });

    }
}