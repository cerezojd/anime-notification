using System.Threading.Tasks;

namespace AnimeNotification.Analyzers
{
    public interface IAnalyzeService
    {
        Task<AnalyzeResult[]> GetLastestPublished();
        Task<AnimeInfoResult> GetAnimeInfoAsync(string animeUrl);
    }
}
