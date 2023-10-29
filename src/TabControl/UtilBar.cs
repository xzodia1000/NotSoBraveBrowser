using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    /**
     * UtilBar is a FlowLayoutPanel that contains the address bar and the buttons.
     */
    public class UtilBar : FlowLayoutPanel
    {
        private readonly Tab tab; // The tab that the UtilBar belongs to
        private readonly SettingForm settingForm; // SettingForm object that contains the settings
        bool? isBookmark = null; // Whether the current page is bookmarked or not
        private readonly Button prevButton; // Button to go back
        private readonly Button nextButton; // Button to go forward
        private readonly Button homeButton; // Button to go to the homepage
        private readonly Button reloadButton; // Button to reload the page
        public readonly TextBox urlTextBox; // TextBox to enter the URL
        private readonly Button goButton; // Button to go to the URL
        private readonly Button bookmarkButton; // Button to bookmark the page or remove the bookmark
        private readonly Button settingButton; // Button to open the settings menu
        private readonly ContextMenuStrip settingsMenu; // Settings menu

        /**
         * UtilBar constructor initializes the tab and the settingForm.
         * It also initializes the buttons and the settings menu.
         */
        public UtilBar(Tab tab, SettingForm settingForm)
        {
            this.tab = tab;
            this.settingForm = settingForm;
            isBookmark = null;

            prevButton = new Button();
            nextButton = new Button();
            homeButton = new Button();
            reloadButton = new Button();
            urlTextBox = new TextBox();
            goButton = new Button();
            bookmarkButton = new Button();
            settingButton = new Button();
            settingsMenu = new ContextMenuStrip();

            InitUtilBar();
            InitPrevButton();
            InitNextButton();
            InitHomeButton();
            InitReloadButton();
            InitUrlTextBox();
            InitGoButton();
            InitBookmarkButton();
            InitSettingButton();
            InitSettingsMenu();
        }

        /**
         * InitUtilBar initializes the UtilBar FlowLayoutPanel.
         * It sets the properties of the FlowLayoutPanel.
         */
        public void InitUtilBar()
        {
            Height = 30;
            BackColor = Color.White;
            FlowDirection = FlowDirection.LeftToRight;
            MouseHover += (sender, e) => Cursor = Cursors.Default;
        }

        /**
         * InitPrevButton initializes the prevButton.
         * It sets the properties of the button.
         */
        private void InitPrevButton()
        {
            prevButton.Name = "prevButton";
            prevButton.Image = ImageUtil.ResizeImage(IconImage.previousIcon, 18, 18); // Sets the icon of the button
            prevButton.Size = new Size(28, 28);
            prevButton.Margin = new Padding(1);
            prevButton.ImageAlign = ContentAlignment.MiddleCenter;
            prevButton.Enabled = false;

            prevButton.Click += PrevButton_Click;
            Controls.Add(prevButton); // Add the button to the UtilBar
        }

        /**
         * InitNextButton initializes the nextButton.
         * It sets the properties of the button.
         */
        private void InitNextButton()
        {
            nextButton.Name = "nextButton";
            nextButton.Image = ImageUtil.ResizeImage(IconImage.nextIcon, 18, 18); // Sets the icon of the button
            nextButton.Size = new Size(28, 28);
            nextButton.Margin = new Padding(1);
            nextButton.ImageAlign = ContentAlignment.MiddleCenter;
            nextButton.Enabled = false;

            nextButton.Click += NextButton_Click;
            Controls.Add(nextButton); // Add the button to the UtilBar
        }

        /**
         * InitHomeButton initializes the homeButton.
         * It sets the properties of the button.
         */
        private void InitHomeButton()
        {
            homeButton.Name = "homeButton";
            homeButton.Image = ImageUtil.ResizeImage(IconImage.homeIcon, 18, 18); // Sets the icon of the button
            homeButton.Size = new Size(28, 28);
            homeButton.Margin = new Padding(1);
            homeButton.ImageAlign = ContentAlignment.MiddleCenter;

            homeButton.MouseHover += (sender, e) => homeButton.Cursor = Cursors.Hand;
            homeButton.Click += (sender, e) => tab.RenderCode(settingForm.HomeUI.homeManager.GetHome()); // Go to the homepage when clicked
            Controls.Add(homeButton); // Add the button to the UtilBar
        }

        /**
         * InitReloadButton initializes the reloadButton.
         * It sets the properties of the button.
         */
        private void InitReloadButton()
        {
            reloadButton.Name = "reloadButton";
            reloadButton.Image = ImageUtil.ResizeImage(IconImage.reloadIcon, 18, 18); // Sets the icon of the button
            reloadButton.Size = new Size(28, 28);
            reloadButton.Margin = new Padding(1);
            reloadButton.ImageAlign = ContentAlignment.MiddleCenter;
            reloadButton.Enabled = false;

            reloadButton.Click += ReloadButton_Click;
            Controls.Add(reloadButton); // Add the button to the UtilBar
        }

        /**
         * InitUrlTextBox initializes the urlTextBox.
         * It sets the properties of the TextBox.
         */
        private void InitUrlTextBox()
        {
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Text = "http://";
            urlTextBox.Height = 28;
            urlTextBox.Width = Width - 28 * 8; // Set the width of the TextBox to the width of the UtilBar minus the width of the buttons

            urlTextBox.KeyDown += new KeyEventHandler(UrlTextBox_KeyDown); // Add the event handler for the key down event
            Controls.Add(urlTextBox); // Add the TextBox to the UtilBar
        }

        /**
         * InitGoButton initializes the goButton.
         * It sets the properties of the button.
         */
        private void InitGoButton()
        {
            goButton.Name = "goButton";
            goButton.Image = ImageUtil.ResizeImage(IconImage.searchIcon, 18, 18); // Sets the icon of the button
            goButton.Size = new Size(28, 28);
            goButton.Margin = new Padding(1);
            goButton.ImageAlign = ContentAlignment.MiddleCenter;

            goButton.MouseHover += (sender, e) => goButton.Cursor = Cursors.Hand;
            goButton.Click += GoButton_Click;
            Controls.Add(goButton);
        }

        /**
         * InitBookmarkButton initializes the bookmarkButton.
         * It sets the properties of the button.
         */
        private void InitBookmarkButton()
        {
            bookmarkButton.Name = "bookmarkButton";
            bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkDisabledIcon, 18, 18); // Sets the icon of the button
            bookmarkButton.Size = new Size(28, 28);
            bookmarkButton.Margin = new Padding(1);
            bookmarkButton.ImageAlign = ContentAlignment.MiddleCenter;
            bookmarkButton.Enabled = false;

            bookmarkButton.Click += BookmarkButton_Click;
            Controls.Add(bookmarkButton); // Add the button to the UtilBar
        }

        /**
         * InitSettingButton initializes the settingButton.
         * It sets the properties of the button.
         */
        private void InitSettingButton()
        {
            settingButton.Name = "settingButton";
            settingButton.Image = ImageUtil.ResizeImage(IconImage.menuIcon, 18, 18); // Sets the icon of the button
            settingButton.Size = new Size(28, 28);
            settingButton.Margin = new Padding(1);
            settingButton.ImageAlign = ContentAlignment.MiddleCenter;

            settingButton.MouseHover += (sender, e) => settingButton.Cursor = Cursors.Hand;
            settingButton.Click += SettingsButton_Click;
            Controls.Add(settingButton);
        }

        /**
         * InitSettingsMenu initializes the settingsMenu.
         * It sets the properties of the ContextMenuStrip.
         */
        private void InitSettingsMenu()
        {
            settingsMenu.Name = "settingsMenu";

            // Add the history menu item
            ToolStripMenuItem historyItem = new("History");
            historyItem.Click += (sender, e) => settingForm.HistoryUI.OpenHistory(); // Open the history when clicked
            settingsMenu.Items.Add(historyItem);

            // Add the bookmark menu item
            ToolStripMenuItem bookmarkItem = new("Bookmarks");
            bookmarkItem.Click += (sender, e) => settingForm.BookmarkUI.OpenBookmark(); // Open the bookmark when clicked
            settingsMenu.Items.Add(bookmarkItem);

            // Add the download menu item
            ToolStripMenuItem downloadItem = new("Download Manager");
            downloadItem.Click += (sender, e) => settingForm.DownloadUI.OpenDownload(); // Open the download manager when clicked
            settingsMenu.Items.Add(downloadItem);

            // Add the home menu item
            ToolStripMenuItem homeItem = new("Change Homepage");
            homeItem.Click += (sender, e) => settingForm.HomeUI.OpenHome(); // Open the home when clicked
            settingsMenu.Items.Add(homeItem);
        }

        /**
         * PrevButton_Click is the event handler for the prevButton.
         * It goes back to the previous page.
         */
        private void PrevButton_Click(object sender, EventArgs e)
        {
            string url = tab.GoBack(); // Go back to the previous page
            urlTextBox.Text = url; // Set the URL to the previous page
        }

        /**
         * NextButton_Click is the event handler for the nextButton.
         * It goes forward to the next page.
         */
        private void NextButton_Click(object sender, EventArgs e)
        {
            string url = tab.GoForward(); // Go forward to the next page
            urlTextBox.Text = url; // Set the URL to the next page
        }

        /**
         * ReloadButton_Click is the event handler for the reloadButton.
         * It reloads the page.
         */
        private void ReloadButton_Click(object sender, EventArgs e)
        {
            string url = tab.Reload(); // Reload the page
            urlTextBox.Text = url;
        }

        /**
         * UrlTextBox_KeyDown is the event handler for the key down event of the urlTextBox.
         * It takes an object and a KeyEventArgs as parameters.
         * The object is the object that triggered the event.
         * The KeyEventArgs is the event arguments.
         */
        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Go to the URL when the Enter key is pressed
                tab.RenderCode(urlTextBox.Text);

                // This prevents the beep sound on pressing Enter
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /**
         * GoButton_Click is the event handler for the goButton.
         * It goes to the URL.
         */
        private void GoButton_Click(object sender, EventArgs e)
        {
            tab.RenderCode(urlTextBox.Text); // Go to the URL
        }

        /**
         * BookmarkButton_Click is the event handler for the bookmarkButton.
         * It adds or removes the bookmark.
         */
        private void BookmarkButton_Click(object sender, EventArgs e)
        {
            if (isBookmark == true)
            {
                settingForm.BookmarkUI.editBookmarkUI.OpenEditBookmark(urlTextBox.Text, UpdateBookmarkButton); // Open the edit bookmark UI
            }
            else if (isBookmark == false)
            {
                settingForm.BookmarkUI.addBookmarkUI.OpenAddBookmark(urlTextBox.Text, UpdateBookmarkButton); // Open the add bookmark UI
            }
        }

        /**
         * SettingsButton_Click is the event handler for the settingButton.
         * It opens the settings menu.
         */
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            // Open the settings menu and not let it overflow outside the screen
            settingsMenu.Show(settingButton, new Point(-settingsMenu.Width + settingButton.Width, settingButton.Height));
        }

        /**
         * UpdateBackForwardButtons updates the back and forward buttons.
         * It enables or disables the buttons depending on whether the tab can go back or forward.
         */
        public void UpdateBackForwardButtons()
        {
            if (tab.tabHistory.CanGoBack())
            {
                prevButton.MouseHover += (sender, e) => prevButton.Cursor = Cursors.Hand;
                prevButton.Enabled = true;
            }
            else
            {
                prevButton.MouseHover -= (sender, e) => prevButton.Cursor = Cursors.Hand;
                prevButton.Enabled = false;
            }

            if (tab.tabHistory.CanGoForward())
            {
                nextButton.MouseHover += (sender, e) => nextButton.Cursor = Cursors.Hand;
                nextButton.Enabled = true;
            }
            else
            {
                nextButton.MouseHover -= (sender, e) => nextButton.Cursor = Cursors.Hand;
                nextButton.Enabled = false;
            }
        }

        /**
         * UpdateReloadButton updates the reload button.
         * It enables or disables the button depending on whether the URL is empty.
         */
        public void UpdateReloadButton()
        {
            if (UrlUtils.IsEmptyUrl(urlTextBox.Text))
            {
                reloadButton.MouseHover -= (sender, e) => reloadButton.Cursor = Cursors.Hand;
                reloadButton.Enabled = false;
            }
            else
            {
                reloadButton.MouseHover += (sender, e) => reloadButton.Cursor = Cursors.Hand;
                reloadButton.Enabled = true;
            }
        }

        /**
         * UpdateBookmarkButton updates the bookmark button.
         * It changes the icon of the button depending on whether the page is bookmarked.
         */
        public void UpdateBookmarkButton()
        {
            if (!string.IsNullOrEmpty(UrlUtils.NormalizeUrl(urlTextBox.Text))) // Check if the URL is not empty
            {
                // Check if the page is bookmarked
                if (settingForm.BookmarkUI.bookmarkManager.CheckBookmark(urlTextBox.Text))
                {
                    // Set the icon to the bookmark fill icon if the page is bookmarked
                    bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkFillIcon, 18, 18);
                    isBookmark = true; // Set isBookmark to true
                }
                else
                {
                    // Set the icon to the bookmark icon if the page is not bookmarked
                    bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkIcon, 18, 18);
                    isBookmark = false; // Set isBookmark to false
                }
                bookmarkButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
                bookmarkButton.Enabled = true; // Enable the button
            }
            else
            {
                // Set the icon to the bookmark disabled icon if the URL is empty
                bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkDisabledIcon, 18, 18);
                bookmarkButton.MouseHover -= (sender, e) => Cursor = Cursors.Hand;
                bookmarkButton.Enabled = false; // Disable the button
            }
        }

        /**
         * UpdateSize updates the size of the UtilBar.
         * It takes a FlowLayoutPanel as a parameter.
         * The FlowLayoutPanel is the canvas.
         */
        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width; // Set the width of the UtilBar to the width of the canvas
            urlTextBox.Width = Width - 28 * 8; // Set the width of the TextBox to the width of the UtilBar minus the width of the buttons
        }


    }
}
