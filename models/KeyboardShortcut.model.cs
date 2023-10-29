namespace NotSoBraveBrowser.models
{
    /**
     * KeyboardShortcut is a class that stores the keyboard shortcut data.
     */
    public class KeyboardShortcut
    {
        public string Shortcut { get; set; } // The keyboard shortcut
        public string Description { get; set; } // The action of the keyboard shortcut

        /**
         * KeyboardShortcut is the constructor of the KeyboardShortcut class.
         * It initializes the keyboard shortcut and the action of the keyboard shortcut.
         */
        public KeyboardShortcut(string shortcut, string description)
        {
            Shortcut = shortcut;
            Description = description;
        }
    }
}