using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
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

        private void OpenEnteredUrl(object sender, EventArgs e)
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

        private void Exit(object sender, EventArgs e)
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

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            var sqrl = new SqrlClient(_identity);
            var uri = e.Argument.ToString();
            sqrl.Process(uri);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void CreateNewIdentity(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to override your current identity with a new one?",
                "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CreateIdentity();
            }
        }

        private void Register(object sender, EventArgs e)
        {
            LaunchSelfProcess("/register");
        }

        private void Unregister(object sender, EventArgs e)
        {
            LaunchSelfProcess("/unregister");
        }

        private void LaunchSelfProcess(string args)
        {
            string msg;
            string action = args.Remove(0, 1);
            Process proc;
            bool success = false;

            ProcessStartInfo info = new ProcessStartInfo(Assembly.GetExecutingAssembly().Location, args);
            info.Verb = "runas";
            info.UseShellExecute = true;

            try
            {
                proc = Process.Start(info);
                proc.WaitForExit();
                success = proc.ExitCode == Program.Success;

                switch (proc.ExitCode)
                {
                    case Program.Success:
                        msg = string.Format("Protocols {0}ed successfully.", action);
                        break;

                    case Program.Unauthorized:
                        msg = string.Format("Unable to {0} protocols: Access denied.", action);
                        break;

                    case Program.NotRegistered:
                        msg = "Protocol is not registered.";
                        break;

                    default:
                        msg = string.Format("There was an error {0}ing the protocol.", action);
                        break;
                }
            }
            catch (Win32Exception ex)
            {
                msg = string.Format("There was an error starting the registration helper: {0}", ex.Message);
            }

            MessageBox.Show(msg, success ? "Success" : "Error", MessageBoxButtons.OK,
                            success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void FormLoad(object sender, EventArgs e)
        {
            if (Urls.Count <= 0)
            {
                return;
            }

            try
            {
                LoadIdentity();

                foreach (var url in Urls)
                {
                    try
                    {
                        var uri = new Uri(url);
                        OpenUrl(uri);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading url: " + ex.Message, "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                Urls.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
