using NotSoBraveBrowser.lib;

namespace NotSoBraveBrowser.src.Home
{
    /**
     * HomeManager is a class that manages the home page of the browser.
     * It stores the home page in a text file in the local app data folder.
     * It also provides methods to get and set the home page.
     */
    public class HomeManager
    {
        private readonly string filePath; // Path to the home page file

        /**
         * HomeManager is the constructor of the HomeManager class.
         * It initializes the path to the home page file.
         */
        public HomeManager()
        {
            // Get the local app data folder
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(path, "NotSoBraveBrowser", "home.txt"); // Set the path to the home page file
        }


        /**
         * GetHome is a method that returns the home page.
         * It reads the home page file and returns the content of the file.
         * If the file is empty, it returns the default home page.
         */
        public string GetHome()
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists

            string content = File.ReadAllText(filePath);
            if (UrlUtils.IsEmptyUrl(content))
            {
                // If the file is empty, return the default home page
                return "https://www.hw.ac.uk/";
            }
            return content;
        }

        /**
         * SetHome is a method that sets the home page.
         * It writes the URL to the home page file.
         */
        public void SetHome(string url)
        {
            FileUtils.EnsureFileExists(filePath); // Ensure that the file exists
            File.WriteAllText(filePath, UrlUtils.AddHttp(url)); // Write the URL to the home page file
        }
    }
}