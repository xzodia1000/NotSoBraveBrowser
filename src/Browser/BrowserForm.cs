using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.Home;
using NotSoBraveBrowser.src.TabControl;

namespace NotSoBraveBrowser.src.Browser
{
    public partial class BrowserForm : Form
    {
        public FlowLayoutPanel canvas;
        private readonly TabPanel tabPanel;
        private readonly HistoryUI historyUI;
        private readonly DownloadUI downloadUI;
        private readonly BookmarkUI bookmarkUI;
        private readonly HomeUI homeUI;
        private readonly ShortcutUI shortcutUI;
        private readonly SettingForm settingForm;

        public BrowserForm()
        {
            InitializeComponent();
            canvas = new FlowLayoutPanel();
            historyUI = new HistoryUI(this);
            downloadUI = new DownloadUI(this);
            bookmarkUI = new BookmarkUI(this);
            homeUI = new HomeUI(this);
            shortcutUI = new ShortcutUI(this);
            settingForm = new SettingForm(historyUI, downloadUI, bookmarkUI, homeUI);
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
            NewTab("New Tab", homeUI.homeManager.GetHome());
        }

        private void Browser_Resize(object sender, EventArgs e)
        {
            tabPanel.UpdatePanelWidth();
        }

        private void Browser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4) Close();
            else if (e.Control && e.Shift && e.KeyCode == Keys.N) new BrowserForm().Show();
            else if (e.Control && e.KeyCode == Keys.T) NewTab("New Tab");
            else if (e.Control && e.KeyCode == Keys.W && tabPanel.selectedTab != null) tabPanel.CloseTab(tabPanel.selectedTab);
            else if (e.Control && e.Shift && e.KeyCode == Keys.Tab) tabPanel.PrevTab();
            else if (e.Control && e.KeyCode == Keys.Tab) tabPanel.NextTab();
            else if (e.Control && e.KeyCode == Keys.R) tabPanel.selectedTab?.Reload();
            else if (e.Alt && e.KeyCode == Keys.Left) tabPanel.selectedTab?.GoBack();
            else if (e.Alt && e.KeyCode == Keys.Right) tabPanel.selectedTab?.GoForward();
            else if (e.Control && e.Shift && e.KeyCode == Keys.H) NewTab("New Tab", homeUI.homeManager.GetHome());
            else if (e.Control && e.KeyCode == Keys.H) historyUI.OpenHistory();
            else if (e.Control && e.KeyCode == Keys.J) downloadUI.OpenDownload();
            else if (e.Control && e.KeyCode == Keys.B) bookmarkUI.OpenBookmark();
            else if (e.Control && e.KeyCode == Keys.P) homeUI.OpenHome();
            else if (e.Control && e.KeyCode == Keys.OemQuestion) shortcutUI.OpenShortcut();
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

            ToolStripMenuItem fileMenu = new("File");
            fileMenu.DropDownItems.Add("New Window");
            fileMenu.DropDownItems.Add("New Tab");
            fileMenu.DropDownItems.Add("Close Window");
            fileMenu.DropDownItems[0].Click += (s, e) => new BrowserForm().Show();
            fileMenu.DropDownItems[1].Click += (s, e) => NewTab("New Tab");
            fileMenu.DropDownItems[2].Click += (s, e) => Close();
            menuStrip.Items.Add(fileMenu);

            ToolStripMenuItem editMenu = new("Edit");
            editMenu.DropDownItems.Add("Change Homepage");
            editMenu.DropDownItems[0].Click += (s, e) => homeUI.OpenHome();
            menuStrip.Items.Add(editMenu);

            ToolStripMenuItem viewMenu = new("View");
            viewMenu.DropDownItems.Add("History");
            viewMenu.DropDownItems.Add("Downloads");
            viewMenu.DropDownItems.Add("Bookmarks");
            viewMenu.DropDownItems[0].Click += (s, e) => historyUI.OpenHistory();
            viewMenu.DropDownItems[1].Click += (s, e) => downloadUI.OpenDownload();
            viewMenu.DropDownItems[2].Click += (s, e) => bookmarkUI.OpenBookmark();
            menuStrip.Items.Add(viewMenu);

            ToolStripMenuItem helpMenu = new("Help");
            helpMenu.DropDownItems.Add("Show Keyboard Shortcuts");
            helpMenu.DropDownItems[0].Click += (s, e) => shortcutUI.OpenShortcut();
            menuStrip.Items.Add(helpMenu);

            // Add the MenuStrip to the form
            Controls.Add(menuStrip);
        }
    }

}