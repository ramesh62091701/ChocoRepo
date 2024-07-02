using Extractor.Model;
using Extractor.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{
    public static class ConfigurationSetup
    {
        public static IServiceProvider ConfigureServices()
        {
            // Set up configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            // Configure services
            var services = new ServiceCollection()
                .Configure<AppSettings>(config)
                .AddSingleton<GPTService>()
                .AddSingleton<FigmaHelper>()
                .BuildServiceProvider();

            return services;
        }
    }
}
