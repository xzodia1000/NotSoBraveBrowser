using NotSoBraveBrowser.src.HttpRequests;

namespace NotSoBraveBrowser.src.TabControl

{
    public class Tab : Button
    {
        private string title { set; get; }
        private Requests client;
        public TabContent content;
        private Button closeButton;
        private readonly TabPanel panel;

        public Tab(string title, TabPanel panel)
        {
            this.title = title;
            this.panel = panel;
            client = new Requests();
            content = new TabContent(this);
            closeButton = new Button();

            InitTab();
            InitCloseButton();
        }

        private void InitTab()
        {
            SuspendLayout();
            Name = title;
            Text = GetDisplayText();
            Size = new Size(100, 28);
            Margin = new Padding(1);
            TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(this);
            Click += Tab_Click;

        }

        private void Tab_Click(object? sender, EventArgs e)
        {
            panel.SetActiveTab(this);
        }

        private void InitCloseButton()
        {
            closeButton.Name = "closeButton";
            closeButton.Text = "X";
            closeButton.Size = new Size(26, 26);
            closeButton.Left = Width - closeButton.Width - 1;
            closeButton.Top = (Height - closeButton.Height) / 2;
            closeButton.TextAlign = ContentAlignment.MiddleCenter;
            closeButton.Click += CloseButton_Click;
            Controls.Add(closeButton);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            panel.CloseTab(this);
        }

        public async void RenderCode(string Url)
        {
            content.renderedContent.Text = "Loading...";
            try
            {
                string html = await Task.Run(() => client.Get(Url));
                content.renderedContent.Text = html;
            }
            catch (HttpRequestException e)
            {
                content.renderedContent.Text = e.StatusCode.ToString();
            }
        }

        private string GetDisplayText()
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(title, Font);

                if (size.Width > Width - 27 && title.Length != 7)
                {
                    return string.Concat(title.AsSpan(0, 7), "...");
                }
            }

            return title;
        }
    }
}
