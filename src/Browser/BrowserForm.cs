using NotSoBraveBrowser.src.TabControl;

namespace Browser
{
    public partial class BrowserForm : Form
    {
        private FlowLayoutPanel canvas;
        private TabPanel tabPanel;

        public BrowserForm()
        {
            InitializeComponent();
            canvas = new FlowLayoutPanel();
            tabPanel = new TabPanel(canvas);
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

        private void Browser_Resize(object sender, EventArgs e)
        {
            tabPanel.UpdatePanelWidth();
        }

    }
}