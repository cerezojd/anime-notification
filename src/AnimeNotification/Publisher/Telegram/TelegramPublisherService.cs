using AnimeNotification.Analyzers;
using AnimeNotification.Publisher.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnimeNotification.Publisher.Telegram
{
    public class TelegramPublisherService: IPublisherService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TelegramPublisherConfiguration _options;

        public TelegramPublisherService(IHttpClientFactory clientFactory, IOptions<TelegramPublisherConfiguration> options)
        {
            _clientFactory = clientFactory;
            _options = options.Value;
        }

        public async Task Publish(AnalyzeResult anime)
        {
            var message = new StringBuilder($"Episodio *{anime.AnimeEpisode}* de *{anime.AnimeTitle}* disponible.");

            if (!string.IsNullOrWhiteSpace(anime.AnimeLink))
                message.Append($" [Ver ahora]({anime.AnimeLink}");

            var client = _clientFactory.CreateClient(nameof(TelegramPublisherService));

            var request = new Uri($"{_options.BaseUrl}/bot{_options.Token}/sendMessage?chat_id={_options.Channel}&parse_mode=Markdown&text={message}");
            var res = await client.GetAsync(request);

            if (!res.IsSuccessStatusCode)
                throw new Exception("Cannot connect to telegram api");
        }
    }
}
