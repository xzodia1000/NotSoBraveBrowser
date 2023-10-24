using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    public class TabContent : FlowLayoutPanel
    {
        public UtilBar utilBar;
        public RichTextBox renderedContent;

        public TabContent(Tab tab, SettingForm settingForm)
        {
            utilBar = new UtilBar(tab, settingForm);
            renderedContent = new RichTextBox();
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
            renderedContent.ScrollBars = RichTextBoxScrollBars.Both;
            renderedContent.ReadOnly = true;
            renderedContent.Margin = new Padding(5);
        }

        public void UpdateSize(FlowLayoutPanel canvas)
        {
            Width = canvas.Width;
            Height = canvas.Height - canvas.Controls[0].Height;
            renderedContent.Width = Width - 15;
            renderedContent.Height = Height - utilBar.Height - 25;
            utilBar.UpdateSize(canvas);
        }
    }
}
