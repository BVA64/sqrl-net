namespace SQRL.Samples.DesktopClient
{
    partial class SqrlDesktopClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importMasterKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMasterKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.registerProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unregisterProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.newIdentityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(550, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openURLToolStripMenuItem,
            this.toolStripSeparator2,
            this.newIdentityToolStripMenuItem,
            this.importMasterKeyToolStripMenuItem,
            this.exportMasterKeyToolStripMenuItem,
            this.toolStripSeparator3,
            this.registerProtocolToolStripMenuItem,
            this.unregisterProtocolToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openURLToolStripMenuItem
            // 
            this.openURLToolStripMenuItem.Name = "openURLToolStripMenuItem";
            this.openURLToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.openURLToolStripMenuItem.Text = "&Open URL";
            this.openURLToolStripMenuItem.Click += new System.EventHandler(this.openURLToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(173, 6);
            // 
            // importMasterKeyToolStripMenuItem
            // 
            this.importMasterKeyToolStripMenuItem.Enabled = false;
            this.importMasterKeyToolStripMenuItem.Name = "importMasterKeyToolStripMenuItem";
            this.importMasterKeyToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.importMasterKeyToolStripMenuItem.Text = "&Import Identity";
            // 
            // exportMasterKeyToolStripMenuItem
            // 
            this.exportMasterKeyToolStripMenuItem.Enabled = false;
            this.exportMasterKeyToolStripMenuItem.Name = "exportMasterKeyToolStripMenuItem";
            this.exportMasterKeyToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exportMasterKeyToolStripMenuItem.Text = "&Export Identity";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(173, 6);
            // 
            // registerProtocolToolStripMenuItem
            // 
            this.registerProtocolToolStripMenuItem.Name = "registerProtocolToolStripMenuItem";
            this.registerProtocolToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.registerProtocolToolStripMenuItem.Text = "&Register Protocol";
            // 
            // unregisterProtocolToolStripMenuItem
            // 
            this.unregisterProtocolToolStripMenuItem.Name = "unregisterProtocolToolStripMenuItem";
            this.unregisterProtocolToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.unregisterProtocolToolStripMenuItem.Text = "&Unregister Protocol";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // newIdentityToolStripMenuItem
            // 
            this.newIdentityToolStripMenuItem.Name = "newIdentityToolStripMenuItem";
            this.newIdentityToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newIdentityToolStripMenuItem.Text = "&New Identity";
            this.newIdentityToolStripMenuItem.Click += new System.EventHandler(this.newIdentityToolStripMenuItem_Click);
            // 
            // SqrlDesktopClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 420);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SqrlDesktopClient";
            this.Text = "SQRL Client";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openURLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem importMasterKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportMasterKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem registerProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unregisterProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newIdentityToolStripMenuItem;
    }
}

