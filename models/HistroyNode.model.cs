namespace NotSoBraveBrowser.models
{
    public class HistoryNode
    {
        public string Url { get; set; }
        public HistoryNode? Next { get; set; }
        public HistoryNode? Prev { get; set; }

        public HistoryNode(string url)
        {
            Url = url;
            Next = null;
            Prev = null;
        }
    }
}