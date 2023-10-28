using System.Text.Json;
using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.Bookmark
{
    public class BookmarkManager
    {
        private readonly string filePath;

        public BookmarkManager()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(path, "NotSoBraveBrowser", "bookmarks.json");
        }

        public List<BookmarkEntry> GetBookmarks()
        {
            try
            {
                string content = File.ReadAllText(filePath);
                List<BookmarkEntry>? bookmarkEntries = JsonSerializer.Deserialize<List<BookmarkEntry>>(content);

                if (bookmarkEntries != null)
                {
                    return bookmarkEntries;
                }
                else
                {
                    return new List<BookmarkEntry>();
                };
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                return new List<BookmarkEntry>();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return new List<BookmarkEntry>();
            }
            catch (JsonException)
            {
                return new List<BookmarkEntry>();
            }
        }

        public void AddBookmark(string url)
        {
            BookmarkEntry entry = new(url, DateTime.Now);
            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                bookmarkEntries.Add(entry);
                string jsonString = JsonSerializer.Serialize(bookmarkEntries);
                File.WriteAllText(filePath, jsonString);
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                AddBookmark(url);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                AddBookmark(url);
            }
            catch (JsonException)
            {
                File.Delete(filePath);
                AddBookmark(url);
            }
        }

        public void RemoveBookmark(string url)
        {
            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                bookmarkEntries.RemoveAll(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url)));
                string jsonString = JsonSerializer.Serialize(bookmarkEntries);
                File.WriteAllText(filePath, jsonString);
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return;
            }
            catch (JsonException)
            {
                File.Delete(filePath);
                return;
            }
        }

        public bool CheckBookmark(string url)
        {
            try
            {
                List<BookmarkEntry> bookmarkEntries = GetBookmarks();
                return bookmarkEntries.Any(entry => UrlUtils.NormalizeUrl(entry.Url).Equals(UrlUtils.NormalizeUrl(url)));
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return false;
            }
            catch (JsonException)
            {
                File.Delete(filePath);
                return false;
            }
        }
    }
}