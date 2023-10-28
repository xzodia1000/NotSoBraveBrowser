using NotSoBraveBrowser.lib;
using NotSoBraveBrowser.models;

namespace NotSoBraveBrowser.src.Browser
{
    public class ShortcutUI : Form
    {
        private readonly BrowserForm browserForm;
        private readonly ListView shortcutTable;

        public ShortcutUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            shortcutTable = new ListView();

            InitShortcutUI();
            InitShortcutTable();
        }

        private void InitShortcutUI()
        {
            Text = "Keyboard Shortcuts";
            Size = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosing += Form_FormClosing;
        }

        private void InitShortcutTable()
        {
            shortcutTable.Name = "shortcutTable";
            shortcutTable.View = View.Details;
            shortcutTable.FullRowSelect = true;
            shortcutTable.GridLines = true;
            shortcutTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            shortcutTable.MultiSelect = false;
            shortcutTable.Size = new Size(500 - SystemInformation.VerticalScrollBarWidth, 500);

            shortcutTable.Columns.Add("Shortcut", 150);
            shortcutTable.Columns.Add("Description", 300 - SystemInformation.VerticalScrollBarWidth);

            foreach (KeyboardShortcut shortcut in KeyboardShortcutData.data)
            {
                ListViewItem item = new(shortcut.Shortcut);
                item.SubItems.Add(shortcut.Description);
                shortcutTable.Items.Add(item);
            }

            Controls.Add(shortcutTable);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            browserForm.Enabled = true;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
            }
        }

        public void OpenShortcut()
        {
            browserForm.Enabled = false;
            Show();
        }
    }
}