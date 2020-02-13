using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

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
                    AnimeEpisode = int.Parse(HttpUtility.HtmlDecode(episodeNode.InnerText.Replace("Episodio ", ""))),
                    AnimeTitle = HttpUtility.HtmlDecode(titleNode.InnerText),
                    Source = HttpUtility.HtmlDecode(Url),
                    AnimeLink = HttpUtility.HtmlDecode(string.Concat(Url.Remove(Url.Length - 1), link))
                });
            }

            return Task.FromResult(result.ToArray());
        }

        public async Task<AnimeInfoResult> GetAnimeInfoAsync(string animeUrl)
        {
            var htmlAnimeWeb = new HtmlWeb();
            var htmlAnimeDocument = await htmlAnimeWeb.LoadFromWebAsync(animeUrl);

            var genreNodes = htmlAnimeDocument.DocumentNode.SelectNodes(".//a[contains(@href, 'genre')]");

            var genres = new List<string>();
            if (!(genreNodes is null))
            {
                foreach (var genreNode in genreNodes)
                {
                    genres.Add(HttpUtility.HtmlDecode(genreNode.InnerText));
                }
            }

            var descriptionNode = htmlAnimeDocument.DocumentNode.SelectSingleNode(".//div[@class = 'Description']/p");

            return new AnimeInfoResult
            {
                Description = HttpUtility.HtmlDecode(descriptionNode.InnerText),
                Genres = genres.ToArray()
            };
        }
    }
}
