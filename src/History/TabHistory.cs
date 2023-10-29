using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.History
{
    /**
     * HistoryNode is a class that represents a node in the history doubly linked list.
     */
    public class TabHistory
    {
        private HistoryNode? currentNode; // The current node in the history doubly linked list

        /**
         * Visit is a method that adds a history entry to the history doubly linked list.
         * It takes a string as a parameter which is the current URL.
         * It creates a new history node and adds it to the history doubly linked list.
         */
        public void Visit(string url)
        {
            var newNode = new HistoryNode(url); // Create a new history node
            if (currentNode != null)
            {
                // Doubly linked list insertion
                newNode.Prev = currentNode; // Set the previous node of the new node to the current node
                currentNode.Next = newNode; // Set the next node of the current node to the new node
            }
            currentNode = newNode; // Set the current node to the new node
        }

        /**
         * PrevUrl is a method that returns the previous URL in the history doubly linked list.
         * It sets the current node to the previous node.
         */
        public string? PrevUrl()
        {
            if (currentNode?.Prev != null)
            {
                // If the previous node is not null
                currentNode = currentNode.Prev; // Set the current node to the previous node
                return currentNode.Url; // Return the URL of the current node
            }
            return null; // Return null if the previous node is null
        }

        /**
         * NextUrl is a method that returns the next URL in the history doubly linked list.
         * It sets the current node to the next node.
         */
        public string? NextUrl()
        {
            if (currentNode?.Next != null)
            {
                // If the next node is not null
                currentNode = currentNode.Next; // Set the current node to the next node
                return currentNode.Url;// Return the URL of the current node
            }
            return null; // Return null if the next node is null
        }

        /**
         * CanGoBack is a method that returns a boolean value indicating whether the browser can go back.
         * It returns true if the previous node is not null.
         */
        public bool CanGoBack()
        {
            // Return true if the previous node is not null
            return currentNode?.Prev != null;
        }

        /**
         * CanGoForward is a method that returns a boolean value indicating whether the browser can go forward.
         * It returns true if the next node is not null.
         */
        public bool CanGoForward()
        {
            // Return true if the next node is not null
            return currentNode?.Next != null;
        }

        /**
         * GetCurrentUrl is a method that returns the URL of the current node.
         */
        public string? GetCurrentUrl()
        {
            return currentNode?.Url;
        }
    }
}