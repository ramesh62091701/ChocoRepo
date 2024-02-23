﻿using Azure.Data.Tables;
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
        builder.Services.AddSingleton<IOrderService, OrderService>();

        builder.Services.AddTransient(provider =>
        {

            return new Func<ITableStorageService<OrderEntity>>(
                () =>
                {
                    var config = provider.GetRequiredService<IConfiguration>();
                    var serviceClient = new TableServiceClient(config[SettingPropertyNames.AzureWebJobsStorage]);
                    return new TableStorageService<OrderEntity>(serviceClient, "order", new Microsoft.Extensions.Logging.LoggerFactory());

                }
            );
        });
    }
}