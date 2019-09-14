using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimeNotification.Analyzers
{
    public class AnimeFlvAnalyzerService : IAnalyzeService
    {
        private const string Url = "https://animeflv.net/";
        private readonly HtmlWeb _web = new HtmlWeb();
        private readonly HtmlDocument _doc;

        public AnimeFlvAnalyzerService()
        {
            _doc = _web.Load(Url);
        }

        public Task<AnalyzeResult[]> GetLastestPublished()
        {
            var result = new List<AnalyzeResult>();
            var episodeListNode = _doc.DocumentNode.SelectNodes(".//ul[contains(@class, 'ListEpisodios')]/li");

            foreach (var animeNode in episodeListNode)
            {
                var animeTitleNode = animeNode.SelectSingleNode(".//a/strong[@class='Title']");
                var animeEpisodeNode = animeNode.SelectSingleNode(".//a/span[@class='Capi']");

                if (animeTitleNode is null || animeEpisodeNode is null)
                    continue;

                result.Add(new AnalyzeResult {
                    //  TODO: Improve parse anime data
                    AnimeEpisode = int.Parse(animeEpisodeNode.InnerText.Replace("Episodio ", "")),
                    AnimeTitle = animeTitleNode.InnerText,
                    Source = Url,
                    // TODO: Get anime url
                    AnimeLink = Url
                });
            }

            return Task.FromResult(result.ToArray());
        }
    }
}
