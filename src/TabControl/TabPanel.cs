using System.Runtime.InteropServices;

namespace NotSoBraveBrowser.src.TabControl
{
    public partial class TabPanel : FlowLayoutPanel
    {
        private Button addTabButton;
        private Tab? selectedTab;
        public readonly FlowLayoutPanel canvas;

        public TabPanel(FlowLayoutPanel canvas)
        {
            addTabButton = new Button();
            selectedTab = null;
            this.canvas = canvas;

            InitTabPanel();
            InitAddTabButton();
        }


        public void InitTabPanel()
        {
            Height = 30;
            Width = canvas.Width;
            BackColor = Color.White;
            FlowDirection = FlowDirection.LeftToRight;
            WrapContents = false;
            AutoScroll = true;
            canvas.Controls.Add(this);
        }

        private void InitAddTabButton()
        {
            addTabButton.Name = "addTabButton";
            addTabButton.Text = "+";
            addTabButton.Size = new Size(28, 28);
            addTabButton.TextAlign = ContentAlignment.MiddleCenter;
            addTabButton.Margin = new Padding(1);
            addTabButton.Click += AddTabButton_Click;
            Controls.Add(addTabButton);
        }

        private void AddTabButton_Click(object sender, EventArgs e)
        {
            AddTab("New Tab");
        }

        public void AddTab(string title)
        {
            _ = new Tab(title, this);
            Controls.SetChildIndex(addTabButton, -1);
        }

        public void CloseTab(Tab tab)
        {
            Controls.Remove(tab);
        }

        public void SetActiveTab(Tab tab)
        {
            if (selectedTab != null) selectedTab.BackColor = Color.White;
            tab.BackColor = Color.LightGray;
            selectedTab = tab;

            if (canvas.Controls.Count == 2) canvas.Controls.Remove(canvas.Controls[1]);
            canvas.Controls.Add(tab.content);
            UpdatePanelWidth();
        }

        public void UpdatePanelWidth()
        {
            Width = canvas.Width;
            if (selectedTab != null) selectedTab!.content.UpdateSize(canvas);
        }

        [LibraryImport("user32.dll")]
        private static partial int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (AutoScroll)
            {
                _ = ShowScrollBar(Handle, 0, 0); // Hide horizontal
                _ = ShowScrollBar(Handle, 1, 0); // Hide vertical
            }
        }
    }
}
