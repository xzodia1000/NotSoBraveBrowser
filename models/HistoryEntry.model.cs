namespace NotSoBraveBrowser.models
{
    /**
     * HistoryEntry is a class that stores the history entry data.
     */
    public class HistoryEntry
    {
        public string Url { get; set; } // The URL of the history entry
        public DateTime Time { get; set; } // The time when the history entry was added

        /**
         * HistoryEntry is the constructor of the HistoryEntry class.
         * It initializes the URL and the time of the history entry.
         */
        public HistoryEntry(string url, DateTime time)
        {
            Url = url;
            Time = time;
        }
    }
}