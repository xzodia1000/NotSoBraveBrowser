using System.Text.RegularExpressions;
using System.Xml;
using NotSoBraveBrowser.lib;
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
        public readonly TabHistory tabHistory;

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

            MouseHover += (sender, e) => Cursor = Cursors.Hand;
            Click += Tab_Click;
            panel.Controls.Add(this);
        }

        private void InitCloseButton()
        {
            closeButton.Name = "closeButton";
            closeButton.Image = ImageUtil.ResizeImage(IconImage.closeIcon, 18, 18);
            closeButton.Size = new Size(20, 20);
            closeButton.Left = Width - closeButton.Width - 4;
            closeButton.Top = (Height - closeButton.Height) / 2;
            closeButton.ImageAlign = ContentAlignment.MiddleCenter;

            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.BackColor = Color.Transparent;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = closeButton.BackColor;  // Color on hover
            closeButton.FlatAppearance.MouseDownBackColor = closeButton.BackColor;

            closeButton.MouseHover += (sender, e) =>
            {
                closeButton.BackColor = Color.Transparent;
                closeButton.Cursor = Cursors.Hand;
                closeButton.Image = ImageUtil.ResizeImage(IconImage.closeRedIcon, 18, 18);
            };

            closeButton.MouseLeave += (sender, e) =>
            {
                closeButton.Image = ImageUtil.ResizeImage(IconImage.closeIcon, 18, 18);
            };

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

        public async void RenderCode(string url, bool isHistory = false)
        {

            if (UrlUtils.IsEmptyUrl(url))
            {
                return;
            }
            url = UrlUtils.AddHttp(url);

            content.utilBar.urlTextBox.Text = url;
            content.UpdateContent("Loading...", true);

            try
            {
                HttpResponseMessage response = await Task.Run(() => client.Get(url));

                if (response.IsSuccessStatusCode)
                {
                    string html = await response.Content.ReadAsStringAsync();
                    content.UpdateContent(html);

                    tabTitle = GetTitle(html) != "Error" ? GetTitle(html) : "200 OK";
                    browserTitle = GetTitle(html) != "Error" ? "200 OK | " + GetTitle(html) : "200 OK";
                }
                else
                {
                    content.UpdateContent($"{(int)response.StatusCode} {response.ReasonPhrase}", true);
                    tabTitle = $"{response.StatusCode}";
                    browserTitle = content.renderedContent.Name;
                }
            }
            catch (UriFormatException)
            {
                content.UpdateContent("Invalid URL", true);
                tabTitle = "Invalid URL";
                browserTitle = "Invalid URL";
            }
            catch (TaskCanceledException)
            {
                content.UpdateContent("The connection has timed out.", true);
                tabTitle = "Timeout";
                browserTitle = content.renderedContent.Text;
            }
            catch (HttpRequestException)
            {
                content.UpdateContent("Something went wrong.\r\nCheck if you entered the right address\r\nCheck your network connection\r\nTry again later", true);
                tabTitle = "Error";
                browserTitle = content.renderedContent.Text;
            }

            if (!isHistory && tabHistory.GetCurrentUrl() != url)
            {
                tabHistory.Visit(url);
                settingForm.HistoryUI.globalHistory.AddHistory(url);
            }

            UpdateTabTitle();
            UpdateBrowserTitle();
            content.utilBar.UpdateBookmarkButton();
            content.utilBar.UpdateBackForwardButtons();
            content.utilBar.UpdateReloadButton();
        }

        public string GoBack()
        {
            if (!tabHistory.CanGoBack())
            {
                return "";
            }

            string? url = tabHistory.PrevUrl();

            if (url is not null)
            {
                Console.WriteLine("test back " + url);
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? "";
        }

        public string GoForward()
        {
            if (!tabHistory.CanGoForward())
            {
                return "";
            }

            string? url = tabHistory.NextUrl();

            if (url is not null)
            {
                Console.WriteLine("test forward " + url);
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
            Regex titleRegex = new("<title*>\\s*(.*)\\s*</title>", RegexOptions.IgnoreCase);
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
