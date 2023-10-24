using System.Text.Json;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.History
{
    public class GlobalHistory
    {
        private readonly string filePath;

        public GlobalHistory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(path, "NotSoBraveBrowser", "history.json");
        }

        public List<HistoryEntry> GetHistory()
        {
            try
            {
                // Attempt to read the file or perform any other operations.
                string content = File.ReadAllText(filePath);
                List<HistoryEntry>? historyEntries = JsonSerializer.Deserialize<List<HistoryEntry>>(content);

                if (historyEntries != null)
                {
                    return historyEntries;
                }
                else
                {
                    return new List<HistoryEntry>();
                };

            }
            catch (FileNotFoundException) // Specific exception for a file that doesn't exist.
            {
                // File does not exist, create it.
                File.Create(filePath).Close();
                return new List<HistoryEntry>();
            }
            catch (DirectoryNotFoundException) // Specific exception for a directory that doesn't exist.
            {
                // Directory does not exist, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return new List<HistoryEntry>();
            }
            catch (JsonException)
            {
                return new List<HistoryEntry>();
            }
        }

        public void AddEntry(string url)
        {
            HistoryEntry entry = new(url, DateTime.Now);
            try
            {
                List<HistoryEntry> historyEntries = GetHistory();
                historyEntries.Add(entry);
                string jsonString = JsonSerializer.Serialize(historyEntries);
                File.WriteAllText(filePath, jsonString);
            }
            catch (DirectoryNotFoundException) // Specific exception for a directory that doesn't exist.
            {
                // Directory does not exist, create it.
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                AddEntry(url);
            }
        }
    }
}