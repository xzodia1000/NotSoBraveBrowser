using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    public class TabContent : FlowLayoutPanel
    {
        public UtilBar utilBar;
        public TextBox renderedContent;

        public TabContent(Tab tab, SettingForm settingForm)
        {
            utilBar = new UtilBar(tab, settingForm);
            renderedContent = new TextBox();
            InitTabContent();
            InitTextBox();
        }

        private void InitTabContent()
        {
            BackColor = Color.White;
            FlowDirection = FlowDirection.TopDown;
            WrapContents = false;

            Controls.Add(utilBar);
            Controls.Add(renderedContent);
        }

        private void InitTextBox()
        {
            renderedContent.Multiline = true;
            renderedContent.ScrollBars = ScrollBars.Vertical;
            renderedContent.ReadOnly = true;
            renderedContent.Cursor = Cursors.Default;
            renderedContent.Margin = new Padding(5);
        }

        public void UpdateContent(string content, bool center = false)
        {
            renderedContent.Clear();
            renderedContent.TextAlign = HorizontalAlignment.Left;
            renderedContent.Font = new Font("Roboto", 10F);
            renderedContent.Padding = new Padding(0);

            if (center)
            {
                renderedContent.Name = content;
                renderedContent.Font = new Font("Roboto", 12F, FontStyle.Bold);

                int height = renderedContent.Height / renderedContent.Font.Height / 2;

                renderedContent.TextAlign = HorizontalAlignment.Center;
                renderedContent.Text = string.Concat(Enumerable.Repeat("\r\n", height - 2)) + content;
            }
            else
            {
                renderedContent.Text = content;
            }
        }

        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width;
            Height = canvas.Height - canvas.Controls[0].Height;
            renderedContent.Width = Width - 15;
            renderedContent.Height = Height - utilBar.Height - 25;

            if (renderedContent.TextAlign.Equals(HorizontalAlignment.Center))
            {
                int height = renderedContent.Height / renderedContent.Font.Height / 2;

                renderedContent.TextAlign = HorizontalAlignment.Center;
                renderedContent.Text = string.Concat(Enumerable.Repeat("\r\n", height - 2)) + renderedContent.Name;
            }

            utilBar.UpdateSize(canvas);
        }
    }
}
