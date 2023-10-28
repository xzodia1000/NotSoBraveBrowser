using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;

namespace NotSoBraveBrowser.models
{
    public class SettingForm
    {
        public HistoryUI HistoryUI { get; set; }
        public DownloadUI DownloadUI { get; set; }
        public BookmarkUI BookmarkUI { get; set; }

        public SettingForm(HistoryUI historyUI, DownloadUI downloadUI, BookmarkUI bookmarkUI)
        {
            HistoryUI = historyUI;
            DownloadUI = downloadUI;
            BookmarkUI = bookmarkUI;
        }
    }
}