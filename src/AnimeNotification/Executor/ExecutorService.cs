using AnimeNotification.Analyzers;
using AnimeNotification.Entities;
using AnimeNotification.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeNotification.Executor
{
    public class ExecutorService
    {
        private readonly IAnimeRepository _repository;
        private readonly IAnalyzeService _analyzer;

        public ExecutorService(IAnimeRepository repository, IAnalyzeService analyzer)
        {
            _repository = repository;
            _analyzer = analyzer;
        }

        public async Task StartExecutor()
        {
            var latestPublished = await _analyzer.GetLastestPublished();

            if (!latestPublished.Any())
                return;

            foreach (var published in latestPublished)
            {
                var anime = await _repository.GetByNameAsync(published.AnimeTitle);

                if (anime is null)
                {
                    anime = await _repository.CreateAsync(published.AnimeTitle, published.AnimeEpisode, published.AnimeLink, published.Source);
                }
                else
                {
                    anime = await _repository.UpdateEposideAsync(published.AnimeTitle, published.AnimeEpisode);
                }

                // publish
                    
            }
        }
    }
}
