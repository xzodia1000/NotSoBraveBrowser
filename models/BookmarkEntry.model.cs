namespace NotSoBraveBrowser.models
{
    /**
     * BookmarkEntry is a class that stores the bookmark entry data.
     */
    public class BookmarkEntry
    {
        public string Url { get; set; } // The URL of the bookmark
        public string Name { get; set; } // The name of the bookmark

        /**
         * BookmarkEntry is the constructor of the BookmarkEntry class.
         * It initializes the URL and the time of the bookmark.
         */
        public BookmarkEntry(string url, string name)
        {
            Url = url;
            Name = name;
        }
    }
}