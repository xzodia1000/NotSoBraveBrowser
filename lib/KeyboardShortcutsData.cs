using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.lib
{
    /**
     * KeyboardShortcutData is a class that stores the keyboard shortcuts data.
     */
    public class KeyboardShortcutData
    {
        /**
         * data is a list of KeyboardShortcut objects.
         * It stores the keyboard shortcuts data.
         */
        public static readonly KeyboardShortcut[] data = new KeyboardShortcut[]
        {

            new("Alt + F4", "Close Window"),
            new("Ctrl + Shift + N", "Open New Window"),
            new("Ctrl + T", "Open New Tab"),
            new("Ctrl + W", "Close Tab"),
            new("Ctrl + Tab", "Next Tab"),
            new("Ctrl + Shift + Tab", "Previous Tab"),
            new("Ctrl + R", "Reload Page"),
            new("Alt + Left Arrow", "Go Back"),
            new("Alt + Right Arrow", "Go Forward"),
            new("Ctrl + H", "Open History"),
            new("Ctrl + J", "Open Downloads"),
            new("Ctrl + B", "Open Bookmarks"),
            new("Ctrl + P", "Change Homepage"),
            new("Ctrl + Shift + H", "Open Homepage (New Tab)"),
            new("Ctrl + /", "Open Keyboard Shortcuts (This Window)")
        };
    }
}