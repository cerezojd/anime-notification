using AnimeNotification.Analyzers;
using System.Threading.Tasks;

namespace AnimeNotification.Publisher.Abstractions
{
    public interface IPublisherService
    {
        Task Publish(AnalyzeResult anime);
    }
}
