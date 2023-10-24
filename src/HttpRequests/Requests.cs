namespace NotSoBraveBrowser.src.HttpRequests
{
    public class Requests
    {
        private readonly HttpClient client;

        public Requests()
        {
            client = new HttpClient();
        }

        public async Task<string> Get(string url)
        {

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
