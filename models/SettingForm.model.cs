using NotSoBraveBrowser.src.History;

namespace NotSoBraveBrowser.models
{
    public class SettingForm
    {
        public HistoryUI HistoryUI { get; set; }

        public SettingForm(HistoryUI historyUI)
        {
            this.HistoryUI = historyUI;
        }
    }
}