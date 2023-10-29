using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Home
{
    /**
     * EditBookmarkUI is a form that displays the edit bookmark UI.
     */
    public class EditBookmarkUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        private readonly BookmarkManager bookmarkManager; // The bookmark manager
        private string url; // The URL of the bookmark
        private Action UpdateBookmarkButton;
        private readonly TextBox nameTextBox;
        private readonly Button editBookmarkButton;
        private readonly Button deleteBookmarkButton;

        /**
         * EditBookmarkUI is the constructor of the EditBookmarkUI class.
         * It takes a BrowserForm object and a BookmarkManager object as parameters.
         * It initializes the update bookmark button, the bookmark manager, etc.
         */
        public EditBookmarkUI(BrowserForm browserForm, BookmarkManager bookmarkManager)
        {
            this.browserForm = browserForm;
            this.bookmarkManager = bookmarkManager;
            url = ""; // Set the URL of the bookmark to an empty string
            UpdateBookmarkButton = () => { }; // Set the update bookmark button to an empty function
            nameTextBox = new TextBox();
            editBookmarkButton = new Button();
            deleteBookmarkButton = new Button();

            InitEditBookmarkUI();
            InitNameTextBox();
            InitEditBookmarkButton();
            InitDeleteBookmarkButton();
        }

        /**
         * InitEditBookmarkUI is a method that initializes the edit bookmark UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
        private void InitEditBookmarkUI()
        {
            Text = "Edit Bookmark";
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
         * InitNameTextBox is a method that initializes the name text box.
         * It sets the properties of the name text box.
         */
        private void InitNameTextBox()
        {
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(400, 30);
            nameTextBox.Location = new Point(50, 50);
            nameTextBox.Text = "";
            Controls.Add(nameTextBox);
        }

        /**
         * InitEditBookmarkButton is a method that initializes the edit bookmark button.
         * It sets the properties of the edit bookmark button.
         */
        private void InitEditBookmarkButton()
        {
            editBookmarkButton.Name = "editBookmarkButton";
            editBookmarkButton.Size = new Size(100, 30);
            editBookmarkButton.Location = new Point(50, 100);
            editBookmarkButton.Text = "Edit Name";
            editBookmarkButton.Click += EditBookmarkButton_Click;
            Controls.Add(editBookmarkButton);
        }

        /**
         * InitDeleteBookmarkButton is a method that initializes the delete bookmark button.
         * It sets the properties of the delete bookmark button.
         */
        private void InitDeleteBookmarkButton()
        {
            deleteBookmarkButton.Name = "deleteBookmarkButton";
            deleteBookmarkButton.Size = new Size(100, 30);
            deleteBookmarkButton.Location = new Point(200, 100);
            deleteBookmarkButton.Text = "Delete";
            deleteBookmarkButton.Click += DeleteBookmarkButton_Click;
            Controls.Add(deleteBookmarkButton);
        }

        /**
         * Form_FormClosing is an event handler for the form closing event.
         * It takes a FormClosingEventArgs object as a parameter.
         * It enables the browser form and hides the edit bookmark UI.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                CloseEditBookmark(); // Close the edit bookmark UI
            }
        }

        /**
         * EditBookmarkButton_Click is an event handler for the click event of the set edit bookmark button.
         * It takes a object and a EventArgs object as parameters.
         * It edits the name of the bookmark in the bookmarks file.
         */
        private void EditBookmarkButton_Click(object? sender, EventArgs e)
        {
            bookmarkManager.EditBookmark(url, nameTextBox.Text); // Set the home page to the text of the home text box
            CloseEditBookmark();
        }

        /**
         * DeleteBookmarkButton_Click is an event handler for the click event of the delete bookmark button.
         * It takes a object and a EventArgs object as parameters.
         * It deletes the bookmark from the bookmarks file.
         */
        private void DeleteBookmarkButton_Click(object? sender, EventArgs e)
        {
            bookmarkManager.RemoveBookmark(url);
            CloseEditBookmark();
        }

        /**
         * OpenEditBookmark is a method that opens the add bookmark UI.
         * It takes a string as a parameter, which is the URL of the bookmark.
         * It takes an Action as a parameter, which is the function to update the bookmark button.
         * It shows the add bookmark UI and disables the browser form.
         */
        public void OpenEditBookmark(string url, Action UpdateBookmarkButton)
        {
            Show(); // Show the home UI
            this.url = url;
            this.UpdateBookmarkButton = UpdateBookmarkButton;
            nameTextBox.Text = bookmarkManager.GetName(url) ?? ""; // Set the text of the name text box to the name of the bookmark
            browserForm.Enabled = false; // Disable the browser form
        }

        /**
         * CloseEditBookmark is a method that closes the edit bookmark UI.
         * It enables the browser form and hides the edit bookmark UI.
         */
        private void CloseEditBookmark()
        {
            browserForm.Enabled = true; // Enable the browser form
            nameTextBox.Text = ""; // Set the text of the name text box to an empty string
            url = ""; // Set the URL of the bookmark to an empty string
            UpdateBookmarkButton();
            Hide();   // hides the form
        }
    }
}