namespace NotSoBraveBrowser.models
{
    /**
     * HistoryNode is a class that stores the history node data
     * for the doubly linked list.
     */
    public class HistoryNode
    {
        public string Url { get; set; } // The URL of the history node
        public HistoryNode? Next { get; set; } // The next history node
        public HistoryNode? Prev { get; set; } // The previous history node

        /**
         * HistoryNode is the constructor of the HistoryNode class.
         * It initializes the URL of the history node.
         */
        public HistoryNode(string url)
        {
            Url = url;
            Next = null;
            Prev = null;
        }
    }
}