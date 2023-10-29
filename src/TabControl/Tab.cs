using System.Text.RegularExpressions;
using System.Xml;
using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.HttpRequests;

namespace NotSoBraveBrowser.src.TabControl

{
    /**
     * Tab is a class that stores the tab data that is used in the tab panel.
     */
    public class Tab : Button
    {
        private string tabTitle; // The title of the tab
        private readonly TabPanel panel; // The tab panel
        private readonly SettingForm settingForm; // The setting form for the util bar
        private string browserTitle; // The title of the browser
        private readonly Requests client; // The HTTP client
        public TabContent content; // The content of the tab
        private readonly Button closeButton; // The close button of the tab
        public readonly TabHistory tabHistory; // The history of the tab

        /**
         * Tab is the constructor of the Tab class.
         * It initializes the title of the tab, the tab panel, the setting form,
         * the HTTP client, the content of the tab, the close button of the tab,
         * and the history of the tab.
         */
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

        /**
         * InitTab is a method that sets the properties of the tab.
         */
        private void InitTab()
        {
            Name = tabTitle; // The name of the tab
            Text = GetDisplayText(); // The title of the tab
            Size = new Size(100, 28); // The size of the tab
            Margin = new Padding(1);
            TextAlign = ContentAlignment.MiddleLeft;

            MouseHover += (sender, e) => Cursor = Cursors.Hand; // Change the cursor on hover
            Click += Tab_Click;
            panel.Controls.Add(this); // Add the tab to the tab panel
        }

        /**
         * InitCloseButton is a method that sets the properties of the close button.
         */
        private void InitCloseButton()
        {
            closeButton.Name = "closeButton";
            closeButton.Image = ImageUtil.ResizeImage(IconImage.closeIcon, 18, 18); // The image of the close button
            closeButton.Size = new Size(20, 20);
            closeButton.Left = Width - closeButton.Width - 4;
            closeButton.Top = (Height - closeButton.Height) / 2;
            closeButton.ImageAlign = ContentAlignment.MiddleCenter;

            closeButton.FlatStyle = FlatStyle.Flat; // The style of the close button
            closeButton.BackColor = Color.Transparent; // The background color of the close button
            closeButton.FlatAppearance.BorderSize = 0; // The border size of the close button
            closeButton.FlatAppearance.MouseOverBackColor = closeButton.BackColor; // The background color of the close button on hover
            closeButton.FlatAppearance.MouseDownBackColor = closeButton.BackColor; // The background color of the close button on click

            // Change the cursor and the image of the close button on hover
            closeButton.MouseHover += (sender, e) =>
            {
                closeButton.BackColor = Color.Transparent;
                closeButton.Cursor = Cursors.Hand;
                closeButton.Image = ImageUtil.ResizeImage(IconImage.closeRedIcon, 18, 18);
            };

            // Change the image back to the original image on leave
            closeButton.MouseLeave += (sender, e) =>
            {
                closeButton.Image = ImageUtil.ResizeImage(IconImage.closeIcon, 18, 18);
            };

            closeButton.Click += CloseButton_Click;
            Controls.Add(closeButton);
        }

        /**
         * Tab_Click is a method that sets the active tab to the current tab.
         */
        private void Tab_Click(object? sender, EventArgs e)
        {
            panel.SetActiveTab(this);
        }

        /**
         * CloseButton_Click is a method that closes the current tab.
         */
        private void CloseButton_Click(object sender, EventArgs e)
        {
            panel.CloseTab(this);
        }

