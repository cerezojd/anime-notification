using AnimeNotification.Analyzers;
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

            if (!latestPublished.Any())
                return;

            foreach (var published in latestPublished)
            {
                // Use transaction is for testing them. You can avoid it.
                await _transactionService.Start();
                var anime = await _repository.GetByNameAsync(published.AnimeTitle);

                if (anime is null)
                    await _repository.CreateAsync(published.AnimeTitle, published.AnimeEpisode, published.AnimeLink, published.Source);
                else
                    await _repository.UpdateEposideAsync(published.AnimeTitle, published.AnimeEpisode);

                if (anime != null && published.AnimeEpisode == anime.Episode)
                {
                    _transactionService.Commit();
                    continue;
                }

                try
                {
                    await _publisher.Publish(published);
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
