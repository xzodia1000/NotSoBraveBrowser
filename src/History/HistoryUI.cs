using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.History
{
    /**
     * HistoryUI is a form that displays the history of the browser.
     */
    public class HistoryUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        public GlobalHistory globalHistory; // The global history
        private readonly ListView historyTable;

        /**
         * HistoryUI is the constructor of the HistoryUI class.
         * It takes a BrowserForm object as a parameter.
         * It initializes the global history and the history table.
         */
        public HistoryUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            globalHistory = new GlobalHistory();
            historyTable = new ListView();

            InitHistoryUI();
            InitHistoryTable();
        }

        /**
         * InitHistoryUI is a method that initializes the history UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
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

        /**
         * InitHistoryTable is a method that initializes the history table.
         * It sets the properties of the history table.
         * It also sets the event handlers for the mouse move and click events.
         */
        private void InitHistoryTable()
        {
            historyTable.Name = "historyTable";
            historyTable.View = View.Details;
            historyTable.FullRowSelect = true;
            historyTable.GridLines = true;
            historyTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            historyTable.MultiSelect = false;

            // Set the size of the history table considering the vertical scroll bar width
            historyTable.Size = new Size(800 - SystemInformation.VerticalScrollBarWidth, 600);
            historyTable.MouseMove += HistoryTable_MouseMove;
            historyTable.Click += HistoryTable_Click;

            historyTable.Columns.Add("Date", 100);
            historyTable.Columns.Add("Time", 100);
            historyTable.Columns.Add("URL", 570 - SystemInformation.VerticalScrollBarWidth);

            Controls.Add(historyTable); // Add the history table to the form
        }

        /**
         * Form_FormClosing is an event handler that is called when the form is closing.
         * It takes a FormClosingEventArgs object as a parameter.
         * It cancels the form close request and hides the form.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
                historyTable.Items.Clear(); // clears the history table
            }
        }

        /**
         * HistoryTable_Click is an event handler that is called when the history table is clicked.
         * It takes an object and an EventArgs object as parameters.
         * It opens a new tab with the URL of the history entry.
         */
        private void HistoryTable_Click(object? sender, EventArgs e)
        {
            // Open a new tab with the URL of the history entry
            string url = historyTable.SelectedItems[0].SubItems[2].Text;
            browserForm.NewTab("New Tab", url); // Open a new tab with the URL of the history entry
        }

        /**
         * HistoryTable_MouseMove is an event handler that is called when the mouse is moved over the history table.
         * It takes an object and a MouseEventArgs object as parameters.
         * It changes the cursor to a hand cursor if the mouse is over a history entry.
         */
        private void HistoryTable_MouseMove(object? sender, MouseEventArgs e)
        {
            // Change the cursor to a hand cursor if the mouse is over a history entry
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

        /**
         * UpdateHistoryTable is a method that updates the history table.
         * It takes a list of HistoryEntry objects as a parameter.
         * It clears the history table and adds the history entries to the history table.
         */
        private void UpdateHistoryTable(List<HistoryEntry> historyEntries)
        {
            historyEntries.Reverse(); // Reverse the list of history entries
            foreach (HistoryEntry entry in historyEntries)
            {
                // Add the history entry to the history table
                historyTable.Items.Add(new ListViewItem(new[] { entry.Time.ToShortDateString(), entry.Time.ToShortTimeString(), entry.Url }));
            }
        }

        /**
         * OpenHistory is a method that opens the history UI.
         * It shows the form and updates the history table.
         */
        public void OpenHistory()
        {
            Show(); // Show the history UI
            UpdateHistoryTable(globalHistory.GetHistory()); // Update the history table
        }
    }
}