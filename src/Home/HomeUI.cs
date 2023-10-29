using NotSoBraveBrowser.src.Browser;

namespace NotSoBraveBrowser.src.Home
{
    /**
     * HomeUI is a form that displays the edit home page UI.
     */
    public class HomeUI : Form
    {
        private readonly BrowserForm browserForm; // The main form of the browser
        public HomeManager homeManager; // The home manager
        private readonly TextBox homeTextBox;
        private readonly Button setHomeButton;

        /**
         * HomeUI is the constructor of the HomeUI class.
         * It takes a BrowserForm object as a parameter.
         * It initializes the home manager, the home text box and the set home button.
         */
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

        /**
         * InitHomeUI is a method that initializes the home UI.
         * It sets the properties of the form.
         * It also sets the event handler for the form closing event.
         */
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

        /**
         * InitHomeTextBox is a method that initializes the home text box.
         * It sets the properties of the home text box.
         */
        private void InitHomeTextBox()
        {
            homeTextBox.Name = "homeTextBox";
            homeTextBox.Size = new Size(400, 30);
            homeTextBox.Location = new Point(50, 50);
            homeTextBox.Text = homeManager.GetHome(); // Set the text of the home text box to the current home page
            Controls.Add(homeTextBox);
        }

        /**
         * InitSetHomeButton is a method that initializes the set home button.
         * It sets the properties of the set home button.
         * It also sets the event handler for the click event.
         */
        private void InitSetHomeButton()
        {
            setHomeButton.Name = "setHomeButton";
            setHomeButton.Size = new Size(100, 30);
            setHomeButton.Location = new Point(50, 100);
            setHomeButton.Text = "Set Home";
            setHomeButton.Click += SetHomeButton_Click;
            Controls.Add(setHomeButton);
        }

        /**
         * Form_FormClosing is an event handler for the form closing event.
         * It takes a FormClosingEventArgs object as a parameter.
         * It enables the browser form and hides the home UI.
         */
        private void Form_FormClosing(object? sender, FormClosingEventArgs e)
        {
            browserForm.Enabled = true; // Enable the browser form

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancels the form close request
                homeTextBox.Text = homeManager.GetHome(); // Set the text of the home text box to the new home page
                Hide();   // hides the form
            }
        }

        /**
         * SetHomeButton_Click is an event handler for the click event of the set home button.
         * It takes an object and an EventArgs object as parameters.
         * It sets the home page to the text of the home text box.
         * It also shows a message box to indicate that the home page is set.
         */
        private async void SetHomeButton_Click(object? sender, EventArgs e)
        {
            bool setHome = await homeManager.SetHome(homeTextBox.Text);
            if (setHome)
            {
                // Set the home page to the text of the home text box
                homeTextBox.Text = homeManager.GetHome();
                MessageBox.Show("Home set to " + homeTextBox.Text);
            }
            else
            {
                MessageBox.Show("Invalid URL");
            }

        }

        /**
         * OpenHome is a method that opens the home UI.
         * It hides the browser form and shows the home UI.
         */
        public void OpenHome()
        {
            Show(); // Show the home UI
            browserForm.Enabled = false; // Disable the browser form
        }
    }
}