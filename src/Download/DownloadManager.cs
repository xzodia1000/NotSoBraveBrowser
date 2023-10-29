using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.HttpRequests;

namespace NotSoBraveBrowser.src.Download
{
    /**
     * DownloadManager is a class that manages the bulk downloads of the browser.
     */
    public class DownloadManager
    {
        private readonly Requests client; // The HTTP client

        /**
         * DownloadManager is the constructor of the DownloadManager class.
         * It initializes the HTTP client.
         */
        public DownloadManager()
        {
            client = new Requests();
        }

        /**
         * GetBulkDownloads is a method that gets the bulk downloads.
         * It takes a string as a parameter which is the path to the file that contains the URLs.
         * It returns a list of BulkDownload objects.
         */
        public async Task<List<BulkDownload>> GetBulkDownloads(string filePath)
        {
            List<BulkDownload> bulkDownloads = new(); // List of BulkDownload objects
            string? line; // A line from the file

            using (StreamReader r = new(filePath)) // Read the file
            {
                while (!string.IsNullOrEmpty(line = r.ReadLine())) // Read the file line by line until the end of the file
                {
                    try
                    {
                        line = UrlUtils.AddHttp(line); // Add http:// to the URL if it doesn't have it
                        HttpResponseMessage response = await Task.Run(() => client.Get(line));; // Send a GET request to the URL

                        if (response.IsSuccessStatusCode)
                        {
                            // If the request is successful, add a new BulkDownload object to the list
                            byte[] content = await response.Content.ReadAsByteArrayAsync(); // Read the content of the response as a byte array
                            BulkDownload bulkDownload = new(response.StatusCode.ToString(), content.Length, line); // Create a new BulkDownload object
                            bulkDownloads.Add(bulkDownload);
                        }
                        else
                        {
                            // If the request is not successful, add a new BulkDownload object to the list with the error status code
                            BulkDownload bulkDownload = new(response.StatusCode.ToString(), 0, line);
                            bulkDownloads.Add(bulkDownload);
                        }
                    }
                    catch (UriFormatException)
                    {
                        // If the URL is invalid, add a new BulkDownload object to the list with the error message
                        BulkDownload bulkDownload = new("Invalid URL", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }
                    catch (TaskCanceledException)
                    {
                        // If the request times out, add a new BulkDownload object to the list with the timeout message
                        BulkDownload bulkDownload = new("Timeout", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }
                    catch (HttpRequestException)
                    {
                        // If the request fails, add a new BulkDownload object to the list with the error message
                        BulkDownload bulkDownload = new("Error", 0, line);
                        bulkDownloads.Add(bulkDownload);
                    }

                }
            }

            return bulkDownloads; // Return the list of BulkDownload objects
        }
    }
}