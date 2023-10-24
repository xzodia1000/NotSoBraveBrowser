namespace NotSoBraveBrowser.models
{
    public class HistoryEntry
    {
        public string Url { get; set; }
        public DateTime Time { get; set; }

        public HistoryEntry(string url, DateTime time)
        {
            Url = url;
            Time = time;
        }
    }
}