namespace NotSoBraveBrowser.src.TabControl
{
    public class UtilBar : FlowLayoutPanel
    {
        private Tab tab;
        private Button prevButton;
        private Button nextButton;
        private TextBox urlTextBox;
        private Button goButton;
        private Button refreshButton;
        private Button settingButton;
        public UtilBar(Tab tab)
        {
            this.tab = tab;

            prevButton = new Button();
            nextButton = new Button();
            urlTextBox = new TextBox();
            goButton = new Button();
            refreshButton = new Button();
            settingButton = new Button();

            InitUtilBar();
            InitPrevButton();
            InitNextButton();
            InitUrlTextBox();
            InitGoButton();
            InitRefreshButton();
            InitSettingButton();
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
            Controls.Add(prevButton);
        }

        private void InitNextButton()
        {
            nextButton.Name = "nextButton";
            nextButton.Text = ">";
            nextButton.Size = new Size(28, 28);
            nextButton.Margin = new Padding(1);
            nextButton.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(nextButton);
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

        private void InitRefreshButton()
        {
            refreshButton.Name = "refreshButton";
            refreshButton.Text = "R";
            refreshButton.Size = new Size(28, 28);
            refreshButton.Margin = new Padding(1);
            refreshButton.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(refreshButton);
        }

        private void InitSettingButton()
        {
            settingButton.Name = "settingButton";
            settingButton.Text = "S";
            settingButton.Size = new Size(28, 28);
            settingButton.Margin = new Padding(1);
            settingButton.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(settingButton);
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            tab.RenderCode(urlTextBox.Text);
        }

        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width;
            urlTextBox.Width = Width - 28 * 6;
        }
    }
}
