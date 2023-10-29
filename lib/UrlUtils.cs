namespace NotSoBraveBrowser.lib
{
    /**
     * UrlUtils is a class that provides methods to work with URLs.
     */
    public static class UrlUtils
    {
        /**
         * NormalizeUrl is a method that normalizes the given URL.
         * It removes the protocol, the subdomain and the trailing slash.
         * It returns the normalized URL.
         */
        public static string NormalizeUrl(string url)
        {
            return url.Replace("https://", "").Replace("http://", "").Replace("www.", "").Replace("/", "");
        }

        /**
         * AddHttp is a method that adds the HTTP protocol to the given URL.
         * It returns the URL with the HTTP protocol.
         */
        public static string AddHttp(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://"))
            {
                return url;
            }
            return "http://" + url;
        }

        /**
         * IsEmptyUrl is a method that checks if the given URL is empty.
         * It returns true if the URL is empty, false otherwise.
         */
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