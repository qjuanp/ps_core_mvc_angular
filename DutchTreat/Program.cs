using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace DutchTreat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                .Build();

            await SeedDb(host);

            host.Run();
        }

        private static async Task SeedDb(IWebHost host)
        {
            // We need to create an scope because DbContext 
            // is added as an scope dependency
            // at this poing, we have to create manually the scope
            // through the scope factory. This is something that
            // asp.net core made in each request
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                await seeder.Seed();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .UseStartup<Startup>();

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            // removing default configuration options
            builder.Sources.Clear();

            // configure the source of configuration values
            // this have an order, last sources will override 
            // previous defined values
            builder
                .AddJsonFile(
                    "config.json",
                    optional: false,
                    reloadOnChange: true);
        }
    }
}
