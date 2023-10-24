using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.TabControl;

namespace NotSoBraveBrowser.src.Browser
{
    public partial class BrowserForm : Form
    {
        public FlowLayoutPanel canvas;
        private readonly TabPanel tabPanel;

        private SettingForm settingForm;

        public BrowserForm()
        {
            InitializeComponent();
            canvas = new FlowLayoutPanel();
            settingForm = new SettingForm(new HistoryUI(this));
            tabPanel = new TabPanel(canvas, settingForm);
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            canvas.Dock = DockStyle.Fill;
            canvas.BackColor = Color.White;
            canvas.FlowDirection = FlowDirection.TopDown;
            canvas.WrapContents = false;
            canvas.AutoScroll = false;
            Controls.Add(canvas);

            tabPanel.AddTab("Home");
            tabPanel.UpdatePanelWidth();
        }

        public void NewTab(string tabTitle, string url)
        {
            Tab newTab = tabPanel.AddTab(tabTitle);
            newTab.RenderCode(url);
        }

        private void Browser_Resize(object sender, EventArgs e)
        {
            tabPanel.UpdatePanelWidth();
        }

    }
}