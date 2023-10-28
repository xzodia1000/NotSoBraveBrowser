namespace NotSoBraveBrowser.models
{
    public class KeyboardShortcut
    {
        public string Shortcut { get; set; }
        public string Description { get; set; }

        public KeyboardShortcut(string shortcut, string description)
        {
            Shortcut = shortcut;
            Description = description;
        }
    }
}