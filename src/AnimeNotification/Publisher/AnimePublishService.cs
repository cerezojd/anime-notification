using AnimeNotification.Analyzers;
using AnimeNotification.Publisher.Abstractions;
using System.Text;
using System.Threading.Tasks;

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
            var message = new StringBuilder($"Episodio *{anime.AnimeEpisode}* de *{anime.AnimeTitle}* disponible.");

            if (!string.IsNullOrWhiteSpace(anime.AnimeLink))
                message.Append($" [Ver ahora]({anime.AnimeLink}");

            await _publishService.PublishAsync(message.ToString());
        }

        public async Task PublishNewAsync(AnalyzeResult anime)
        {
            var message = new StringBuilder($"Episodio *{anime.AnimeEpisode}* de *{anime.AnimeTitle}* disponible.");

            if (!string.IsNullOrWhiteSpace(anime.AnimeLink))
                message.Append($" [Ver ahora]({anime.AnimeLink}");

            await _publishService.PublishAsync(message.ToString());
        }
    }
}
