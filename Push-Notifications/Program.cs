#define ClearDB
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Push_Notifications.DataLevelLogic;

namespace Push_Notifications
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            ILogger logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Create web host.");
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    logger.LogInformation("Program.cs get context");
                    var db = services.GetRequiredService<PushContext>();
                    logger.LogInformation("Start migrations");
#if ClearDB
                    db.Database.EnsureDeleted();
#endif
                    db.Database.EnsureCreated();
#if !ClearDB
                    db.Database.Migrate();
#endif
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
