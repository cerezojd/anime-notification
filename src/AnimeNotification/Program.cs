using AnimeNotification.Analyzers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace AnimeNotification
{
    public class Program
    {
        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                /*.AddJsonFile("appsettings.json", optional: false)*/;

            return builder.Build();
        }

        private static IServiceProvider BuildDi(IConfiguration configurationRoot)
        {
            var services = new ServiceCollection();

            services
                .AddSingleton(configurationRoot)
                .AddOptions();

            services.AddScoped<IAnalyzeService, AnimeFlvAnalyzerService>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }


        static void Main(string[] args)
        {
            var config = LoadConfiguration();
            var services = BuildDi(config);

        }
    }
}
