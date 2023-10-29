namespace NotSoBraveBrowser.lib
{
    /**
     * FileUtils is a class that provides methods to work with files.
     */
    public static class FileUtils
    {
        /**
         * EnsureFileExists is a method that ensures that the file exists.
         * If the file does not exist, it creates the file.
         */
        public static void EnsureFileExists(string filePath)
        {
            try
            {
                // Try to open the file and create it if it does not exist
                File.Open(filePath, FileMode.OpenOrCreate).Close();
            }
            catch (DirectoryNotFoundException)
            {
                // If the directory does not exist, create the directory and create the file
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
            }
        }
    }
}