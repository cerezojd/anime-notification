using AnimeNotification.Analyzers;
using AnimeNotification.Entities;
using AnimeNotification.Publisher.Abstractions;
using AnimeNotification.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeNotification.Executor
{
    public class ExecutorService
    {
        private readonly IAnimeRepository _repository;
        private readonly IAnalyzeService _analyzer;
        private readonly IPublisherService _publisher;

        public ExecutorService(IAnimeRepository repository, IAnalyzeService analyzer, IPublisherService publisher)
        {
            _repository = repository;
            _analyzer = analyzer;
            _publisher = publisher;
        }

        public async Task StartExecutor()
        {

            var latestPublished = await _analyzer.GetLastestPublished();

            if (!latestPublished.Any())
                return;

            foreach (var published in latestPublished)
            {
                var anime = await _repository.GetByNameAsync(published.AnimeTitle);
                var isNewAnime = false;

                if (anime is null)
                {
                    isNewAnime = true;
                    anime = await _repository.CreateAsync(published.AnimeTitle, published.AnimeEpisode, published.AnimeLink, published.Source);
                }
                else
                {
                    await _repository.UpdateEposideAsync(published.AnimeTitle, published.AnimeEpisode);
                }


                if (isNewAnime == false && published.AnimeEpisode == anime.Episode)
                    continue;


                await _publisher.Publish($"Capítulo {published.AnimeEpisode} de {published.AnimeTitle} disponible.");
            }
        }
    }
}
