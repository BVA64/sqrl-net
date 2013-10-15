using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SQRL.Client;
using SQRL.Samples.DesktopClient.StorageProvider;

namespace SQRL.Samples.DesktopClient
{
    public partial class SqrlDesktopClient : Form
    {
        private Identity _identity;

        public SqrlDesktopClient()
        {
            InitializeComponent();
            Urls = new List<string>();
        }

        public List<string> Urls { get; private set; }

        private void openURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadIdentity();

                var uri = PromptForUrl();
                if (uri != null)
                {
                    OpenUrl(uri);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenUrl(Uri uri)
        {
            backgroundWorker.RunWorkerAsync(uri);
        }

        private Uri PromptForUrl()
        {
            using (var dialog = new UrlPrompt())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.Url;
                }
            }

            return null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadIdentity()
        {
            if (_identity != null) return;

            string password = GetIdentityPassword();
            if (password != null)
            {
                try
                {
                    _identity = Identity.Open("Identity", password);
                }
                catch (SqrlIdentityNotFoundException)
                {
                    CreateIdentity(password);
                }
            }
        }

        private void CreateIdentity(string password = null)
        {
            if (password == null)
            {
                password = GetIdentityPassword();
            }

            if (password == null) return;

            var random = new Random();
            var entropy = new byte[64];
            random.NextBytes(entropy);

            _identity = Identity.CreateNew("Identity", password, entropy);
        }

        private string GetIdentityPassword()
        {
            using (var dialog = new PasswordPrompt())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.Password;
                }
            }

            return null;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sqrl = new SqrlClient(_identity);
            var uri = e.Argument.ToString();
            sqrl.Process(uri);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("There was an error authenticating: " + e.Error.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newIdentityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you want to override your current identity with a new one?",
                                "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CreateIdentity();
            }
        }
    }
}
