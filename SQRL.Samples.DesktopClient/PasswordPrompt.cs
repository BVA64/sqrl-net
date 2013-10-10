using System;
using System.Windows.Forms;

namespace SQRL.Samples.DesktopClient
{
    public partial class PasswordPrompt : Form
    {
        public PasswordPrompt()
        {
            InitializeComponent();
        }

        public string Password
        {
            get { return passwordTextBox.Text; }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            Close();
        }
    }
}