        /**
         * RenderCode is a method that renders the HTML code of the URL.
         * It takes the URL and a boolean value as parameters.
         * The boolean value is used to check if the URL is from the history.
         */
        public async void RenderCode(string url, bool isHistory = false)
        {

            if (UrlUtils.IsEmptyUrl(url))
            {
                // If the URL is empty, return
                return;
            }
            url = UrlUtils.AddHttp(url); // Add the HTTP protocol to the URL

            content.utilBar.urlTextBox.Text = url; // Set the URL text box to the edited URL
            content.UpdateContent("Loading...", true); // Set the content to loading

            try
            {
                // Get the response from the URL.
                // Task.Run is used to run the code in a separate thread.
                HttpResponseMessage response = await Task.Run(() => client.Get(url));

                if (response.IsSuccessStatusCode)
                {
                    // If the response is successful, update the content with the HTML code.
                    string html = await response.Content.ReadAsStringAsync();
                    content.UpdateContent(html);

                    // Set the title of the tab and browser from the HTML code or set it to "200 OK" if the title is not found.
                    tabTitle = !string.IsNullOrEmpty(GetTitle(html)) ? GetTitle(html) : "200 OK";
                    browserTitle = !string.IsNullOrEmpty(GetTitle(html)) ? "200 OK | " + GetTitle(html) : "200 OK";
                }
                else
                {
                    // If the response is not successful, update the content with the status code and the reason phrase.
                    content.UpdateContent($"{(int)response.StatusCode} {response.ReasonPhrase}", true);

                    // Set the title of the tab and browser to the status code.
                    tabTitle = $"{response.StatusCode}";
                    browserTitle = content.renderedContent.Name;
                }
            }
            catch (UriFormatException)
            {
                // If the URL is invalid, update the content with the error message.
                content.UpdateContent("Invalid URL", true);
                tabTitle = "Invalid URL";
                browserTitle = "Invalid URL";
            }
            catch (TaskCanceledException)
            {
                // If the connection has timed out, update the content with the error message.
                content.UpdateContent("The connection has timed out.", true);
                tabTitle = "Timeout";
                browserTitle = content.renderedContent.Text;
            }
            catch (HttpRequestException)
            {
                // If the connection has failed, update the content with the error message.
                content.UpdateContent("Something went wrong.\r\nCheck if you entered the right address\r\nCheck your network connection\r\nTry again later", true);
                tabTitle = "Error";
                browserTitle = content.renderedContent.Text;
            }

            if (!isHistory && tabHistory.GetCurrentUrl() != url)
            {
                // If the URL is not from the history and the current URL is not the same as the URL, add the URL to the history.
                tabHistory.Visit(url);
                settingForm.HistoryUI.globalHistory.AddHistory(url);
            }

            UpdateTabTitle(); // Update the title of the tab
            UpdateBrowserTitle(); // Update the title of the browser
            content.utilBar.UpdateBookmarkButton(); // Update the bookmark button
            content.utilBar.UpdateBackForwardButtons(); // Update the back and forward buttons
            content.utilBar.UpdateReloadButton(); // Update the reload button
        }

        /**
         * GoBack is a method that goes back to the previous URL.
         * It returns the new current URL.
         */
        public string GoBack()
        {
            if (!tabHistory.CanGoBack())
            {
                // If the tab cannot go back, return an empty string.
                return "";
            }

            string? url = tabHistory.PrevUrl();

            if (!string.IsNullOrEmpty(url))
            {
                // If the URL is not null, render the code of the URL.
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? ""; // Return the current URL or an empty string if the current URL is null.
        }

        /**
         * GoForward is a method that goes forward to the next URL.
         * It returns the new current URL.
         */
        public string GoForward()
        {
            if (!tabHistory.CanGoForward())
            {
                // If the tab cannot go forward, return an empty string.
                return "";
            }

            string? url = tabHistory.NextUrl();

            if (!string.IsNullOrEmpty(url))
            {
                // If the URL is not null, render the code of the URL.
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? "";  // Return the current URL or an empty string if the current URL is null.
        }

        /**
         * Reload is a method that reloads the current URL.
         * It returns the new current URL.
         */
        public string Reload()
        {
            string? url = tabHistory.GetCurrentUrl();

            if (!string.IsNullOrEmpty(url))
            {
                // If the URL is not null, render the code of the URL.
                RenderCode(url, true);
            }

            return tabHistory.GetCurrentUrl() ?? ""; // Return the current URL or an empty string if the current URL is null.
        }

        /**
         * GetTitle is a method that gets the title of the HTML code.
         * It takes the HTML code as a parameter.
         */
        private static string GetTitle(string html)
        {
            // Get the title from the HTML code using a regular expression.
            // The regular expression looks for the <title> tag and gets the text inside the tag.
            Regex titleRegex = new("<title*>\\s*(.*)\\s*</title>", RegexOptions.IgnoreCase);
            Match match = titleRegex.Match(html);

            if (match.Success)
            {
                // If the title is found, return the title.
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }

        /**
         * GetDisplayText is a method that gets the display text of the tab
         * based on the width of the tab with ellipsis if the title is too long.
         * It returns the display text of the tab.
         */
        private string GetDisplayText()
        {
            using (Graphics g = CreateGraphics()) // Create a graphics object
            {
                SizeF size = g.MeasureString(tabTitle, Font); // Get the size of the tab title with the current font
                if (size.Width > Width - 27 && tabTitle.Length != 7)
                {
                    // If the width of the tab title is greater than the width of the tab, add ellipsis and trim the title.
                    return string.Concat(tabTitle.AsSpan(0, 7), "...");
                }
            }

            return tabTitle; // Return the edited title
        }

        /**
         * UpdateTabTitle is a method that updates the title of the tab.
         */
        private void UpdateTabTitle()
        {
            Name = tabTitle;
            Text = GetDisplayText();
        }

        /**
         * UpdateBrowserTitle is a method that updates the title of the browser.
         */
        public void UpdateBrowserTitle()
        {
            panel.Parent!.Parent!.Text = "NotSoBraveBrowser - " + browserTitle;
        }
    }
}
