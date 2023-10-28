using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.HttpRequests;

namespace NotSoBraveBrowser.src.Download
{
    public class DownloadManager
    {
        private readonly Requests client;

        public DownloadManager()
        {
            client = new Requests();
        }

        public async Task<List<BulkDownload>> GetBulkDownloads(string filePath)
        {
            List<BulkDownload> bulkDownloads = new();
            string? line;

            using (StreamReader r = new(filePath))
            {
                while ((line = r.ReadLine()) != null && line != "")
                {
                    try
                    {
                        if (!line.StartsWith("http://") && !line.StartsWith("https://"))
                        {
                            line = "http://" + line;
                        }
                        HttpResponseMessage response = await client.Get(line);

                        if (response.IsSuccessStatusCode)
                        {
                            byte[] content = await response.Content.ReadAsByteArrayAsync();
                            BulkDownload bulkDownload = new(response.StatusCode.ToString(), content.Length, line);
                            bulkDownloads.Add(bulkDownload);
                        }
                        else
                        {
                            BulkDownload bulkDownload = new(response.StatusCode.ToString(), 0, line);
                            bulkDownloads.Add(bulkDownload);
                        }
                    }
                    catch (UriFormatException)
                    {
                        BulkDownload bulkDownload = new("Invalid URL", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }
                    catch (TaskCanceledException)
                    {
                        BulkDownload bulkDownload = new("Timeout", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }
                    catch (HttpRequestException)
                    {
                        BulkDownload bulkDownload = new("Error", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }

                }
            }

            return bulkDownloads;
        }
    }
}