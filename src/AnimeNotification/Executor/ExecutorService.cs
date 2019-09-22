using AnimeNotification.Analyzers;
using AnimeNotification.Extensions;
using AnimeNotification.Publisher;
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
        private readonly AnimePublisherService _publisher;
        private readonly TransactionService _transactionService;

        public ExecutorService(IAnimeRepository repository, IAnalyzeService analyzer, AnimePublisherService publisher, TransactionService transactionService)
        {
            _repository = repository;
            _analyzer = analyzer;
            _publisher = publisher;
            _transactionService = transactionService;
        }

        public async Task StartExecutor()
        {
            var latestPublished = await _analyzer.GetLastestPublished();

            if (!latestPublished.Any())
                return;

            foreach (var published in latestPublished)
            {
                var anime = await _repository.GetByNameAsync(published.AnimeTitle);

                if (anime != null && published.AnimeEpisode == anime.Episode)
                    continue;

                // Use transaction is for testing them. You can avoid it.
                await _transactionService.Start();

                if (anime is null)
                    await _repository.CreateAsync(published.AnimeTitle, published.AnimeEpisode, published.AnimeLink, published.Source);
                else
                    await _repository.UpdateEposideAsync(published.AnimeTitle, published.AnimeEpisode);

                try
                {
                    if (published.AnimeEpisode > 1)
                    {
                        await _publisher.PublishEpisodeAsync(published);
                    }
                    else
                    {
                        var animeInfo = await _analyzer.GetAnimeInfoAsync(published.AnimeLink.ToAnimeUrl());
                        await _publisher.PublishNewAsync(published, animeInfo);
                    }
                }
                catch
                {
                    _transactionService.Rollback();
                    throw;
                }

                _transactionService.Commit();
            }
        }
    }
}
