using System.Text.Json;
using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.History
{
    /**
     * GlobalHistory is a class that manages the global history of the browser.
     */
    public class GlobalHistory
    {
        private readonly string filePath; // The path to the history file

        /**
         * GlobalHistory is the constructor of the GlobalHistory class.
         * It initializes the path to the history file.
         */
        public GlobalHistory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // Get the local app data folder
            filePath = Path.Combine(path, "NotSoBraveBrowser", "history.json"); // Set the path to the history file
        }

        /**
         * GetHistory is a method that returns a list of HistoryEntry objects.
         * It reads the history file and deserializes the JSON content into a list of HistoryEntry objects.
         * If the file is corrupted, it deletes the file and returns an empty list.
         */
        public List<HistoryEntry> GetHistory()
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            try
            {
                string content = File.ReadAllText(filePath); // Read the file

                // Deserialize the JSON content into a list of HistoryEntry objects
                List<HistoryEntry>? historyEntries = JsonSerializer.Deserialize<List<HistoryEntry>>(content);

                if (historyEntries != null)
                {
                    // Return the list of HistoryEntry objects
                    return historyEntries;
                }
                else
                {
                    // Return an empty list
                    return new List<HistoryEntry>();
                };

            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and return an empty list
                return new List<HistoryEntry>();
            }
        }

        /**
         * AddHistory is a method that adds a history entry to the history file.
         * It takes a string as a parameter which is the URL of the history entry.
         * It creates a HistoryEntry object and adds it to the history file.
         */
        public void AddHistory(string url)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            // Create a new HistoryEntry object with the URL and the current date and time
            HistoryEntry entry = new(url, DateTime.Now);

            try
            {
                // Read the file and deserialize the JSON content into a list of HistoryEntry objects
                List<HistoryEntry> historyEntries = GetHistory();
                historyEntries.Add(entry);
                string jsonString = JsonSerializer.Serialize(historyEntries);
                File.WriteAllText(filePath, jsonString); // Write the JSON string to the file
            }
            catch (JsonException)
            {
                // If the file is corrupted, delete the file and add the history entry
                File.Delete(filePath);
                AddHistory(url);
            }
        }
    }
}