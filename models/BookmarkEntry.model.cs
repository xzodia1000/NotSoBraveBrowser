namespace NotSoBraveBrowser.models
{
    public class BookmarkEntry
    {
        public string Url { get; set; }
        public DateTime Time { get; set; }

        public BookmarkEntry(string url, DateTime time)
        {
            Url = url;
            Time = time;
        }
    }
}