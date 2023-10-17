namespace NotSoBraveBrowser.src.History
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

    public class TabHistory
    {
        private HistoryNode? currentNode;

        public void Visit(string url)
        {
            var newNode = new HistoryNode(url);
            if (currentNode != null)
            {
                newNode.Prev = currentNode;
                currentNode.Next = newNode;
            }
            currentNode = newNode;
        }

        public string? PrevUrl()
        {
            if (currentNode?.Prev != null)
            {
                currentNode = currentNode.Prev;
                return currentNode.Url;
            }
            return null; // or throw an exception or handle as you see fit
        }

        public string? NextUrl()
        {
            if (currentNode?.Next != null)
            {
                currentNode = currentNode.Next;
                return currentNode.Url;
            }
            return null; // or throw an exception or handle as you see fit
        }

        public string? GetCurrentUrl()
        {
            return currentNode?.Url;
        }
    }
}