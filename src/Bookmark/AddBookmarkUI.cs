using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Home
{
    /**
     * AddBookmarkUI is a form that displays the add bookmark UI.
     */
    public class AddBookmarkUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        private readonly BookmarkManager bookmarkManager; // The bookmark manager
        private string url; // The URL of the bookmark
        private Action UpdateBookmarkButton;
        private readonly TextBox nameTextBox;
        private readonly Button addBookmarkButton;

        /**
         * AddBookmarkUI is the constructor of the AddBookmarkUI class.
         * It takes a BrowserForm object and a BookmarkManager object as parameters.
         * It initializes the update bookmark button, the bookmark manager, etc.
         */
        public AddBookmarkUI(BrowserForm browserForm, BookmarkManager bookmarkManager)
        {
            this.browserForm = browserForm;
            this.bookmarkManager = bookmarkManager;
            url = ""; // Set the URL of the bookmark to an empty string
            UpdateBookmarkButton = () => { }; // Set the update bookmark button to an empty function
            nameTextBox = new TextBox();
            addBookmarkButton = new Button();

            InitAddBookmarkUI();
            InitNameTextBox();
            InitAddBookmarkButton();
        }

        /**
         * InitAddBookmarkUI is a method that initializes the add bookmark UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
        private void InitAddBookmarkUI()
        {
            Text = "Add Bookmark";
            Size = new Size(500, 200);
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
         * InitNameTextBox is a method that initializes the home text box.
         * It sets the properties of the name text box.
         */
        private void InitNameTextBox()
        {
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(400, 30);
            nameTextBox.Location = new Point(50, 50);
            nameTextBox.Text = "New Bookmark";
            Controls.Add(nameTextBox);
        }

        /**
         * InitAddBookmarkButton is a method that initializes the add bookmark button.
         * It sets the properties of the add bookmark button.
         */
        private void InitAddBookmarkButton()
        {
            addBookmarkButton.Name = "addBookmarkButton";
            addBookmarkButton.Size = new Size(100, 30);
            addBookmarkButton.Location = new Point(50, 100);
            addBookmarkButton.Text = "Add Bookmark";
            addBookmarkButton.Click += AddBookmarkButton_Click;
            Controls.Add(addBookmarkButton);
        }

        /**
         * Form_FormClosing is an event handler for the form closing event.
         * It takes a FormClosingEventArgs object as a parameter.
         * It enables the browser form and hides the add bookmark UI.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                CloseAddBookmark(); // Close the add bookmark UI
            }
        }

        /**
         * AddBookmarkButton_Click is an event handler for the click event of the add bookmark button.
         * It takes a object and a EventArgs object as parameters.
         * It adds the bookmark to the bookmarks file.
         */
        private void AddBookmarkButton_Click(object? sender, EventArgs e)
        {
            bookmarkManager.AddBookmark(url, nameTextBox.Text); // Add the bookmark to the bookmarks file
            CloseAddBookmark(); // Close the add bookmark UI
        }

        /**
         * OpenAddBookmark is a method that opens the add bookmark UI.
         * It takes a string as a parameter, which is the URL of the bookmark.
         * It also takes an Action as a parameter, which is the function to update the bookmark button.
         * It shows the add bookmark UI and disables the browser form.
         */
        public void OpenAddBookmark(string url, Action UpdateBookmarkButton)
        {
            Show(); // Show the home UI
            this.url = url;
            this.UpdateBookmarkButton = UpdateBookmarkButton;
            browserForm.Enabled = false; // Disable the browser form
        }

        private void CloseAddBookmark()
        {
            browserForm.Enabled = true; // Enable the browser form
            nameTextBox.Text = "New Bookmark"; // Set the text of the home text box to the new home page
            url = "";
            UpdateBookmarkButton();
            Hide();   // hides the form
        }
    }
}