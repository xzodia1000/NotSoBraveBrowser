namespace NotSoBraveBrowser.lib
{
    public static class UrlUtils
    {
        public static string NormalizeUrl(string url)
        {
            return url.Replace("https://", "").Replace("http://", "").Replace("www.", "").Replace("/", "");
        }

        public static string AddHttp(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://"))
            {
                return url;
            }
            return "http://" + url;
        }

        public static bool IsEmptyUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || url.Equals("http://") || url.Equals("https://"))
            {
                return true;
            }
            return false;
        }
    }
}