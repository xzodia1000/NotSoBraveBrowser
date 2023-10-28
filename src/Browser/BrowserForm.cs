using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.TabControl;

namespace NotSoBraveBrowser.src.Browser
{
    public partial class BrowserForm : Form
    {
        public FlowLayoutPanel canvas;
        private readonly TabPanel tabPanel;
        private readonly HistoryUI historyUI;
        private readonly DownloadUI downloadUI;
        private readonly SettingForm settingForm;

        public BrowserForm()
        {
            InitializeComponent();
            canvas = new FlowLayoutPanel();
            historyUI = new HistoryUI(this);
            downloadUI = new DownloadUI(this);
            settingForm = new SettingForm(historyUI, downloadUI);
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

            tabPanel.UpdatePanelWidth();
            CreateMenu();
            NewTab("New Tab", "http://status.savanttools.com/?code=401%20Unauthorized");
        }

        private void Browser_Resize(object sender, EventArgs e)
        {
            tabPanel.UpdatePanelWidth();
        }

        public void NewTab(string tabTitle, string url = "")
        {
            Tab newTab = tabPanel.AddTab(tabTitle);
            if (url != "") newTab.RenderCode(url);
        }


        private void CreateMenu()
        {
            // Create a new MenuStrip
            MenuStrip menuStrip = new();

            // Create top-level menu items
            ToolStripMenuItem fileMenu = new("File");

            // Add sub-items to the File menu
            fileMenu.DropDownItems.Add("New Window");
            fileMenu.DropDownItems.Add("New Tab");
            fileMenu.DropDownItems.Add("Close");

            // Add click event for a menu item
            fileMenu.DropDownItems[0].Click += (s, e) => new BrowserForm().Show();
            fileMenu.DropDownItems[1].Click += (s, e) => NewTab("New Tab");
            fileMenu.DropDownItems[2].Click += (s, e) => Close();

            // Add top-level items to the MenuStrip
            menuStrip.Items.Add(fileMenu);

            // Add the MenuStrip to the form
            Controls.Add(menuStrip);
        }
    }

}