using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using birds.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace birds
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Display the name of the current thread culture.
            Console.WriteLine("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name);

            // Change the current culture to en-US.
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            Console.WriteLine("CurrentCulture is now {0}.", CultureInfo.CurrentCulture.Name);

            // Display the name of the current UI culture.
            Console.WriteLine("CurrentUICulture is {0}.", CultureInfo.CurrentUICulture.Name);

            // Change the current UI culture to en-US.
            CultureInfo.CurrentUICulture = new CultureInfo("en-US", false);
            Console.WriteLine("CurrentUICulture is now {0}.", CultureInfo.CurrentUICulture.Name);
        
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
