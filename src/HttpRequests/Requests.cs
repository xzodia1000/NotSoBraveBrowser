namespace NotSoBraveBrowser.src.HttpRequests
{
    /**
     * Requests is a class that provides methods to make HTTP requests.
     */
    public class Requests
    {
        private readonly HttpClient client; // The HTTP client

        /**
         * Requests is the constructor of the Requests class.
         * It initializes the HTTP client.
         */
        public Requests()
        {
            client = new HttpClient();
        }

        /**
         * Get is a method that makes a GET request to the given URL.
         * It returns the response of the request.
         */
        public async Task<HttpResponseMessage> Get(string url)
        {
            Uri uri = new(url); // Create a new URI object from the URL
            return await client.GetAsync(uri); // Make a GET request to the URI
        }
    }
}
