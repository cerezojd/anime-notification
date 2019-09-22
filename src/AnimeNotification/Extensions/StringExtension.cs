namespace AnimeNotification.Extensions
{
    public static class StringExtension
    {
        public static string ToAnimeUrl(this string url)
        {
            return url.Remove(url.LastIndexOf('-')).Replace("/ver/", "/anime/");
        }
    }
}
