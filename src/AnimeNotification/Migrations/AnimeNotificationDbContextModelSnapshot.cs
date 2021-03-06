﻿// <auto-generated />
using AnimeNotification.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnimeNotification.Migrations
{
    [DbContext(typeof(AnimeNotificationDbContext))]
    partial class AnimeNotificationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("AnimeNotification.Entities.Anime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Episode");

                    b.Property<string>("Link");

                    b.Property<string>("Source");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Animes");
                });
#pragma warning restore 612, 618
        }
    }
}
