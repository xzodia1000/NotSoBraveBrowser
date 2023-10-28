using NotSoBraveBrowser.lib;

namespace NotSoBraveBrowser.src.Home
{
    public class HomeManager
    {
        private readonly string filePath;

        public HomeManager()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(path, "NotSoBraveBrowser", "home.txt");
        }

        public string GetHome()
        {
            try
            {
                string content = File.ReadAllText(filePath);
                if (UrlUtils.IsEmptyUrl(content))
                {
                    return "https://www.hw.ac.uk/";
                }
                return content;
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                return "https://www.hw.ac.uk/";
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return "https://www.hw.ac.uk/";
            }
        }

        public void SetHome(string url)
        {
            try
            {
                File.WriteAllText(filePath, UrlUtils.AddHttp(url));
            }
            catch (FileNotFoundException)
            {
                File.Create(filePath).Close();
                SetHome(url);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                SetHome(url);
            }
        }
    }
}