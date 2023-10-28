using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Bookmark
{
    public class BookmarkUI : Form
    {
        private readonly BrowserForm browserForm;
        public BookmarkManager bookmarkManager;
        private readonly ListView bookmarkTable;

        public BookmarkUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            bookmarkManager = new BookmarkManager();
            bookmarkTable = new ListView();

            InitBookmarkUI();
            InitBookmarkTable();
        }

        private void InitBookmarkUI()
        {
            Text = "Bookmark";
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

        private void InitBookmarkTable()
        {
            bookmarkTable.Name = "bookmarkTable";
            bookmarkTable.View = View.Details;
            bookmarkTable.FullRowSelect = true;
            bookmarkTable.GridLines = true;
            bookmarkTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            bookmarkTable.MultiSelect = false;
            bookmarkTable.Size = new Size(800 - SystemInformation.VerticalScrollBarWidth, 600);
            bookmarkTable.MouseMove += BookmarkTable_MouseMove;
            bookmarkTable.Click += BookmarkTable_Click;

            bookmarkTable.Columns.Add("Date", 100);
            bookmarkTable.Columns.Add("Time", 100);
            bookmarkTable.Columns.Add("URL", 570 - SystemInformation.VerticalScrollBarWidth);

            Controls.Add(bookmarkTable);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
                bookmarkTable.Items.Clear();
            }
        }

        private void BookmarkTable_Click(object sender, EventArgs e)
        {
            string url = bookmarkTable.SelectedItems[0].SubItems[2].Text;
            browserForm.NewTab("New Tab", url);
        }

        private void BookmarkTable_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem? item = bookmarkTable.GetItemAt(e.X, e.Y);

            if (item != null)
            {
                bookmarkTable.Cursor = Cursors.Hand;
            }
            else
            {
                bookmarkTable.Cursor = Cursors.Default;
            }
        }

        private void UpdateBookmarkTable(List<BookmarkEntry> bookmarkEntries)
        {
            foreach (BookmarkEntry entry in bookmarkEntries)
            {
                bookmarkTable.Items.Add(new ListViewItem(new[] { entry.Time.ToShortDateString(), entry.Time.ToShortTimeString(), entry.Url }));
            }
        }

        public void OpenBookmark()
        {
            Show();
            UpdateBookmarkTable(bookmarkManager.GetBookmarks());
        }
    }
}