using System.Threading.Tasks;

namespace AnimeNotification.Publisher.Abstractions
{
    public interface IPublisherService
    {
        Task PublishAsync(string content);
    }
}
