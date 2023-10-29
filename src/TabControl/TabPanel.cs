using System.Runtime.InteropServices;
using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.TabControl
{
    /**
     * TabPanel is a FlowLayoutPanel that contains the tabs.
     * It also contains the addTabButton.
     */
    public partial class TabPanel : FlowLayoutPanel
    {
        private readonly FlowLayoutPanel canvas; // Main canvas of the browser
        public readonly SettingForm settingForm; // SettingForm object that contains the settings
        public Tab? selectedTab; // The currently selected tab
        private Button addTabButton; // Button to add a new tab

        /**
         * TabPanel constructor initializes the canvas and the settingForm.
         * It also initializes the addTabButton.
         */
        public TabPanel(FlowLayoutPanel canvas, SettingForm settingForm)
        {
            this.canvas = canvas;
            this.settingForm = settingForm;
            selectedTab = null;
            addTabButton = new Button();


            InitTabPanel();
            InitAddTabButton();
        }


        /**
         * ShowScrollBar is a function imported from user32.dll.
         * It takes a handle, a bar, and a boolean as parameters.
         * The handle is the handle of the window.
         * The bar is the bar to be shown or hidden.
         * The boolean is whether the bar should be shown or hidden.
         */
        [LibraryImport("user32.dll")]
        private static partial int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        /**
         * WndProc is a function that overrides the default window procedure.
         * It takes a Message object as a parameter.
         * The Message object is the message to be processed.
         */
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (AutoScroll)
            {
                // Hide scrollbars
                _ = ShowScrollBar(Handle, 0, 0); // Hide horizontal
                _ = ShowScrollBar(Handle, 1, 0); // Hide vertical
            }
        }

        /**
         * InitTabPanel initializes the TabPanel FlowLayoutPanel.
         * It sets the properties of the FlowLayoutPanel.
         */
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

        /**
         * InitAddTabButton initializes the addTabButton.
         * It sets the properties of the button.
         */
        private void InitAddTabButton()
        {
            addTabButton.Name = "addTabButton";
            addTabButton.Image = ImageUtil.ResizeImage(IconImage.addIcon, 18, 18); // Sets the add icon
            addTabButton.Size = new Size(28, 28);
            addTabButton.Margin = new Padding(1);
            addTabButton.ImageAlign = ContentAlignment.MiddleCenter;

            addTabButton.MouseHover += (sender, e) => addTabButton.Cursor = Cursors.Hand;
            addTabButton.Click += AddTabButton_Click;
            Controls.Add(addTabButton);
        }

        /**
         * AddTabButton_Click is the event handler for the addTabButton click event.
         * It takes an object and an EventArgs as parameters.
         * The object is the object that triggered the event.
         * The EventArgs is the event arguments.
         */
        private void AddTabButton_Click(object? sender, EventArgs e)
        {
            AddTab("New Tab"); // Add a new tab
        }

        /**
         * AddTab adds a new tab to the TabPanel.
         * It takes a string as a parameter.
         * The string is the title of the tab.
         */
        public Tab AddTab(string title)
        {
            Tab newTab = new(title, this, settingForm); // Create a new tab
            SetActiveTab(newTab); // Set the new tab as the active tab
            Controls.SetChildIndex(addTabButton, -1); // Move the addTabButton to the end
            return newTab; // Return the new tab
        }

        /**
         * CloseTab closes a tab.
         * It takes a Tab object as a parameter.
         * The Tab object is the tab to be closed.
         */
        public void CloseTab(Tab? tab)
        {
            int index = Controls.IndexOf(tab); // Get the index of the tab

            if (selectedTab != tab) { } // Do not move the selected tab if it is not the tab to be closed
            else if (Controls[index + 1] is Tab nextTab) SetActiveTab(nextTab); // Move the selected tab to the next tab if it exists
            else
            {
                // Move the selected tab to the previous tab if it exists
                try
                {
                    SetActiveTab((Tab)Controls[index - 1]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    SetActiveTab(null);
                }
            }

            Controls.Remove(tab); // Remove the tab
        }

        /**
         * SetActiveTab sets the active tab.
         * It takes a Tab object as a parameter.
         * The Tab object is the tab to be set as the active tab.
         */
        public void SetActiveTab(Tab? tab)
        {
            if (selectedTab is not null) selectedTab.BackColor = Color.White; // Set the background color of the previous tab to white
            if (tab is not null) tab.BackColor = Color.WhiteSmoke; // Set the background color of the new tab to white smoke

            selectedTab = tab; // Set the new tab as the selected tab

            if (canvas.Controls.Count == 2) canvas.Controls.Remove(canvas.Controls[1]); // Remove the previous tab content

            if (tab != null) canvas.Controls.Add(tab.content); // Add the new tab content
            else
            {
                // Add a label if there are no tabs open
                canvas.Controls.Add(new Label() { Text = "No tabs open", AutoSize = true, Margin = new Padding(10) });
            }

            selectedTab?.UpdateBrowserTitle(); // Update the browser title

            UpdatePanelWidth(); // Update the width of the panel
        }

        /**
         * NextTab selects the next tab.
         */
        public void NextTab()
        {
            if (selectedTab is null) return; // Do nothing if there are no tabs
            int index = Controls.IndexOf(selectedTab); // Get the index of the selected tab
            if (index == Controls.Count - 2) SetActiveTab((Tab)Controls[0]); // Set the first tab as the selected tab if the selected tab is the last tab
            else SetActiveTab((Tab)Controls[index + 1]); // Set the next tab as the selected tab
        }

        /**
         * PrevTab selects the previous tab.
         */
        public void PrevTab()
        {
            if (selectedTab is null) return; // Do nothing if there are no tabs
            int index = Controls.IndexOf(selectedTab); // Get the index of the selected tab
            if (index == 0) SetActiveTab((Tab)Controls[Controls.Count - 2]); // Set the last tab as the selected tab if the selected tab is the first tab
            else SetActiveTab((Tab)Controls[index - 1]); // Set the previous tab as the selected tab
        }

        /**
         * UpdatePanelWidth updates the width of the panel.
         */
        public void UpdatePanelWidth()
        {
            Width = canvas.Width; // Set the width of the panel to the width of the canvas
            if (selectedTab != null) selectedTab!.content.UpdateSize(canvas); // Update the size of the selected tab content
        }

    }
}
