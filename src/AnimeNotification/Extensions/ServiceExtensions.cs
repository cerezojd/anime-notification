using AnimeNotification.Analyzers;
using AnimeNotification.Executor;
using AnimeNotification.Publisher;
using AnimeNotification.Publisher.Abstractions;
using AnimeNotification.Publisher.Telegram;
using AnimeNotification.Repositories;
using AnimeNotification.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeNotification.Services
{
    public static class ServiceExtensions
    {

        public static void ConfigureApplicationServices(this ServiceCollection services)
        {
            services.AddScoped<IAnalyzeService, AnimeFlvAnalyzerService>();
            services.AddScoped<IAnimeRepository, AnimeRepository>();
            services.AddScoped<IPublisherService, TelegramPublisherService>();
            services.AddScoped<ExecutorService>();
            services.AddTransient<TransactionService>();
            services.AddScoped<AnimePublisherService>();
        }
    }
}
