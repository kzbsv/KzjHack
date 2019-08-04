namespace KzjHack {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMessage = new System.Windows.Forms.TabPage();
            this.panelMessage = new System.Windows.Forms.Panel();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.tabTasks = new System.Windows.Forms.TabPage();
            this.tasksControl = new KzjHack.TasksControl();
            this.tabActions = new System.Windows.Forms.TabPage();
            this.actionsMain = new KzjHack.ActionsMain();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.settingsGrid = new System.Windows.Forms.PropertyGrid();
            this.tabOkCancel = new System.Windows.Forms.TabPage();
            this.tabMnemonic = new System.Windows.Forms.TabPage();
            this.mnemonicControl = new KzjHack.Controls.MnemonicControl();
            this.tabWallet = new System.Windows.Forms.TabPage();
            this.buttonTabs = new System.Windows.Forms.Button();
            this.UiRootPanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.walletControl = new KzjHack.Controls.WalletControl();
            this.tabControl.SuspendLayout();
            this.tabMessage.SuspendLayout();
            this.panelMessage.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.tabTasks.SuspendLayout();
            this.tabActions.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabOkCancel.SuspendLayout();
            this.tabMnemonic.SuspendLayout();
            this.tabWallet.SuspendLayout();
            this.UiRootPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMessage);
            this.tabControl.Controls.Add(this.tabLog);
            this.tabControl.Controls.Add(this.tabTasks);
            this.tabControl.Controls.Add(this.tabActions);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Controls.Add(this.tabOkCancel);
            this.tabControl.Controls.Add(this.tabMnemonic);
            this.tabControl.Controls.Add(this.tabWallet);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(960, 610);
            this.tabControl.TabIndex = 0;
            // 
            // tabMessage
            // 
            this.tabMessage.Controls.Add(this.panelMessage);
            this.tabMessage.Location = new System.Drawing.Point(4, 22);
            this.tabMessage.Name = "tabMessage";
            this.tabMessage.Padding = new System.Windows.Forms.Padding(3);
            this.tabMessage.Size = new System.Drawing.Size(952, 584);
            this.tabMessage.TabIndex = 1;
            this.tabMessage.Text = "Message";
            this.tabMessage.UseVisualStyleBackColor = true;
            // 
            // panelMessage
            // 
            this.panelMessage.Controls.Add(this.tbMessage);
            this.panelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMessage.Location = new System.Drawing.Point(3, 3);
            this.panelMessage.Name = "panelMessage";
            this.panelMessage.Size = new System.Drawing.Size(946, 578);
            this.panelMessage.TabIndex = 6;
            // 
            // tbMessage
            // 
            this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessage.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMessage.Location = new System.Drawing.Point(0, 0);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ReadOnly = true;
            this.tbMessage.Size = new System.Drawing.Size(946, 578);
            this.tbMessage.TabIndex = 0;
            // 
            // tabLog
            // 
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(952, 584);
            this.tabLog.TabIndex = 2;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // tabTasks
            // 
            this.tabTasks.Controls.Add(this.tasksControl);
            this.tabTasks.Location = new System.Drawing.Point(4, 22);
            this.tabTasks.Name = "tabTasks";
            this.tabTasks.Padding = new System.Windows.Forms.Padding(3);
            this.tabTasks.Size = new System.Drawing.Size(952, 584);
            this.tabTasks.TabIndex = 3;
            this.tabTasks.Text = "Tasks";
            this.tabTasks.UseVisualStyleBackColor = true;
            // 
            // tasksControl
            // 
            this.tasksControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tasksControl.Location = new System.Drawing.Point(3, 3);
            this.tasksControl.Name = "tasksControl";
            this.tasksControl.Size = new System.Drawing.Size(946, 578);
            this.tasksControl.TabIndex = 1;
            // 
            // tabActions
            // 
            this.tabActions.Controls.Add(this.actionsMain);
            this.tabActions.Location = new System.Drawing.Point(4, 22);
            this.tabActions.Name = "tabActions";
            this.tabActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabActions.Size = new System.Drawing.Size(952, 584);
            this.tabActions.TabIndex = 4;
            this.tabActions.Text = "Actions";
            this.tabActions.UseVisualStyleBackColor = true;
            // 
            // actionsMain
            // 
            this.actionsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionsMain.Location = new System.Drawing.Point(3, 3);
            this.actionsMain.Name = "actionsMain";
            this.actionsMain.Size = new System.Drawing.Size(946, 578);
            this.actionsMain.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.settingsGrid);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(952, 584);
            this.tabSettings.TabIndex = 5;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // settingsGrid
            // 
            this.settingsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGrid.Location = new System.Drawing.Point(3, 3);
            this.settingsGrid.Name = "settingsGrid";
            this.settingsGrid.Size = new System.Drawing.Size(946, 578);
            this.settingsGrid.TabIndex = 0;
            // 
            // tabOkCancel
            // 
            this.tabOkCancel.Location = new System.Drawing.Point(4, 22);
            this.tabOkCancel.Name = "tabOkCancel";
            this.tabOkCancel.Padding = new System.Windows.Forms.Padding(3);
            this.tabOkCancel.Size = new System.Drawing.Size(952, 584);
            this.tabOkCancel.TabIndex = 6;
            this.tabOkCancel.Text = "OkCancel";
            this.tabOkCancel.UseVisualStyleBackColor = true;
            // 
            // tabMnemonic
            // 
            this.tabMnemonic.Controls.Add(this.mnemonicControl);
            this.tabMnemonic.Location = new System.Drawing.Point(4, 22);
            this.tabMnemonic.Name = "tabMnemonic";
            this.tabMnemonic.Padding = new System.Windows.Forms.Padding(3);
            this.tabMnemonic.Size = new System.Drawing.Size(952, 584);
            this.tabMnemonic.TabIndex = 7;
            this.tabMnemonic.Text = "Mnemonic";
            this.tabMnemonic.UseVisualStyleBackColor = true;
            // 
            // mnemonicControl
            // 
            this.mnemonicControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mnemonicControl.Location = new System.Drawing.Point(3, 3);
            this.mnemonicControl.MinimumSize = new System.Drawing.Size(458, 150);
            this.mnemonicControl.Name = "mnemonicControl";
            this.mnemonicControl.Size = new System.Drawing.Size(946, 578);
            this.mnemonicControl.TabIndex = 0;
            // 
            // tabWallet
            // 
            this.tabWallet.Controls.Add(this.walletControl);
            this.tabWallet.Location = new System.Drawing.Point(4, 22);
            this.tabWallet.Name = "tabWallet";
            this.tabWallet.Padding = new System.Windows.Forms.Padding(3);
            this.tabWallet.Size = new System.Drawing.Size(952, 584);
            this.tabWallet.TabIndex = 8;
            this.tabWallet.Text = "Wallet";
            this.tabWallet.UseVisualStyleBackColor = true;
            // 
            // buttonTabs
            // 
            this.buttonTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTabs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTabs.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonTabs.Image = ((System.Drawing.Image)(resources.GetObject("buttonTabs.Image")));
            this.buttonTabs.Location = new System.Drawing.Point(939, 0);
            this.buttonTabs.Name = "buttonTabs";
            this.buttonTabs.Size = new System.Drawing.Size(20, 12);
            this.buttonTabs.TabIndex = 1;
            this.buttonTabs.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonTabs.UseVisualStyleBackColor = true;
            // 
            // UiRootPanel
            // 
            this.UiRootPanel.Controls.Add(this.buttonTabs);
            this.UiRootPanel.Controls.Add(this.tabControl);
            this.UiRootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UiRootPanel.Location = new System.Drawing.Point(0, 0);
            this.UiRootPanel.Name = "UiRootPanel";
            this.UiRootPanel.Size = new System.Drawing.Size(960, 610);
            this.UiRootPanel.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            // 
            // walletControl
            // 
            this.walletControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.walletControl.Location = new System.Drawing.Point(3, 3);
            this.walletControl.Name = "walletControl";
            this.walletControl.Size = new System.Drawing.Size(946, 578);
            this.walletControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 610);
            this.Controls.Add(this.UiRootPanel);
            this.Name = "MainForm";
            this.Text = "KzjHack - 2019 BSV Hackathon";
            this.tabControl.ResumeLayout(false);
            this.tabMessage.ResumeLayout(false);
            this.panelMessage.ResumeLayout(false);
            this.panelMessage.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.tabTasks.ResumeLayout(false);
            this.tabActions.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabOkCancel.ResumeLayout(false);
            this.tabMnemonic.ResumeLayout(false);
            this.tabWallet.ResumeLayout(false);
            this.UiRootPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button buttonTabs;
        private System.Windows.Forms.Panel UiRootPanel;
        private System.Windows.Forms.TabPage tabMessage;
        private System.Windows.Forms.Panel panelMessage;
        internal System.Windows.Forms.TextBox tbMessage;
        internal System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TabPage tabTasks;
        internal System.Windows.Forms.Timer timer1;
        internal TasksControl tasksControl;
        internal System.Windows.Forms.TabPage tabActions;
        internal ActionsMain actionsMain;
        private System.Windows.Forms.TabPage tabSettings;
        internal System.Windows.Forms.PropertyGrid settingsGrid;
        private System.Windows.Forms.TabPage tabOkCancel;
        private System.Windows.Forms.TabPage tabMnemonic;
        internal Controls.MnemonicControl mnemonicControl;
        private System.Windows.Forms.TabPage tabWallet;
        internal Controls.WalletControl walletControl;
    }
}

