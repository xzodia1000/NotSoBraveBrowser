namespace NotSoBraveBrowser.src.HttpRequests
{
    public class Requests
    {
        private readonly HttpClient client;

        public Requests()
        {
            client = new HttpClient();
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            Uri uri = new(url);
            return await client.GetAsync(uri);
        }
    }
}
