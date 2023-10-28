using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    public class UtilBar : FlowLayoutPanel
    {
        private readonly Tab tab;
        private readonly SettingForm settingForm;
        private readonly Button prevButton;
        private readonly Button nextButton;
        public readonly TextBox urlTextBox;
        private readonly Button goButton;
        private readonly Button refreshButton;
        private readonly Button settingButton;
        private ContextMenuStrip settingsMenu;

        public UtilBar(Tab tab, SettingForm settingForm)
        {
            this.tab = tab;
            this.settingForm = settingForm;

            prevButton = new Button();
            nextButton = new Button();
            refreshButton = new Button();
            urlTextBox = new TextBox();
            goButton = new Button();
            settingButton = new Button();
            settingsMenu = new ContextMenuStrip();

            InitUtilBar();
            InitPrevButton();
            InitNextButton();
            InitRefreshButton();
            InitUrlTextBox();
            InitGoButton();
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

            nextButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);
        }

        private void InitRefreshButton()
        {
            refreshButton.Name = "refreshButton";
            refreshButton.Image = ImageUtil.ResizeImage(IconImage.reloadIcon, 18, 18);
            refreshButton.Size = new Size(28, 28);
            refreshButton.Margin = new Padding(1);
            refreshButton.ImageAlign = ContentAlignment.MiddleCenter;

            refreshButton.MouseHover += (sender, e) => Cursor = Cursors.Hand;
            refreshButton.Click += RefreshButton_Click;
            Controls.Add(refreshButton);
        }

        private void InitUrlTextBox()
        {
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Text = "http://";
            urlTextBox.Height = 28;
            urlTextBox.Width = Width - 28 * 6;
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

            settingsMenu.Items.Add("Bookmarks");

            ToolStripMenuItem downloadItem = new("Download");
            downloadItem.Click += (sender, e) => settingForm.DownloadUI.OpenDownload();
            settingsMenu.Items.Add(downloadItem);

            settingsMenu.Items.Add("Change Homepage");
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

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            string url = tab.Reload();
            urlTextBox.Text = url;
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            tab.RenderCode(urlTextBox.Text);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            settingsMenu.Show(settingButton, new Point(-settingsMenu.Width + settingButton.Width, settingButton.Height));
        }

        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width;
            urlTextBox.Width = Width - 28 * 6;
        }


    }
}
