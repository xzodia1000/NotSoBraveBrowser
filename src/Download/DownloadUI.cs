using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Download
{
    /**
     * DownloadUI is a form that displays the download manager of the browser.
     */
    public class DownloadUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        private readonly DownloadManager downloadManager; // The download manager
        private readonly TableLayoutPanel downloadPanel;
        private readonly Panel emptyPanel;
        private readonly Label downloadText;
        private readonly ListView downloadTable;
        private readonly Button downloadButton;
        private string downloadPath; // The path to the file that contains the URLs

        /**
         * DownloadUI is the constructor of the DownloadUI class.
         * It takes a BrowserForm object as a parameter.
         * It initializes the download manager and the download table.
         */
        public DownloadUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            downloadManager = new DownloadManager();
            downloadPanel = new TableLayoutPanel();
            emptyPanel = new Panel();
            downloadText = new Label();
            downloadTable = new ListView();
            downloadButton = new Button();
            downloadPath = ""; // Set the path to the file that contains the URLs to an empty string

            InitDownloadUI();
            InitDownloadPanel();
            InitDownloadTable();
            InitEmptyPanel();
            InitDownloadButton();
        }

        /**
         * InitDownloadUI is a method that initializes the download UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
        private void InitDownloadUI()
        {
            Text = "Download Manager";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosing += Form_FormClosing;
        }

        /**
         * InitDownloadPanel is a method that initializes the download panel.
         * It sets the properties of the download panel.
         */
        private void InitDownloadPanel()
        {
            downloadPanel.Dock = DockStyle.Fill; // Fill the form with the download panel
            downloadPanel.ColumnCount = 1;
            downloadPanel.RowCount = 2;
            downloadPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // 100% width
            downloadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Takes up all available space
            downloadPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Fixed 50px height

            Controls.Add(downloadPanel); // Add the download panel to the form
        }

        /**
         * InitEmptyPanel is a method that initializes the inital panel to get the file
         * from the user.
         * It sets the properties of the empty panel.
         * It also sets the event handlers for the drag enter and drop events.
         */
        private void InitEmptyPanel()
        {
            emptyPanel.Name = "emptyPanel";
            emptyPanel.BackColor = Color.White;
            emptyPanel.Dock = DockStyle.Fill;
            emptyPanel.BorderStyle = BorderStyle.FixedSingle;
            emptyPanel.AllowDrop = true;
            emptyPanel.DragEnter += EmptyPanel_DragEnter; // Set the event handler for the drag enter event
            emptyPanel.DragDrop += EmptyPanel_DragDrop; // Set the event handler for the drag drop event


            downloadText.Name = "downloadText";
            downloadText.Text = "Drag and drop or attach file to download.";
            downloadText.Dock = DockStyle.Fill; // Fill the empty panel with the label
            downloadText.TextAlign = ContentAlignment.MiddleCenter;
            downloadText.Font = new Font("Roboto", 12F, FontStyle.Bold);
            downloadText.MouseHover += (sender, e) => downloadText.Cursor = Cursors.Hand; // Set the cursor to hand when the mouse hovers over the label
            downloadText.Click += EmptyPanel_Click;

            emptyPanel.Controls.Add(downloadText); // Add the label to the empty panel
            downloadPanel.Controls.Add(emptyPanel, 0, 0); // Add the empty panel to the download panel at row 0 and column 0
        }

        /**
         * InitDownloadTable is a method that initializes the download table.
         * It sets the properties of the download table.
         */
        private void InitDownloadTable()
        {
            downloadTable.Name = "downloadTable";
            downloadTable.View = View.Details;
            downloadTable.FullRowSelect = true;
            downloadTable.GridLines = true;
            downloadTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            downloadTable.MultiSelect = false;
            downloadTable.Dock = DockStyle.Fill;

            downloadTable.Columns.Add("Status", 100);
            downloadTable.Columns.Add("Size", 100);

            // Set the width of the URL column considering the width of the vertical scroll bar
            downloadTable.Columns.Add("URL", downloadPanel.Width - 300 - SystemInformation.VerticalScrollBarWidth);

        }

        /**
         * InitDownloadButton is a method that initializes the download button.
         * It sets the properties of the download button.
         * It also sets the event handler for the click event.
         */
        private void InitDownloadButton()
        {
            downloadButton.Name = "downloadButton";
            downloadButton.Text = "Download";
            downloadButton.Dock = DockStyle.Fill; // Fill the download panel with the button
            downloadButton.Enabled = false;

            // Set the cursor to default when the mouse hovers over the button
            downloadButton.MouseHover += (sender, e) => downloadButton.Cursor = Cursors.Hand;
            downloadButton.Click += DownloadButton_Click;
            downloadPanel.Controls.Add(downloadButton, 0, 1);
        }

        /**
         * Form_FormClosing is an event handler for the form closing event.
         * It takes an object and a FormClosingEventArgs object as parameters.
         * It cancels the form close request and hides the form so it can be reopened later.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            browserForm.Enabled = true; // Enable the main form

            // Reset the download UI
            downloadPanel.Controls.Remove(downloadTable); // Remove the download table from the download panel
            downloadPanel.Controls.Add(emptyPanel, 0, 0); // Add the empty panel to the download panel at row 0 and column 0

            emptyPanel.Click += EmptyPanel_Click;
            emptyPanel.Enabled = true;
            downloadText.Enabled = true;

            downloadText.MouseHover += (sender, e) => downloadText.Cursor = Cursors.Hand;
            downloadButton.MouseHover += (sender, e) => downloadButton.Cursor = Cursors.Hand;

            UpdatePanelText("Drag and drop or attach file to download.");

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                Hide();   // hides the form
            }
        }

        /**
         * EmptyPanel_DragDrop is an event handler for the drag drop event of the empty panel.
         * It takes an object and a DragEventArgs object as parameters.
         * It gets the file path from the user and calls the SelectedFile method.
         */
        private void EmptyPanel_DragDrop(object? sender, DragEventArgs e)
        {
            // Get the file path from the user through the drag and drop event
            string[]? files = (string[]?)e.Data?.GetData(DataFormats.FileDrop);

            if (files != null && files.Length > 0)
            {
                // Get the file path if the user dropped a file
                string filePath = files[0];
                SelectedFile(filePath); // Call the SelectedFile method
            }
        }

        /**
         * EmptyPanel_DragEnter is an event handler for the drag enter event of the empty panel.
         * It takes an object and a DragEventArgs object as parameters.
         * It sets the effect of the drag drop event.
         */
        private void EmptyPanel_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // If the user dropped a file, set the effect of the drag drop event to copy
                e.Effect = DragDropEffects.Copy; // Visual feedback
            }
            else
            {
                // If the user didn't drop a file, set the effect of the drag drop event to none
                e.Effect = DragDropEffects.None;
            }
        }

        /**
         * EmptyPanel_Click is an event handler for the click event of the empty panel.
         * It takes an object and an EventArgs object as parameters.
         * It gets the file path from the user and calls the SelectedFile method.
         */
        private void EmptyPanel_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new(); // Open file dialog

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the file path from the user through the open file dialog
                string filePath = openFileDialog.FileName;
                SelectedFile(filePath); // Call the SelectedFile method
            }
        }

        /**
         * DownloadButton_Click is an event handler for the click event of the download button.
         * It takes an object and an EventArgs object as parameters.
         * It gets the bulk downloads and updates the download table.
         */
        private async void DownloadButton_Click(object? sender, EventArgs e)
        {
            UpdateButtonText("Downloading...");
            UpdatePanelText("Please wait...");

            // Disable the download button and the empty panel
            downloadButton.Enabled = false;
            emptyPanel.Click -= EmptyPanel_Click;
            emptyPanel.Enabled = false;
            downloadText.Enabled = false;

            // Set the cursor to default when the mouse hovers over the button and the panel
            downloadText.MouseHover -= (sender, e) => downloadText.Cursor = Cursors.Default;
            downloadButton.MouseHover -= (sender, e) => downloadButton.Cursor = Cursors.Default;

            // Get the bulk downloads and update the download table
            List<BulkDownload> result = await downloadManager.GetBulkDownloads(downloadPath);
            downloadPath = ""; // Reset the path to the file that contains the URLs
            downloadButton.Text = "Download";

            UpdateDownloadTable(result);
        }

        /**
         * SelectedFile is a method that checks if the file is a text file and sets the path to the file.
         * It takes a string as a parameter which is the path to the file.
         * It shows a message box if the file is not a text file.
         */
        private void SelectedFile(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower().Equals(".txt"))
            {
                // If the file is a text file, set the path to the file that contains the URLs
                MessageBox.Show($"Dropped file: {filePath}");
                downloadPath = filePath;
                downloadText.Text = Path.GetFileName(filePath);

                // Enable the download button
                downloadButton.Enabled = true;
                downloadButton.MouseHover -= (sender, e) => downloadButton.Cursor = Cursors.Default;
            }
            else
            {
                // If the file is not a text file, show a message box
                MessageBox.Show("Please drop only text files (.txt)");
            }
        }


        /**
         * UpdatePanelText is a method that updates the text of the download panel.
         * It takes a string as a parameter which is the text of the download panel.
         */
        private void UpdatePanelText(string text)
        {
            downloadText.Text = text;
        }

        /**
         * UpdateButtonText is a method that updates the text of the download button.
         * It takes a string as a parameter which is the text of the download button.
         */
        private void UpdateButtonText(string text)
        {
            downloadButton.Text = text;
        }


        /**
         * UpdateDownloadTable is a method that updates the download table.
         * It takes a list of BulkDownload objects as a parameter.
         * It clears the download table and adds the bulk downloads to the download table.
         */
        private void UpdateDownloadTable(List<BulkDownload> bulkDownloads)
        {
            downloadTable.Items.Clear(); // Clear the download table

            // Add the bulk downloads to the download table and remove the empty panel
            downloadPanel.Controls.Remove(emptyPanel);
            downloadPanel.Controls.Add(downloadTable, 0, 0);


            foreach (BulkDownload bulkDownload in bulkDownloads)
            {
                // Add the bulk download to the download table
                downloadTable.Items.Add(new ListViewItem(new[] { bulkDownload.Code, bulkDownload.Bytes.ToString(), bulkDownload.Url }));
            }
        }

        /**
         * OpenDownload is a method that opens the download UI.
         * It shows the download UI and disables the main form.
         */
        public void OpenDownload()
        {
            Show(); // Show the download UI
            browserForm.Enabled = false; // Disable the main form
        }
    }
}