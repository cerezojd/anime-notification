using AnimeNotification.Publisher.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
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

        public async Task Publish(string message)
        {
            var client = _clientFactory.CreateClient(nameof(TelegramPublisherService));

            var request = new Uri($"{_options.BaseUrl}/bot{_options.Token}/sendMessage?chat_id={_options.Channel}&text={message}");
            var res = await client.GetAsync(request);

            if (!res.IsSuccessStatusCode)
                throw new Exception("Cannot connect to telegram api");
        }
    }
}
