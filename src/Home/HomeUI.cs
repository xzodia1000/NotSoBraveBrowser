using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Home
{
    public class HomeUI : Form
    {
        private readonly BrowserForm browserForm;
        public HomeManager homeManager;
        private readonly TextBox homeTextBox;
        private readonly Button setHomeButton;

        public HomeUI(BrowserForm browserForm)
        {
            this.browserForm = browserForm;
            homeManager = new HomeManager();
            homeTextBox = new TextBox();
            setHomeButton = new Button();

            InitHomeUI();
            InitHomeTextBox();
            InitSetHomeButton();
        }

        private void InitHomeUI()
        {
            Text = "Change Homepage";
            Size = new Size(500, 200);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            FormClosing += Form_FormClosing;
        }

        private void InitHomeTextBox()
        {
            homeTextBox.Name = "homeTextBox";
            homeTextBox.Size = new Size(400, 30);
            homeTextBox.Location = new Point(50, 50);
            homeTextBox.Text = homeManager.GetHome();
            Controls.Add(homeTextBox);
        }

        private void InitSetHomeButton()
        {
            setHomeButton.Name = "setHomeButton";
            setHomeButton.Size = new Size(100, 30);
            setHomeButton.Location = new Point(50, 100);
            setHomeButton.Text = "Set Home";
            setHomeButton.Click += SetHomeButton_Click;
            Controls.Add(setHomeButton);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            browserForm.Enabled = true;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                homeTextBox.Text = homeManager.GetHome();
                Hide();   // hides the form
            }
        }

        private void SetHomeButton_Click(object sender, EventArgs e)
        {
            homeManager.SetHome(homeTextBox.Text);
            homeTextBox.Text = homeManager.GetHome();
            MessageBox.Show("Home set to " + homeTextBox.Text);
        }

        public void OpenHome()
        {
            Show();
            browserForm.Enabled = false;
        }
    }
}