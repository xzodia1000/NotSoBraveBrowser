using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.History
{
    public class HistoryUI : Form
    {
        private readonly BrowserForm browserForm;
        public GlobalHistory globalHistory;
        private readonly ListView historyTable;
        public HistoryUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            globalHistory = new GlobalHistory();
            historyTable = new ListView();

            InitHistoryUI();
            InitHistoryTable();
        }

        private void InitHistoryUI()
        {
            Text = "History";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosing += Form_FormClosing;
        }

        private void InitHistoryTable()
        {
            historyTable.Name = "historyTable";
            historyTable.View = View.Details;
            historyTable.FullRowSelect = true;
            historyTable.GridLines = true;
            historyTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            historyTable.MultiSelect = false;
            historyTable.Size = new Size(800 - SystemInformation.VerticalScrollBarWidth, 600);
            historyTable.MouseMove += HistoryTable_MouseMove;
            historyTable.Click += HistoryTable_Click;

            historyTable.Columns.Add("Date", 100);
            historyTable.Columns.Add("Time", 100);
            historyTable.Columns.Add("URL", 570 - SystemInformation.VerticalScrollBarWidth);

            Controls.Add(historyTable);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
                historyTable.Items.Clear();
            }
        }

        private void HistoryTable_Click(object sender, EventArgs e)
        {
            string url = historyTable.SelectedItems[0].SubItems[2].Text;
            browserForm.NewTab("New Tab", url);
        }

        private void HistoryTable_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem? item = historyTable.GetItemAt(e.X, e.Y);

            if (item != null)
            {
                historyTable.Cursor = Cursors.Hand;
            }
            else
            {
                historyTable.Cursor = Cursors.Default;
            }
        }

        private void UpdateHistoryTable(List<HistoryEntry> historyEntries)
        {
            historyEntries.Reverse();
            foreach (HistoryEntry entry in historyEntries)
            {
                historyTable.Items.Add(new ListViewItem(new[] { entry.Time.ToShortDateString(), entry.Time.ToShortTimeString(), entry.Url }));
            }
        }

        public void OpenHistory()
        {
            Show();
            UpdateHistoryTable(globalHistory.GetHistory());
        }
    }
}