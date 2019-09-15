using AnimeNotification.Entities;
using System.Threading.Tasks;

namespace AnimeNotification.Repositories
{
    public interface IAnimeRepository
    {
        Task<Anime> CreateAsync(string title, int eposide, string link, string source);
        Task<Anime> GetByNameAsync(string title);
        Task<Anime> UpdateEposideAsync(string title, int episode);
    }
}
