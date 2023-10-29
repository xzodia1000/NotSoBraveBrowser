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
        public static bool EnsureFileExists(string filePath)
        {
            try
            {
                // If the file exists, do nothing
                File.OpenRead(filePath).Close();
            }
            catch (FileNotFoundException)
            {
                // If the file does not exist, create the file
                File.Create(filePath).Close();
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                // If the directory does not exist, create the directory and create the file
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.Create(filePath).Close();
                return false;
            }

            return true;
        }
    }
}