using System;
using System.Security;
using System.Windows.Forms;

namespace SQRL.Samples.DesktopClient
{
    public partial class PasswordPrompt : Form
    {
        public PasswordPrompt()
        {
            InitializeComponent();
        }

        public SecureString Password
        {
            get
            {
                var password = new SecureString();
                foreach (var c in passwordTextBox.Text)
                {
                    password.AppendChar(c);
                }
                return password;
            }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            Close();
        }
    }
}
