using NotSoBraveBrowser.models;
using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Download
{

    public class DownloadUI : Form
    {
        private readonly BrowserForm browserForm;
        private readonly DownloadManager downloadManager;
        private readonly TableLayoutPanel downloadPanel;
        private readonly Panel emptyPanel;
        private Label downloadText;
        private readonly ListView downloadTable;
        private readonly Button downloadButton;
        private string downloadPath;

        public DownloadUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            downloadManager = new DownloadManager();
            downloadPanel = new TableLayoutPanel();
            emptyPanel = new Panel();
            downloadText = new Label();
            downloadTable = new ListView();
            downloadButton = new Button();
            downloadPath = "";

            InitDownloadUI();
            InitDownloadPanel();
            InitDownloadTable();
            InitEmptyPanel();
            InitDownloadButton();
        }

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

        private void InitDownloadPanel()
        {
            downloadPanel.Dock = DockStyle.Fill;
            downloadPanel.ColumnCount = 1;
            downloadPanel.RowCount = 2;
            downloadPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // 100% width
            downloadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // Takes up all available space
            downloadPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));     // Fixed 50px height
            Controls.Add(downloadPanel);
        }

        private void InitEmptyPanel()
        {
            emptyPanel.Name = "emptyPanel";
            emptyPanel.BackColor = Color.White;
            emptyPanel.Dock = DockStyle.Fill;
            emptyPanel.BorderStyle = BorderStyle.FixedSingle;
            emptyPanel.AllowDrop = true;
            emptyPanel.DragEnter += EmptyPanel_DragEnter;
            emptyPanel.DragDrop += EmptyPanel_DragDrop;


            downloadText.Name = "downloadText";
            downloadText.Text = "Drag and drop or attach file to download.";
            downloadText.Dock = DockStyle.Fill;
            downloadText.TextAlign = ContentAlignment.MiddleCenter;
            downloadText.Font = new Font("Roboto", 12F, FontStyle.Bold);
            downloadText.MouseHover += (sender, e) => downloadText.Cursor = Cursors.Hand;
            downloadText.Click += EmptyPanel_Click;

            emptyPanel.Controls.Add(downloadText);
            downloadPanel.Controls.Add(emptyPanel, 0, 0);
        }

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
            downloadTable.Columns.Add("URL", downloadPanel.Width - 300 - SystemInformation.VerticalScrollBarWidth);

        }

        private void InitDownloadButton()
        {
            downloadButton.Name = "downloadButton";
            downloadButton.Text = "Download";
            downloadButton.Dock = DockStyle.Fill;
            downloadButton.Enabled = false;

            downloadButton.MouseHover += (sender, e) => downloadButton.Cursor = Cursors.Hand;
            downloadButton.Click += DownloadButton_Click;
            downloadPanel.Controls.Add(downloadButton, 0, 1);
        }

        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            browserForm.Enabled = true;

            downloadPanel.Controls.Remove(downloadTable);
            downloadPanel.Controls.Add(emptyPanel, 0, 0);

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


        private void EmptyPanel_DragDrop(object? sender, DragEventArgs e)
        {
            string[]? files = (string[]?)e.Data?.GetData(DataFormats.FileDrop);

            if (files != null && files.Length > 0)
            {
                string filePath = files[0];

                if (Path.GetExtension(filePath).ToLower() == ".txt")
                {
                    SelectedFile(filePath);
                }
                else
                {
                    MessageBox.Show("Please drop only text files (.txt)");
                }
            }
        }

        private void EmptyPanel_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Visual feedback
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void EmptyPanel_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                if (Path.GetExtension(filePath).ToLower() == ".txt")
                {
                    SelectedFile(filePath);
                }
                else
                {
                    MessageBox.Show("Please drop only text files (.txt)");
                }
            }
        }

        private async void DownloadButton_Click(object? sender, EventArgs e)
        {
            UpdateButtonText("Downloading...");
            UpdatePanelText("Please wait...");

            downloadButton.Enabled = false;
            emptyPanel.Click -= EmptyPanel_Click;
            emptyPanel.Enabled = false;
            downloadText.Enabled = false;

            downloadText.MouseHover -= (sender, e) => downloadText.Cursor = Cursors.Default;
            downloadButton.MouseHover -= (sender, e) => downloadButton.Cursor = Cursors.Default;

            List<BulkDownload> result = await downloadManager.GetBulkDownloads(downloadPath);
            downloadPath = "";
            downloadButton.Text = "Download";

            UpdateDownloadTable(result);
        }

        private void SelectedFile(string filePath)
        {
            MessageBox.Show($"Dropped file: {filePath}");
            downloadPath = filePath;
            downloadText.Text = Path.GetFileName(filePath);
            downloadButton.Enabled = true;
            downloadButton.MouseHover -= (sender, e) => downloadButton.Cursor = Cursors.Default;
        }

        private void UpdatePanelText(string text)
        {
            downloadText.Text = text;
        }

        private void UpdateButtonText(string text)
        {
            downloadButton.Text = text;
        }

        private void UpdateDownloadTable(List<BulkDownload> bulkDownloads)
        {
            downloadTable.Items.Clear();
            downloadPanel.Controls.Remove(emptyPanel);
            downloadPanel.Controls.Add(downloadTable, 0, 0);
            foreach (BulkDownload bulkDownload in bulkDownloads)
            {
                downloadTable.Items.Add(new ListViewItem(new[] { bulkDownload.Code, bulkDownload.Bytes.ToString(), bulkDownload.Url }));
            }
        }

        public void OpenDownload()
        {
            Show();
            browserForm.Enabled = false;
        }
    }
}