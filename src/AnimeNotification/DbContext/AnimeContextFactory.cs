using AnimeNotification.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AnimeNotification.Sqlite
{
    public class AnimeContextFactory : IDesignTimeDbContextFactory<AnimeNotificationDbContext>
    {
        public AnimeNotificationDbContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AnimeNotificationDbContext>();
            optionsBuilder.UseSqlite(configuration["ConnectionString"]);

            return new AnimeNotificationDbContext(optionsBuilder.Options);
        }
    }
}
