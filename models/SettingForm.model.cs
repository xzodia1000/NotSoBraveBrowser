using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.Home;

namespace NotSoBraveBrowser.models
{
    /**
     * SettingForm is a class that stores the setting form data for the util bar.
     */
    public class SettingForm
    {
        public HistoryUI HistoryUI { get; set; } // The history UI
        public DownloadUI DownloadUI { get; set; } // The download UI
        public BookmarkUI BookmarkUI { get; set; } // The bookmark UI
        public HomeUI HomeUI { get; set; } // The home UI

        /**
         * SettingForm is the constructor of the SettingForm class.
         * It initializes the history UI, the download UI, the bookmark UI and the home UI.
         */
        public SettingForm(HistoryUI historyUI, DownloadUI downloadUI, BookmarkUI bookmarkUI, HomeUI homeUI)
        {
            HistoryUI = historyUI;
            DownloadUI = downloadUI;
            BookmarkUI = bookmarkUI;
            HomeUI = homeUI;
        }
    }
}