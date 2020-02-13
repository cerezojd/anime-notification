using AnimeNotification.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotification.EntityFrameworkCore.Sqlite
{
    public class AnimeNotificationDbContext : DbContext
    {
        public DbSet<Anime> Animes { get; set; }

        public AnimeNotificationDbContext(DbContextOptions<AnimeNotificationDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=anime.db");

        //    base.OnConfiguring(optionsBuilder);
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Anime>(b =>
        //    {
        //        b.ToTable("Animes");
        //    });

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
 
