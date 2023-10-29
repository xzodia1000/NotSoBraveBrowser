namespace NotSoBraveBrowser.models
{
    /**
     * BulkDownload is a class that stores the bulk download data.
     */
    public class BulkDownload
    {
        public string Code { get; set; } // The status code of the response
        public float Bytes { get; set; } // The size of the file in bytes
        public string Url { get; set; } // The URL of the file


        /**
         * BulkDownload is the constructor of the BulkDownload class.
         * It initializes the status code, the size of the file and the URL of the file.
         */
        public BulkDownload(string code, float bytes, string url)
        {
            Code = code;
            Bytes = bytes;
            Url = url;
        }
    }
}