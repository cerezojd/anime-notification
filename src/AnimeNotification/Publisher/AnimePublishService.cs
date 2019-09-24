using AnimeNotification.Analyzers;
using AnimeNotification.Publisher.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnimeNotification.Publisher
{
    public class AnimePublisherService
    {
        private readonly IPublisherService _publishService;

        public AnimePublisherService(IPublisherService publishService)
        {
            _publishService = publishService;
        }

        public async Task PublishEpisodeAsync(AnalyzeResult anime)
        {
            var message = new StringBuilder($"*{anime.AnimeTitle}* episodio *{anime.AnimeEpisode}* disponible.");

            if (!string.IsNullOrWhiteSpace(anime.AnimeLink))
                message.Append($" [Ver ahora]({anime.AnimeLink}");

            await _publishService.PublishAsync(message.ToString());
        }

        public async Task PublishNewAsync(AnalyzeResult anime, AnimeInfoResult animeInfo)
        {
            var message = new StringBuilder($"*{anime.AnimeTitle}* ha comenzado a emitirse.\n\n");

            if (!string.IsNullOrWhiteSpace(animeInfo.Description))
            {
                if(animeInfo.Description.Length > 120)
                    animeInfo.Description = string.Concat(animeInfo.Description.Remove(120), $"... [Leer más]({anime.GetAnimeProfileUrl()})");

                message.Append($"{animeInfo.Description} \n\n");
            }


            if (!string.IsNullOrWhiteSpace(anime.AnimeLink))
                message.Append($"[Ver capítulo]({anime.AnimeLink}) \n\n");

            if (!(animeInfo.Genres is null) && animeInfo.Genres.Any())
            {
                var genres = string.Join(' ', animeInfo.Genres.Select(g => $"#{g.Replace(" ", "")}")).ToString();
                message.Append(genres);
            }

            await _publishService.PublishAsync(message.ToString());
        }
    }
}
