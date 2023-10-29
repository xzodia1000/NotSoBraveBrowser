using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    /**
     * TabContent is a FlowLayoutPanel that contains the rendered content of a tab.
     * It also contains a UtilBar that allows the user to interact with the tab.
     */
    public class TabContent : FlowLayoutPanel
    {
        public UtilBar utilBar; // Utilbar is the address bar and the buttons
        public TextBox renderedContent; // renderedContent is the content of the tab

        /**
         * TabContent constructor initializes the utilBar and the renderedContent.
         * It takes a Tab object and a SettingForm object as parameters.
         */
        public TabContent(Tab tab, SettingForm settingForm)
        {
            utilBar = new UtilBar(tab, settingForm);
            renderedContent = new TextBox();
            InitTabContent();
            InitTextBox();
        }

        /**
         * InitTabContent initializes the TabContent FlowLayoutPanel.
         * It sets the properties of the FlowLayoutPanel.
         */
        private void InitTabContent()
        {
            BackColor = Color.White;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;

            Controls.Add(utilBar);
            Controls.Add(renderedContent);
        }

        /**
         * InitTextBox initializes the renderedContent TextBox.
         * It sets the properties of the TextBox.
         */
        private void InitTextBox()
        {
            renderedContent.Multiline = true;
            renderedContent.ScrollBars = ScrollBars.Vertical; // Only vertical scroll bar
            renderedContent.ReadOnly = true;
            renderedContent.Cursor = Cursors.Default;
            renderedContent.Margin = new Padding(5);
        }

        /**
         * UpdateContent updates the content of the renderedContent TextBox.
         * It takes a string and a boolean as parameters.
         * The string is the content to be displayed.
         * The boolean is whether the content should be centered or not.
         */
        public void UpdateContent(string content, bool center = false)
        {
            renderedContent.Clear(); // Clear the previous content
            renderedContent.TextAlign = HorizontalAlignment.Left;
            renderedContent.Font = new Font("Roboto", 10F);
            renderedContent.Padding = new Padding(0);

            if (center)
            {
                // Center the content
                renderedContent.Name = content;
                renderedContent.Font = new Font("Roboto", 12F, FontStyle.Bold);

                int height = renderedContent.Height / renderedContent.Font.Height / 2; // Get the number of lines to center the content

                renderedContent.TextAlign = HorizontalAlignment.Center;

                // Add new lines to center the content
                renderedContent.Text = string.Concat(Enumerable.Repeat("\r\n", height - 2)) + content;
            }
            else
            {
                renderedContent.Text = content; // Add the content
            }
        }

        /**
         * UpdateSize updates the size of the TabContent FlowLayoutPanel.
         * It takes a FlowLayoutPanel as a parameter.
         * The FlowLayoutPanel is the canvas that contains the TabContent.
         */
        public void UpdateSize(FlowLayoutPanel canvas)
        {
            // Update the size of the TabContent FlowLayoutPanel
            Width = canvas.Width;
            Height = canvas.Height - canvas.Controls[0].Height;
            renderedContent.Width = Width - 15;
            renderedContent.Height = Height - utilBar.Height - 25;

            if (renderedContent.TextAlign.Equals(HorizontalAlignment.Center))
            {
                // Center the content with the new size
                int height = renderedContent.Height / renderedContent.Font.Height / 2;

                renderedContent.TextAlign = HorizontalAlignment.Center;
                renderedContent.Text = string.Concat(Enumerable.Repeat("\r\n", height - 2)) + renderedContent.Name;
            }

            // Update the size of the UtilBar
            utilBar.UpdateSize(canvas);
        }
    }
}
