using System.Runtime.InteropServices;

namespace NotSoBraveBrowser.src.TabControl
{
    public partial class TabPanel : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel canvas;
        private Button addTabButton;
        private Tab? selectedTab;


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

        public TabPanel(FlowLayoutPanel canvas)
        {
            this.canvas = canvas;

            addTabButton = new Button();
            selectedTab = null;

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
            SetActiveTab(new Tab(title, this));
            Controls.SetChildIndex(addTabButton, -1);
        }

        public void CloseTab(Tab tab)
        {
            int index = Controls.IndexOf(tab);

            if (Controls[index + 1] is Tab)
            {
                SetActiveTab((Tab)Controls[index + 1]);
            }
            else
            {
                try
                {
                    SetActiveTab((Tab)Controls[index - 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    SetActiveTab(null);
                }
            }

            Controls.Remove(tab);
        }

        public void SetActiveTab(Tab? tab)
        {
            if (selectedTab is not null) selectedTab.BackColor = Color.White;
            if (tab is not null) tab.BackColor = Color.LightGray;

            selectedTab = tab;

            if (canvas.Controls.Count == 2) canvas.Controls.Remove(canvas.Controls[1]);
            if (tab is not null) canvas.Controls.Add(tab.content);

            if (tab != null)
            {
                canvas.Controls.Add(tab.content);
            }
            else
            {
                canvas.Controls.Add(new Label() { Text = "No tabs open", AutoSize = true, Margin = new Padding(10) });
            }

            selectedTab?.UpdateBrowserTitle();

            UpdatePanelWidth();
        }

        public void UpdatePanelWidth()
        {
            Width = canvas.Width;
            if (selectedTab != null) selectedTab!.content.UpdateSize(canvas);
        }

    }
}
