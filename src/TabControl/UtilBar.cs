using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    public class UtilBar : FlowLayoutPanel
    {
        private readonly Tab tab;
        private readonly SettingForm settingForm;
        bool? isBookmark = null;
        private readonly Button prevButton;
        private readonly Button nextButton;
        private readonly Button homeButton;
        private readonly Button reloadButton;
        public readonly TextBox urlTextBox;
        private readonly Button goButton;
        private readonly Button bookmarkButton;
        private readonly Button settingButton;
        private readonly ContextMenuStrip settingsMenu;

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

        public void InitUtilBar()
        {
            Height = 30;
            BackColor = Color.White;
            FlowDirection = FlowDirection.LeftToRight;
        }

        private void InitPrevButton()
        {
            prevButton.Name = "prevButton";
            prevButton.Image = ImageUtil.ResizeImage(IconImage.previousIcon, 18, 18);
            prevButton.Size = new Size(28, 28);
            prevButton.Margin = new Padding(1);
            prevButton.ImageAlign = ContentAlignment.MiddleCenter;
            prevButton.Enabled = false;

            prevButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            prevButton.Click += PrevButton_Click;
            Controls.Add(prevButton);
        }

        private void InitNextButton()
        {
            nextButton.Name = "nextButton";
            nextButton.Image = ImageUtil.ResizeImage(IconImage.nextIcon, 18, 18);
            nextButton.Size = new Size(28, 28);
            nextButton.Margin = new Padding(1);
            nextButton.ImageAlign = ContentAlignment.MiddleCenter;
            nextButton.Enabled = false;

            nextButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);
        }

        private void InitHomeButton()
        {
            homeButton.Name = "homeButton";
            homeButton.Image = ImageUtil.ResizeImage(IconImage.homeIcon, 18, 18);
            homeButton.Size = new Size(28, 28);
            homeButton.Margin = new Padding(1);
            homeButton.ImageAlign = ContentAlignment.MiddleCenter;

            homeButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            homeButton.Click += (sender, e) => tab.RenderCode(settingForm.HomeUI.homeManager.GetHome());
            Controls.Add(homeButton);
        }

        private void InitReloadButton()
        {
            reloadButton.Name = "reloadButton";
            reloadButton.Image = ImageUtil.ResizeImage(IconImage.reloadIcon, 18, 18);
            reloadButton.Size = new Size(28, 28);
            reloadButton.Margin = new Padding(1);
            reloadButton.ImageAlign = ContentAlignment.MiddleCenter;
            reloadButton.Enabled = false;

            reloadButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            reloadButton.Click += ReloadButton_Click;
            Controls.Add(reloadButton);
        }

        private void InitUrlTextBox()
        {
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Text = "http://";
            urlTextBox.Height = 28;
            urlTextBox.Width = Width - 28 * 8;

            urlTextBox.KeyDown += new KeyEventHandler(UrlTextBox_KeyDown);
            Controls.Add(urlTextBox);
        }

        private void InitGoButton()
        {
            goButton.Name = "goButton";
            goButton.Image = ImageUtil.ResizeImage(IconImage.searchIcon, 18, 18);
            goButton.Size = new Size(28, 28);
            goButton.Margin = new Padding(1);
            goButton.ImageAlign = ContentAlignment.MiddleCenter;

            goButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            goButton.Click += GoButton_Click;
            Controls.Add(goButton);
        }

        private void InitBookmarkButton()
        {
            bookmarkButton.Name = "bookmarkButton";
            bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkDisabledIcon, 18, 18);
            bookmarkButton.Size = new Size(28, 28);
            bookmarkButton.Margin = new Padding(1);
            bookmarkButton.ImageAlign = ContentAlignment.MiddleCenter;
            bookmarkButton.Enabled = false;

            bookmarkButton.Click += BookmarkButton_Click;
            Controls.Add(bookmarkButton);
        }

        private void InitSettingButton()
        {
            settingButton.Name = "settingButton";
            settingButton.Image = ImageUtil.ResizeImage(IconImage.menuIcon, 18, 18);
            settingButton.Size = new Size(28, 28);
            settingButton.Margin = new Padding(1);
            settingButton.ImageAlign = ContentAlignment.MiddleCenter;

            settingButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            settingButton.Click += SettingsButton_Click;
            Controls.Add(settingButton);
        }

        private void InitSettingsMenu()
        {
            settingsMenu.Name = "settingsMenu";

            ToolStripMenuItem historyItem = new("History");
            historyItem.Click += (sender, e) => settingForm.HistoryUI.OpenHistory();
            settingsMenu.Items.Add(historyItem);

            ToolStripMenuItem bookmarkItem = new("Bookmarks");
            bookmarkItem.Click += (sender, e) => settingForm.BookmarkUI.OpenBookmark();
            settingsMenu.Items.Add(bookmarkItem);

            ToolStripMenuItem downloadItem = new("Download Manager");
            downloadItem.Click += (sender, e) => settingForm.DownloadUI.OpenDownload();
            settingsMenu.Items.Add(downloadItem);

            ToolStripMenuItem homeItem = new("Change Homepage");
            homeItem.Click += (sender, e) => settingForm.HomeUI.OpenHome();
            settingsMenu.Items.Add(homeItem);
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            string url = tab.GoBack();
            urlTextBox.Text = url;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            string url = tab.GoForward();
            urlTextBox.Text = url;
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            string url = tab.Reload();
            urlTextBox.Text = url;
        }

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tab.RenderCode(urlTextBox.Text);
                e.Handled = true;  // This prevents the beep sound on pressing Enter
                e.SuppressKeyPress = true;  // Also part of preventing the beep
            }
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            tab.RenderCode(urlTextBox.Text);
        }

        private void BookmarkButton_Click(object sender, EventArgs e)
        {
            if (isBookmark == true)
            {
                bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkIcon, 18, 18);
                settingForm.BookmarkUI.bookmarkManager.RemoveBookmark(urlTextBox.Text);
                isBookmark = false;
            }
            else if (isBookmark == false)
            {
                bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkFillIcon, 18, 18);
                settingForm.BookmarkUI.bookmarkManager.AddBookmark(urlTextBox.Text);
                isBookmark = true;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            settingsMenu.Show(settingButton, new Point(-settingsMenu.Width + settingButton.Width, settingButton.Height));
        }

        public void UpdateBackForwardButtons()
        {
            if (tab.tabHistory.CanGoBack()) prevButton.Enabled = true;
            else prevButton.Enabled = false;

            if (tab.tabHistory.CanGoForward()) nextButton.Enabled = true;
            else nextButton.Enabled = false;
        }

        public void UpdateReloadButton()
        {
            if (UrlUtils.IsEmptyUrl(urlTextBox.Text)) reloadButton.Enabled = false;
            else reloadButton.Enabled = true;
        }

        public void UpdateBookmarkButton()
        {
            if (!UrlUtils.NormalizeUrl(urlTextBox.Text).Equals(""))
            {
                if (settingForm.BookmarkUI.bookmarkManager.CheckBookmark(urlTextBox.Text))
                {
                    bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkFillIcon, 18, 18);
                    isBookmark = true;
                }
                else
                {
                    bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkIcon, 18, 18);
                    isBookmark = false;
                }
                bookmarkButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
                bookmarkButton.Enabled = true;
            }
            else
            {
                bookmarkButton.Image = ImageUtil.ResizeImage(IconImage.bookmarkDisabledIcon, 18, 18);
                bookmarkButton.MouseHover -= (sender, e) => Cursor = Cursors.Hand;
                bookmarkButton.Enabled = false;
            }
        }

        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width;
            urlTextBox.Width = Width - 28 * 8;
        }


    }
}
