namespace AnimeNotification.Analyzers
{
    public class AnalyzeResult
    {
        public string AnimeTitle { get; set; }
        public int AnimeEpisode { get; set; }
        public string AnimeLink { get; set; }
        public string Source { get; set; }


        public string GetAnimeProfileUrl()
        {
            return AnimeLink.Remove(AnimeLink.LastIndexOf('-')).Replace("/ver/", "/anime/");
        }
    }
}
