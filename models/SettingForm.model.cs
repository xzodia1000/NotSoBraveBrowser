using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;

namespace NotSoBraveBrowser.models
{
    public class SettingForm
    {
        public HistoryUI HistoryUI { get; set; }
        public DownloadUI DownloadUI { get; set; }

        public SettingForm(HistoryUI historyUI, DownloadUI downloadUI)
        {
            HistoryUI = historyUI;
            DownloadUI = downloadUI;
        }
    }
}