using AnimeNotification.Analyzers;
using AnimeNotification.Entities;
using AnimeNotification.Publisher.Abstractions;
using AnimeNotification.Repositories;
using AnimeNotification.Sqlite;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeNotification.Executor
{
    public class ExecutorService
    {
        private readonly IAnimeRepository _repository;
        private readonly IAnalyzeService _analyzer;
        private readonly IPublisherService _publisher;
        private readonly TransactionService _transactionService;

        public ExecutorService(IAnimeRepository repository, IAnalyzeService analyzer, IPublisherService publisher, TransactionService transactionService)
        {
            _repository = repository;
            _analyzer = analyzer;
            _publisher = publisher;
            _transactionService = transactionService;
        }

        public async Task StartExecutor()
        {
            var latestPublished = await _analyzer.GetLastestPublished();

            await _transactionService.Start();

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

                try
                {
                    await _publisher.Publish($"Capítulo {published.AnimeEpisode} de {published.AnimeTitle} disponible.");
                }
                catch
                {
                    _transactionService.Rollback();
                    throw;
                }
            }

            _transactionService.Commit();
        }
    }
}
