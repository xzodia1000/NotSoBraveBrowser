using System.Text.RegularExpressions;
using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.HttpRequests;

namespace NotSoBraveBrowser.src.TabControl

{
    public class Tab : Button
    {
        private string tabTitle;
        private readonly TabPanel panel;
        private readonly SettingForm settingForm;
        private string browserTitle;
        private readonly Requests client;
        public TabContent content;
        private readonly Button closeButton;
        private readonly TabHistory tabHistory;

        public Tab(string tabTitle, TabPanel panel, SettingForm settingForm)
        {
            this.tabTitle = tabTitle;
            this.panel = panel;
            this.settingForm = settingForm;
            browserTitle = tabTitle;

            client = new Requests();
            content = new TabContent(this, settingForm);
            closeButton = new Button();
            tabHistory = new TabHistory();

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

        private void Tab_Click(object? sender, EventArgs e)
        {
            panel.SetActiveTab(this);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            panel.CloseTab(this);
        }

        public async void RenderCode(string Url, bool isHistory = false)
        {
            content.renderedContent.Text = "Loading...";
            try
            {
                string html = await Task.Run(() => client.Get(Url));
                content.renderedContent.Text = html;

                tabTitle = GetTitle(html) != "Error" ? GetTitle(html) : "200 OK";
                browserTitle = GetTitle(html) != "Error" ? "200 OK | " + GetTitle(html) : "200 OK";
            }
            catch (HttpRequestException e)
            {
                content.renderedContent.Text = e.StatusCode.ToString() != "" ? $"{(int?)e.StatusCode} {e.StatusCode}" : "The connection has timed out.";

                tabTitle = (e.StatusCode.ToString() != "" ? e.StatusCode.ToString() : "Timeout") ?? tabTitle;
                browserTitle = content.renderedContent.Text;
            }

            if (!isHistory && tabHistory.GetCurrentUrl() != Url)
            {
                tabHistory.Visit(Url);
                settingForm.HistoryUI.globalHistory.AddEntry(Url);
            }
            UpdateTabTitle();
            UpdateBrowserTitle();
        }

        public string GoBack()
        {
            string? url = tabHistory.PrevUrl();

            if (url is not null)
            {
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? "";
        }

        public string GoForward()
        {
            string? url = tabHistory.NextUrl();

            if (url is not null)
            {
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? "";
        }

        public string Reload()
        {
            string? url = tabHistory.GetCurrentUrl();

            if (url is not null)
            {
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? "";
        }

        private static string GetTitle(string html)
        {
            Regex titleRegex = new("<title>(.+?)</title>", RegexOptions.IgnoreCase);
            Match match = titleRegex.Match(html);

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

        private void UpdateTabTitle()
        {
            Name = tabTitle;
            Text = GetDisplayText();
        }

        public void UpdateBrowserTitle()
        {
            panel.Parent!.Parent!.Text = "NotSoBraveBrowser - " + browserTitle;
        }
    }
}
