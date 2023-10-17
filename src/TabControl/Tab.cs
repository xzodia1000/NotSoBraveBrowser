using System.Text.RegularExpressions;
using NotSoBraveBrowser.src.HttpRequests;
using Browser;

namespace NotSoBraveBrowser.src.TabControl

{
    public partial class Tab : Button
    {
        private string tabTitle;
        private string browserTitle;
        private readonly Requests client;
        public TabContent content;
        private readonly Button closeButton;
        private readonly TabPanel panel;

        public Tab(string tabTitle, TabPanel panel)
        {
            this.tabTitle = tabTitle;
            this.panel = panel;
            browserTitle = "NotSoBraveBrowser - " + tabTitle;
            client = new Requests();
            content = new TabContent(this);
            closeButton = new Button();

            InitTab();
            InitCloseButton();
        }

        private void InitTab()
        {
            SuspendLayout();
            Name = tabTitle;
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

                tabTitle = GetTitle(html) != "Error" ? GetTitle(html) : "200 OK";
                browserTitle = "NotSoBraveBrowser - " + GetTitle(html) != "Error" ? "200 OK | " + GetTitle(html) : "200 OK";
            }
            catch (HttpRequestException e)
            {
                content.renderedContent.Text = e.StatusCode.ToString() != "" ? $"{(int?)e.StatusCode} {e.StatusCode}" : "The connection has timed out.";

                tabTitle = (e.StatusCode.ToString() != "" ? e.StatusCode.ToString() : "Timeout") ?? tabTitle;
                browserTitle = "NotSoBraveBrowser - " + tabTitle;
            }

            UpdateTabTitle();
            UpdateBrowserTitle();
        }

        private void UpdateTabTitle()
        {
            Name = tabTitle;
            Text = GetDisplayText();
        }

        public void UpdateBrowserTitle()
        {
            panel.Parent!.Parent!.Text = browserTitle;
        }


        private static string GetTitle(string html)
        {
            var titleRegex = new Regex("<title>(.+?)</title>", RegexOptions.IgnoreCase);
            var match = titleRegex.Match(html);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return "Error";
        }

        private string GetDisplayText()
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(tabTitle, Font);

                if (size.Width > Width - 27 && tabTitle.Length != 7)
                {
                    return string.Concat(tabTitle.AsSpan(0, 7), "...");
                }
            }

            return tabTitle;
        }
    }
}
