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
                var titleNode = animeNode.SelectSingleNode(".//a/strong[@class='Title']");
                var episodeNode = animeNode.SelectSingleNode(".//a/span[@class='Capi']");

                if (titleNode is null || episodeNode is null)
                    continue;

                var link = animeNode.SelectSingleNode(".//a").GetAttributeValue("href", null);

                result.Add(new AnalyzeResult {
                    AnimeEpisode = int.Parse(episodeNode.InnerText.Replace("Episodio ", "")),
                    AnimeTitle = titleNode.InnerText,
                    Source = Url,
                    AnimeLink = string.Concat(Url.Remove(Url.Length - 1), link)
                });
            }

            return Task.FromResult(result.ToArray());
        }
    }
}
