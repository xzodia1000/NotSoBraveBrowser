using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    public class UtilBar : FlowLayoutPanel
    {
        private readonly Tab tab;
        private readonly SettingForm settingForm;
        private readonly Button prevButton;
        private readonly Button nextButton;
        private readonly TextBox urlTextBox;
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
            prevButton.Text = "<";
            prevButton.Size = new Size(28, 28);
            prevButton.Margin = new Padding(1);
            prevButton.TextAlign = ContentAlignment.MiddleCenter;
            prevButton.Click += PrevButton_Click;
            Controls.Add(prevButton);
        }

        private void InitNextButton()
        {
            nextButton.Name = "nextButton";
            nextButton.Text = ">";
            nextButton.Size = new Size(28, 28);
            nextButton.Margin = new Padding(1);
            nextButton.TextAlign = ContentAlignment.MiddleCenter;
            nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);
        }

        private void InitRefreshButton()
        {
            refreshButton.Name = "refreshButton";
            refreshButton.Text = "R";
            refreshButton.Size = new Size(28, 28);
            refreshButton.Margin = new Padding(1);
            refreshButton.TextAlign = ContentAlignment.MiddleCenter;
            refreshButton.Click += RefreshButton_Click;
            Controls.Add(refreshButton);
        }

        private void InitUrlTextBox()
        {
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Text = "http://";
            urlTextBox.Height = 28;
            urlTextBox.Width = Width - 28 * 6;
            urlTextBox.TextAlign = HorizontalAlignment.Left;
            Controls.Add(urlTextBox);
        }

        private void InitGoButton()
        {
            goButton.Name = "goButton";
            goButton.Text = "G";
            goButton.Size = new Size(28, 28);
            goButton.Margin = new Padding(1);
            goButton.TextAlign = ContentAlignment.MiddleCenter;
            goButton.Click += GoButton_Click;
            Controls.Add(goButton);
        }


        private void InitSettingButton()
        {
            settingButton.Name = "settingButton";
            settingButton.Image = ResizeImage(IconImage.menuIcon, 22, 22);
            settingButton.Size = new Size(28, 28);
            settingButton.Margin = new Padding(1);
            settingButton.Click += SettingsButton_Click;
            Controls.Add(settingButton);
        }

        private void InitSettingsMenu()
        {
            settingsMenu.Name = "settingsMenu";

            ToolStripMenuItem historyItem = new("History");
            historyItem.Click += (sender, e) => settingForm.HistoryUI.Open();
            settingsMenu.Items.Add(historyItem);

            settingsMenu.Items.Add("Bookmarks");
            settingsMenu.Items.Add("Download Manager");
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

        private static Image ResizeImage(Image image, int width, int height)
        {
            Bitmap bitmap = new(image, new Size(width, height));
            return bitmap;
        }
    }
}
