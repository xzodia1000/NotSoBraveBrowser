namespace NotSoBraveBrowser.lib
{
    public static class UrlUtils
    {
        public static string NormalizeUrl(string url)
        {
            return url.Replace("https://", "").Replace("http://", "").Replace("www.", "").Replace("/", "");
        }
    }
}