using System;
using System.Windows.Forms;

namespace SQRL.Samples.DesktopClient
{
    public partial class UrlPrompt : Form
    {
        public UrlPrompt()
        {
            InitializeComponent();
        }

        private void UrlPrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;

            try
            {
                var uri = Url;
            }
            catch (UriFormatException)
            {
                if (DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("Invalid address.", "Invalid address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        public Uri Url
        {
            get { return new Uri(urlTextBox.Text); }
        }

        private void CloseDialog(object sender, EventArgs e)
        {
            Close();
        }
    }
}
