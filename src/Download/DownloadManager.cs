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
        private static readonly List<string> urls = new() {
            "https://www.hw.ac.uk/",
            "https://www.google.com/",
            "https://www.duckduckgo.com/",
            "http://status.savanttools.com/?code=400%20Bad%20Request",
            "http://status.savanttools.com/?code=403%20Forbidden",
            "http://status.savanttools.com/?code=404%20Not%20Found"
        }; // List of default URLs

        private readonly Requests client; // The HTTP client
        private readonly string defaultFilePath; // Path to the default downloads file

        /**
         * DownloadManager is the constructor of the DownloadManager class.
         * It initializes the HTTP client.
         */
        public DownloadManager()
        {
            client = new Requests();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // Get the local app data folder
            defaultFilePath = Path.Combine(path, "NotSoBraveBrowser", "bulk.txt"); // Set the path to the default downloads file

            SetDefaultDownloads(); // Set the default downloads
        }

        /**
         * SetDefaultDownloads is a method that sets the default downloads.
         * It writes the URLs to the default downloads file.
         */
        private void SetDefaultDownloads()
        {
            if (FileUtils.EnsureFileExists(defaultFilePath) && File.ReadAllText(defaultFilePath).Trim().Length > 0)
            {
                // If the file exists and is not empty, do nothing
                return;
            }

            // Write the URLs to the default downloads file
            foreach (string url in urls)
            {
                File.AppendAllText(defaultFilePath, url + Environment.NewLine);
            }

        }

        /**
         * GetBulkDownloads is a method that gets the bulk downloads.
         * It takes a string as a parameter which is the path to the file that contains the URLs.
         * It returns a list of BulkDownload objects.
         */
        public async Task<List<BulkDownload>> GetBulkDownloads(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) filePath = defaultFilePath; // If the file path is empty, set it to the default downloads file

            List<BulkDownload> bulkDownloads = new(); // List of BulkDownload objects
            string? line; // A line from the file

            using (StreamReader r = new(filePath)) // Read the file
            {
                while (!string.IsNullOrEmpty(line = r.ReadLine())) // Read the file line by line until the end of the file
                {
                    try
                    {
                        line = UrlUtils.AddHttp(line); // Add http:// to the URL if it doesn't have it
                        HttpResponseMessage response = await Task.Run(() => client.Get(line)); ; // Send a GET request to the URL

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