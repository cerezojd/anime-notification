using System;
using System.Threading.Tasks;
using AnimeNotification.Entities;
using AnimeNotification.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnimeNotification.Repositories
{
    public class AnimeRepository : IAnimeRepository
    {

        private readonly AnimeNotificationDbContext _context;
        private readonly DbSet<Anime> _dbSet;

        public AnimeRepository(AnimeNotificationDbContext context)
        {
            _dbSet = context.Animes;
            _context = context;
        }

        public async Task<Anime> CreateAsync(string title, int eposide, string link, string source)
        {
            var newAnime = new Anime
            {
                Title = title,
                Episode = eposide,
                Link = link,
                Source = source
            };

            await _dbSet.AddAsync(newAnime);
            await _context.SaveChangesAsync();

            return newAnime;
        }

        public async Task<Anime> GetByNameAsync(string title)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Title == title);
        }

        public async Task<Anime> UpdateEposideAsync(string title, int episode)
        {
            var anime = await GetByNameAsync(title);
            if (anime is null)
                throw new Exception("Anime not found");

            anime.Episode = episode;
            await _context.SaveChangesAsync();

            return anime;
        }
    }
}
