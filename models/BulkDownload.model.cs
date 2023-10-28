namespace NotSoBraveBrowser.models
{
    public class BulkDownload
    {
        public string Code { get; set; }
        public float Bytes { get; set; }
        public string Url { get; set; }

        public BulkDownload(string code, float bytes, string url)
        {
            Code = code;
            Bytes = bytes;
            Url = url;
        }
    }
}