using System.Text.Json;
using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.Bookmark
{
    /**
     * BookmarkManager is a class that manages the bookmarks of the browser.
     * It stores the bookmarks in a JSON file in the local app data folder.
     * It also provides methods to add, remove and check bookmarks.
     */
    public class BookmarkManager
    {
        private readonly string filePath; // Path to the bookmarks file

        /**
         * BookmarkManager is the constructor of the BookmarkManager class.
         * It initializes the path to the bookmarks file.
         */
        public BookmarkManager()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // Get the local app data folder
            filePath = Path.Combine(path, "NotSoBraveBrowser", "bookmarks.json"); // Set the path to the bookmarks file
        }

        /**
         * GetBookmarks is a method that returns a list of BookmarkEntry objects.
         * It reads the bookmarks file and deserializes the JSON content into a list of BookmarkEntry objects.
         * If the file is corrupted, it deletes the file and returns an empty list.
         */
        public List<BookmarkEntry> GetBookmarks()
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                // Read the file and deserialize the JSON content into a list of BookmarkEntry objects
                string content = File.ReadAllText(filePath);
                List<BookmarkEntry>? bookmarkEntries = JsonSerializer.Deserialize<List<BookmarkEntry>>(content);

                if (bookmarkEntries != null)
                {
                    // Return the list of BookmarkEntry objects
                    return bookmarkEntries;
                }
                else
                {
                    // Return an empty list
                    return new List<BookmarkEntry>();
                };
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and return an empty list
                File.Delete(filePath);
                return new List<BookmarkEntry>();
            }
        }

        /**
         * AddBookmark is a method that adds a new bookmark to the bookmarks file.
         * It takes a string as a parameter, which is the URL of the bookmark.
         * It creates a new BookmarkEntry object and adds it to the bookmarks file.
         * If the file is corrupted, it deletes the file and adds the bookmark.
         */
        public void AddBookmark(string url, string name)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists
            BookmarkEntry entry = new(url, name); // Create a new BookmarkEntry object

            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();  // Read the file and deserialize the JSON content into a list of BookmarkEntry objects
                bookmarkEntries.Add(entry); // Add the new BookmarkEntry object to the list
                string jsonString = JsonSerializer.Serialize(bookmarkEntries); // Serialize the list of BookmarkEntry objects into a JSON string
                File.WriteAllText(filePath, jsonString); // Write the JSON string to the file
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and add the bookmark
                File.Delete(filePath);
                AddBookmark(url, name);
            }
        }

        /**
         * RemoveBookmark is a method that removes a bookmark from the bookmarks file.
         * It takes a string as a parameter, which is the URL of the bookmark.
         * It removes the bookmark from the bookmarks file using Linq.
         * If the file is corrupted, it deletes the file and returns.
         */
        public void RemoveBookmark(string url)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                bookmarkEntries.RemoveAll(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url))); // Remove the bookmark from the list (Linq)
                string jsonString = JsonSerializer.Serialize(bookmarkEntries);
                File.WriteAllText(filePath, jsonString);
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file
                File.Delete(filePath);
                return;
            }
        }

        public void EditBookmark(string url, string name)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                bookmarkEntries.RemoveAll(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url))); // Remove the bookmark from the list (Linq)
                BookmarkEntry entry = new(url, name); // Create a new BookmarkEntry object
                bookmarkEntries.Add(entry); // Add the new BookmarkEntry object to the list
                string jsonString = JsonSerializer.Serialize(bookmarkEntries); // Serialize the list of BookmarkEntry objects into a JSON string
                File.WriteAllText(filePath, jsonString); // Write the JSON string to the file
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and add the bookmark
                File.Delete(filePath);
                AddBookmark(url, name);
            }
        }

        public string? GetName(string url)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                return bookmarkEntries.Find(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url), StringComparison.Ordinal))?.Name; // Remove the bookmark from the list (Linq)
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and add the bookmark
                File.Delete(filePath);
                return "";
            }
        }

        /**
         * CheckBookmark is a method that checks if a bookmark exists in the bookmarks file.
         * It takes a string as a parameter, which is the URL of the bookmark.
         * It returns a boolean value, which is true if the bookmark exists and false if it doesn't using Linq.
         * If the file is corrupted, it deletes the file and returns false.
         */
        public bool CheckBookmark(string url)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                return bookmarkEntries.Any(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url))); // Check if the bookmark exists in the list (Linq)
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and return false
                File.Delete(filePath);
                return false;
            }
        }
    }
}