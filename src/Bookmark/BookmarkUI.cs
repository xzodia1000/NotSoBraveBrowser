using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;
using NotSoBraveBrowser.src.Home;

namespace NotSoBraveBrowser.src.Bookmark
{
    /**
     * BookmarkUI is a form that displays the bookmarks of the browser.
     */
    public class BookmarkUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        public BookmarkManager bookmarkManager; // The bookmark manager
        public AddBookmarkUI addBookmarkUI; // The add bookmark UI
        public EditBookmarkUI editBookmarkUI; // The edit bookmark UI
        private readonly ListView bookmarkTable; // The bookmark table that displays the bookmarks

        /**
         * BookmarkUI is the constructor of the BookmarkUI class.
         * It takes a BrowserForm object as a parameter.
         * It initializes the bookmark manager and the bookmark table.
         */
        public BookmarkUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            bookmarkManager = new BookmarkManager();
            addBookmarkUI = new AddBookmarkUI(browserForm, bookmarkManager);
            editBookmarkUI = new EditBookmarkUI(browserForm, bookmarkManager);
            bookmarkTable = new ListView();

            InitBookmarkUI();
            InitBookmarkTable();
        }

        /**
         * InitBookmarkUI is a method that initializes the bookmark UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
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
            FormClosing += Form_FormClosing; // Set the event handler for the form closing event
        }

        /**
         * InitBookmarkTable is a method that initializes the bookmark table.
         * It sets the properties of the bookmark table.
         * It also sets the event handlers for the mouse move and click events.
         */
        private void InitBookmarkTable()
        {
            bookmarkTable.Name = "bookmarkTable";
            bookmarkTable.View = View.Details;
            bookmarkTable.FullRowSelect = true;
            bookmarkTable.GridLines = true;
            bookmarkTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            bookmarkTable.MultiSelect = false;
            bookmarkTable.Size = new Size(800 - SystemInformation.VerticalScrollBarWidth, 600); // Set the size of the bookmark table considering the width of the vertical scroll bar
            bookmarkTable.MouseMove += BookmarkTable_MouseMove; // Set the event handler for the mouse move event
            bookmarkTable.Click += BookmarkTable_Click; // Set the event handler for the click event

            bookmarkTable.Columns.Add("Name", 200);
            bookmarkTable.Columns.Add("URL", 570 - SystemInformation.VerticalScrollBarWidth);

            Controls.Add(bookmarkTable);
        }

        /**
         * Form_FormClosing is an event handler for the form closing event.
         * It takes a FormClosingEventArgs object as a parameter.
         * It cancels the form close request and hides the form so it can be reopened later.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
                bookmarkTable.Items.Clear(); // clears the bookmark table
            }
        }

        /**
         * BookmarkTable_Click is an event handler for the click event of the bookmark table.
         * It takes an object and an EventArgs object as parameters.
         * It opens the URL of the bookmark in a new tab.
         */
        private void BookmarkTable_Click(object? sender, EventArgs e)
        {
            string url = bookmarkTable.SelectedItems[0].SubItems[2].Text; // Get the URL of the bookmark
            browserForm.NewTab("New Tab", url);
        }

        /**
         * BookmarkTable_MouseMove is an event handler for the mouse move event of the bookmark table.
         * It takes an object and a MouseEventArgs object as parameters.
         * It changes the cursor to a hand cursor if the mouse is over a bookmark.
         */
        private void BookmarkTable_MouseMove(object? sender, MouseEventArgs e)
        {
            ListViewItem? item = bookmarkTable.GetItemAt(e.X, e.Y);

            if (item != null)
            {
                bookmarkTable.Cursor = Cursors.Hand; // Change the cursor to a hand cursor if the mouse is over a bookmark
            }
            else
            {
                bookmarkTable.Cursor = Cursors.Default; // Change the cursor to a default cursor
            }
        }

        /**
         * UpdateBookmarkTable is a method that updates the bookmark table.
         * It takes a list of BookmarkEntry objects as a parameter.
         * It clears the bookmark table and adds the bookmarks to the bookmark table.
         */
        private void UpdateBookmarkTable(List<BookmarkEntry> bookmarkEntries)
        {
            foreach (BookmarkEntry entry in bookmarkEntries)
            {
                // Add the bookmark to the bookmark table
                bookmarkTable.Items.Add(new ListViewItem(new[] { entry.Name, entry.Url }));
            }
        }

        /**
         * OpenBookmark is a method that opens the bookmark UI.
         * It shows the bookmark UI and updates the bookmark table.
         */
        public void OpenBookmark()
        {
            Show(); // Show the bookmark UI
            UpdateBookmarkTable(bookmarkManager.GetBookmarks()); // Update the bookmark table
        }
    }
}