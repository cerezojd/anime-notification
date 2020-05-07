using AnimeNotification.EntityFrameworkCore.Sqlite;
using AnimeNotification.Executor;
using AnimeNotification.Publisher.Telegram;
using AnimeNotification.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeNotification
{
    public class Program
    {
        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }

        private static IServiceProvider BuildDi(IConfiguration configurationRoot)
        {
            var services = new ServiceCollection();

            services
                .AddSingleton(configurationRoot)
                .AddOptions()
                .Configure<TelegramPublisherConfiguration>(configurationRoot.GetSection("TelegramApi"));

            services.AddDbContext<AnimeNotificationDbContext>(opt =>
            {
                opt.UseSqlite(configurationRoot["ConnectionString"]);
            });

            services.AddHttpClient();
            services.ConfigureApplicationServices();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\anlogs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public async static Task Main(string[] args)
        {
            var config = LoadConfiguration();
            var services = BuildDi(config);
            ConfigureSerilog();

            var executor = services.GetService<ExecutorService>();

            Log.Information("Application started");
            while (true)
            {
                try
                {
                    Log.Information("Start new analysis");
                    await executor.StartExecutor();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }

                Log.Information("Waiting to start a new analysis...");
                Thread.Sleep(600000);
            }


        }
    }
}
