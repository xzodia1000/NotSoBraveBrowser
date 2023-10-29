using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Bookmark;
using NotSoBraveBrowser.src.Download;
using NotSoBraveBrowser.src.History;
using NotSoBraveBrowser.src.Home;
using NotSoBraveBrowser.src.TabControl;

namespace NotSoBraveBrowser.src.Browser
{
    /**
     * BrowserForm is the main form of the browser.
     * It contains the canvas, the tabPanel, and the settingForm.
     */
    public partial class BrowserForm : Form
    {
        public FlowLayoutPanel canvas; // Main canvas of the browser
        private readonly TabPanel tabPanel; // TabPanel object that contains the tabs
        private readonly HistoryUI historyUI; // HistoryUI object that contains the history
        private readonly DownloadUI downloadUI; // DownloadUI object that contains the downloads
        private readonly BookmarkUI bookmarkUI; // BookmarkUI object that contains the bookmarks
        private readonly HomeUI homeUI; // HomeUI object that contains the home
        private readonly ShortcutUI shortcutUI; // ShortcutUI object that contains the shortcuts
        private readonly SettingForm settingForm; // SettingForm object that contains the settings

        /**
         * BrowserForm constructor initializes the canvas, the tabPanel, and the settingForm.
         */
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

        /**
         * Browser_Load is a function that is called when the form is loaded.
         * It takes an object and an EventArgs as parameters.
         * The object is the sender of the event.
         * The EventArgs is the event arguments.
         */
        private void Browser_Load(object sender, EventArgs e)
        {
            // Set the canvas properties
            canvas.Dock = DockStyle.Fill;
            canvas.BackColor = Color.White;
            canvas.FlowDirection = FlowDirection.TopDown;
            canvas.WrapContents = false;
            canvas.AutoScroll = false;
            Controls.Add(canvas);

            tabPanel.UpdatePanelWidth(); // Update the tab panel width
            CreateMenu(); // Create the top menu
            NewTab("New Tab", homeUI.homeManager.GetHome()); // Create a new tab with the home page
        }

        /**
         * Browser_Resize is a function that is called when the form is resized.
         * It takes an object and an EventArgs as parameters.
         * The object is the sender of the event.
         * The EventArgs is the event arguments.
         */
        private void Browser_Resize(object sender, EventArgs e)
        {
            tabPanel?.UpdatePanelWidth(); // Update the tab panel width
        }

        /**
         * Browser_KeyDown is a function that is called when a key is pressed.
         * It takes an object and a KeyEventArgs as parameters.
         * The object is the sender of the event.
         * The KeyEventArgs is the event arguments.
         */
        private async void Browser_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key pressed is a shortcut and execute the shortcut
            if (e.Alt && e.KeyCode == Keys.F4) Close();
            else if (e.Control && e.Shift && e.KeyCode == Keys.N) await NewWindowAsync();
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

        /**
         * NewWindowAsync is a function that creates a new window.
         * It takes no parameters.
         */
        private static async Task NewWindowAsync()
        {
            BrowserForm newBrowserForm = new();
            newBrowserForm.Show();
            newBrowserForm.Enabled = false;
            await Task.Delay(1500); // Wait for 1.5 seconds to prevent the new window from being closed immediately
            newBrowserForm.Enabled = true;
        }

        /**
         * NewTab is a function that creates a new tab.
         * It takes a string and a string as parameters.
         * The first string is the title of the tab.
         * The second string is the url of the tab.
         */
        public void NewTab(string tabTitle, string url = "")
        {
            Tab newTab = tabPanel.AddTab(tabTitle); // Add a new tab to the tab panel
            newTab.RenderCode(url); // Render the url
        }

        /**
         * CreateMenu is a function that creates the top menu.
         */
        private void CreateMenu()
        {
            // Create a new MenuStrip
            MenuStrip menuStrip = new();

            // Create File option
            ToolStripMenuItem fileMenu = new("File");
            fileMenu.DropDownItems.Add("New Window"); // Add New Window option
            fileMenu.DropDownItems.Add("New Tab"); // Add New Tab option
            fileMenu.DropDownItems.Add("Close Window"); // Add Close Window option
            fileMenu.DropDownItems[0].Click += async (s, e) => await NewWindowAsync(); // Add event handler for New Window option
            fileMenu.DropDownItems[1].Click += (s, e) => NewTab("New Tab"); // Add event handler for New Tab option
            fileMenu.DropDownItems[2].Click += (s, e) => Close(); // Add event handler for Close Window option
            menuStrip.Items.Add(fileMenu); // Add File option to the MenuStrip

            // Create Edit option
            ToolStripMenuItem editMenu = new("Edit");
            editMenu.DropDownItems.Add("Change Homepage"); // Add Change Homepage option
            editMenu.DropDownItems[0].Click += (s, e) => homeUI.OpenHome(); // Add event handler for Change Homepage option
            menuStrip.Items.Add(editMenu); // Add Edit option to the MenuStrip

            // Create View option
            ToolStripMenuItem viewMenu = new("View");
            viewMenu.DropDownItems.Add("History"); // Add History option
            viewMenu.DropDownItems.Add("Downloads"); // Add Downloads option
            viewMenu.DropDownItems.Add("Bookmarks"); // Add Bookmarks option 
            viewMenu.DropDownItems[0].Click += (s, e) => historyUI.OpenHistory(); // Add event handler for History option
            viewMenu.DropDownItems[1].Click += (s, e) => downloadUI.OpenDownload(); // Add event handler for Downloads option 
            viewMenu.DropDownItems[2].Click += (s, e) => bookmarkUI.OpenBookmark(); // Add event handler for Bookmarks option
            menuStrip.Items.Add(viewMenu);

            // Create Settings option
            ToolStripMenuItem helpMenu = new("Help");
            helpMenu.DropDownItems.Add("Show Keyboard Shortcuts"); // Add Show Keyboard Shortcuts option
            helpMenu.DropDownItems[0].Click += (s, e) => shortcutUI.OpenShortcut(); // Add event handler to Show Keyboard Shortcuts
            menuStrip.Items.Add(helpMenu); // Add Settings option to the MenuStrip

            // Add the MenuStrip to the form
            Controls.Add(menuStrip);
        }
    }

}