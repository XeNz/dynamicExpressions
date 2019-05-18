using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExpressFuncStuff.DataAccess;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpressFuncStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            SeedData(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                            optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseStartup<Startup>();

        private static void SeedData(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<EntityContext>();
                    DbInitializer.Seed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }

    public static class DbInitializer
    {
        public static void Seed(EntityContext context)
        {
            context.Database.EnsureCreated();

            var testBlog = context.Comments.FirstOrDefault();
            if (testBlog == null)
            {
                context.Comments.AddRange(
                    new List<Comment>
                    {
                        new Comment {Id = 1, Text = "bla", PlacedAt = DateTime.Now.Subtract(new TimeSpan(1, 0, 0))},
                        new Comment
                        {
                            Id = 2, Text = "bla2", PlacedAt = DateTime.Now.Subtract(new TimeSpan(2, 0, 0))
                        },
                        new Comment {Id = 3, Text = "ble", PlacedAt = DateTime.Now.Subtract(new TimeSpan(2, 0, 0))},
                        new Comment {Id = 4, Text = "ble", PlacedAt = DateTime.Now.Subtract(new TimeSpan(4, 0, 0))},
                        new Comment
                        {
                            Id = 5, Text = "bled", PlacedAt = DateTime.Now.Subtract(new TimeSpan(4, 0, 0))
                        },
                        new Comment {Id = 6, Text = "bled", PlacedAt = DateTime.Now.Subtract(new TimeSpan(4, 0, 0))}
                    });

                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}