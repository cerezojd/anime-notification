using AnimeNotification.Analyzers;
using AnimeNotification.EntityFrameworkCore.Sqlite;
using AnimeNotification.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
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
                .AddOptions();

            services.AddDbContext<AnimeNotificationDbContext>(opt =>
            {
                opt.UseSqlite(configurationRoot["ConnectionString"]);
            });

            services
                .AddSingleton(configurationRoot)
                .AddOptions();

            services.AddScoped<IAnalyzeService, AnimeFlvAnalyzerService>();
            //services.AddScoped<AnimeNotificationDbContext>();
            services.AddScoped<AnimeService>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public async static Task Main(string[] args)
        {
            var config = LoadConfiguration();
            var services = BuildDi(config);

            var animeService = services.GetService<AnimeService>();

            await animeService.CreateAsync();
        }
    }
}
