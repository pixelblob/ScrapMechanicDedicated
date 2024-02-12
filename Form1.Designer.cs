
namespace ScrapMechanicDedicated
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            startGameServerCtx = new ToolStripMenuItem();
            stopGameServerCtx = new ToolStripMenuItem();
            Exit = new ToolStripMenuItem();
            startServerButton = new Button();
            stopServerButton = new Button();
            panel1 = new Panel();
            resumeServerButton = new Button();
            suspendServerButton = new Button();
            showGameServer = new Button();
            saveGamesListBox = new ListBox();
            label1 = new Label();
            saveGameLabel = new Label();
            label2 = new Label();
            gameTickLabel = new Label();
            label3 = new Label();
            saveGameLabelVersion = new Label();
            label4 = new Label();
            panel2 = new Panel();
            gameLastModifiedDate = new Label();
            gamePlaytimeLabel = new Label();
            gameLastModifiedTime = new Label();
            gameTimeLabel = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            gameDayLabel = new Label();
            richLogBox = new RichTextBox();
            backupServerBtn = new Button();
            imageList1 = new ImageList(components);
            listBox1 = new ListBox();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "test";
            notifyIcon1.BalloonTipTitle = "test";
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Scrap Mechanic Dedicated";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.BackColor = Color.FromArgb(45, 45, 48);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { startGameServerCtx, stopGameServerCtx, Exit });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(134, 70);
            // 
            // startGameServerCtx
            // 
            startGameServerCtx.BackColor = Color.Transparent;
            startGameServerCtx.ForeColor = Color.White;
            startGameServerCtx.Image = Properties.Resources.power_on_solid1;
            startGameServerCtx.ImageAlign = ContentAlignment.MiddleLeft;
            startGameServerCtx.Name = "startGameServerCtx";
            startGameServerCtx.Size = new Size(133, 22);
            startGameServerCtx.Text = "Start Server";
            startGameServerCtx.Click += startServerButton_Click;
            // 
            // stopGameServerCtx
            // 
            stopGameServerCtx.BackColor = Color.Transparent;
            stopGameServerCtx.Enabled = false;
            stopGameServerCtx.ForeColor = Color.White;
            stopGameServerCtx.Image = Properties.Resources.power_off_solid1;
            stopGameServerCtx.ImageTransparentColor = Color.Chartreuse;
            stopGameServerCtx.Name = "stopGameServerCtx";
            stopGameServerCtx.Size = new Size(133, 22);
            stopGameServerCtx.Text = "Stop Server";
            stopGameServerCtx.Click += stopServerButton_Click;
            // 
            // Exit
            // 
            Exit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            Exit.Font = new Font("Segoe UI", 9F);
            Exit.ForeColor = Color.Red;
            Exit.Name = "Exit";
            Exit.Size = new Size(133, 22);
            Exit.Text = "Exit";
            Exit.Click += Exit_Click;
            // 
            // startServerButton
            // 
            startServerButton.BackColor = Color.PaleGreen;
            startServerButton.FlatAppearance.BorderSize = 0;
            startServerButton.FlatStyle = FlatStyle.Flat;
            startServerButton.ForeColor = SystemColors.ControlText;
            startServerButton.Location = new Point(9, 40);
            startServerButton.Name = "startServerButton";
            startServerButton.Size = new Size(97, 23);
            startServerButton.TabIndex = 1;
            startServerButton.Text = " Start Server";
            startServerButton.UseVisualStyleBackColor = false;
            startServerButton.Click += startServerButton_Click;
            // 
            // stopServerButton
            // 
            stopServerButton.BackColor = Color.Crimson;
            stopServerButton.Enabled = false;
            stopServerButton.FlatAppearance.BorderSize = 0;
            stopServerButton.FlatStyle = FlatStyle.Flat;
            stopServerButton.Location = new Point(9, 11);
            stopServerButton.Name = "stopServerButton";
            stopServerButton.Size = new Size(97, 23);
            stopServerButton.TabIndex = 2;
            stopServerButton.Text = " Stop Server";
            stopServerButton.UseVisualStyleBackColor = false;
            stopServerButton.Click += stopServerButton_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(resumeServerButton);
            panel1.Controls.Add(suspendServerButton);
            panel1.Controls.Add(stopServerButton);
            panel1.Controls.Add(startServerButton);
            panel1.Location = new Point(676, 312);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 6, 6);
            panel1.Size = new Size(115, 130);
            panel1.TabIndex = 3;
            // 
            // resumeServerButton
            // 
            resumeServerButton.BackColor = Color.YellowGreen;
            resumeServerButton.Enabled = false;
            resumeServerButton.FlatAppearance.BorderSize = 0;
            resumeServerButton.FlatStyle = FlatStyle.Flat;
            resumeServerButton.ForeColor = SystemColors.ControlText;
            resumeServerButton.Location = new Point(9, 98);
            resumeServerButton.Name = "resumeServerButton";
            resumeServerButton.Size = new Size(97, 23);
            resumeServerButton.TabIndex = 4;
            resumeServerButton.Text = "Resume Server";
            resumeServerButton.UseVisualStyleBackColor = false;
            resumeServerButton.Click += resumeServerButton_Click;
            // 
            // suspendServerButton
            // 
            suspendServerButton.BackColor = Color.Yellow;
            suspendServerButton.Enabled = false;
            suspendServerButton.FlatAppearance.BorderSize = 0;
            suspendServerButton.FlatStyle = FlatStyle.Flat;
            suspendServerButton.ForeColor = SystemColors.ControlText;
            suspendServerButton.Location = new Point(9, 69);
            suspendServerButton.Name = "suspendServerButton";
            suspendServerButton.Size = new Size(97, 23);
            suspendServerButton.TabIndex = 3;
            suspendServerButton.Text = "Suspend Server";
            suspendServerButton.UseVisualStyleBackColor = false;
            suspendServerButton.Click += suspendServerButton_Click;
            // 
            // showGameServer
            // 
            showGameServer.BackColor = Color.FromArgb(57, 57, 57);
            showGameServer.Location = new Point(12, 12);
            showGameServer.Name = "showGameServer";
            showGameServer.Size = new Size(145, 23);
            showGameServer.TabIndex = 4;
            showGameServer.Text = "Show";
            showGameServer.UseVisualStyleBackColor = false;
            showGameServer.Click += show_Click;
            // 
            // saveGamesListBox
            // 
            saveGamesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            saveGamesListBox.BackColor = Color.FromArgb(81, 81, 81);
            saveGamesListBox.FormattingEnabled = true;
            saveGamesListBox.ItemHeight = 15;
            saveGamesListBox.Location = new Point(12, 41);
            saveGamesListBox.Name = "saveGamesListBox";
            saveGamesListBox.Size = new Size(145, 379);
            saveGamesListBox.TabIndex = 5;
            saveGamesListBox.SelectedIndexChanged += saveGamesListBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 8);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 6;
            label1.Text = "Save Game:";
            // 
            // saveGameLabel
            // 
            saveGameLabel.AutoSize = true;
            saveGameLabel.Location = new Point(96, 8);
            saveGameLabel.Name = "saveGameLabel";
            saveGameLabel.Size = new Size(38, 15);
            saveGameLabel.TabIndex = 7;
            saveGameLabel.Text = "label2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 23);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 8;
            label2.Text = "Game Tick:";
            // 
            // gameTickLabel
            // 
            gameTickLabel.AutoSize = true;
            gameTickLabel.Location = new Point(96, 23);
            gameTickLabel.Name = "gameTickLabel";
            gameTickLabel.Size = new Size(38, 15);
            gameTickLabel.TabIndex = 9;
            gameTickLabel.Text = "label3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 38);
            label3.Name = "label3";
            label3.Size = new Size(82, 15);
            label3.TabIndex = 10;
            label3.Text = "Game Version:";
            // 
            // saveGameLabelVersion
            // 
            saveGameLabelVersion.AutoSize = true;
            saveGameLabelVersion.Location = new Point(96, 38);
            saveGameLabelVersion.Name = "saveGameLabelVersion";
            saveGameLabelVersion.Size = new Size(38, 15);
            saveGameLabelVersion.TabIndex = 11;
            saveGameLabelVersion.Text = "label4";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 53);
            label4.Name = "label4";
            label4.Size = new Size(30, 15);
            label4.TabIndex = 12;
            label4.Text = "Day:";
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.BackColor = Color.DimGray;
            panel2.Controls.Add(gameLastModifiedDate);
            panel2.Controls.Add(gamePlaytimeLabel);
            panel2.Controls.Add(gameLastModifiedTime);
            panel2.Controls.Add(gameTimeLabel);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(gameDayLabel);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(saveGameLabel);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(saveGameLabelVersion);
            panel2.Controls.Add(gameTickLabel);
            panel2.Controls.Add(label3);
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(163, 12);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5);
            panel2.Size = new Size(148, 133);
            panel2.TabIndex = 13;
            // 
            // gameLastModifiedDate
            // 
            gameLastModifiedDate.AutoSize = true;
            gameLastModifiedDate.Location = new Point(96, 113);
            gameLastModifiedDate.Name = "gameLastModifiedDate";
            gameLastModifiedDate.Size = new Size(44, 15);
            gameLastModifiedDate.TabIndex = 21;
            gameLastModifiedDate.Text = "label12";
            // 
            // gamePlaytimeLabel
            // 
            gamePlaytimeLabel.AutoSize = true;
            gamePlaytimeLabel.Location = new Point(96, 98);
            gamePlaytimeLabel.Name = "gamePlaytimeLabel";
            gamePlaytimeLabel.Size = new Size(44, 15);
            gamePlaytimeLabel.TabIndex = 20;
            gamePlaytimeLabel.Text = "label11";
            // 
            // gameLastModifiedTime
            // 
            gameLastModifiedTime.AutoSize = true;
            gameLastModifiedTime.Location = new Point(96, 83);
            gameLastModifiedTime.Name = "gameLastModifiedTime";
            gameLastModifiedTime.Size = new Size(44, 15);
            gameLastModifiedTime.TabIndex = 19;
            gameLastModifiedTime.Text = "label10";
            // 
            // gameTimeLabel
            // 
            gameTimeLabel.AutoSize = true;
            gameTimeLabel.Location = new Point(96, 68);
            gameTimeLabel.Name = "gameTimeLabel";
            gameTimeLabel.Size = new Size(38, 15);
            gameTimeLabel.TabIndex = 18;
            gameTimeLabel.Text = "label9";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(8, 113);
            label8.Name = "label8";
            label8.Size = new Size(34, 15);
            label8.TabIndex = 17;
            label8.Text = "Date:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 98);
            label7.Name = "label7";
            label7.Size = new Size(58, 15);
            label7.TabIndex = 16;
            label7.Text = "PlayTime:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 83);
            label6.Name = "label6";
            label6.Size = new Size(36, 15);
            label6.TabIndex = 15;
            label6.Text = "Time:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 68);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 14;
            label5.Text = "Game Time:";
            // 
            // gameDayLabel
            // 
            gameDayLabel.AutoSize = true;
            gameDayLabel.Location = new Point(96, 53);
            gameDayLabel.Name = "gameDayLabel";
            gameDayLabel.Size = new Size(38, 15);
            gameDayLabel.TabIndex = 13;
            gameDayLabel.Text = "label5";
            // 
            // richLogBox
            // 
            richLogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richLogBox.BackColor = Color.FromArgb(81, 81, 81);
            richLogBox.ForeColor = Color.WhiteSmoke;
            richLogBox.Location = new Point(163, 12);
            richLogBox.Name = "richLogBox";
            richLogBox.Size = new Size(504, 436);
            richLogBox.TabIndex = 14;
            richLogBox.Text = "";
            richLogBox.Visible = false;
            // 
            // backupServerBtn
            // 
            backupServerBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            backupServerBtn.BackColor = Color.FromArgb(57, 57, 57);
            backupServerBtn.Location = new Point(12, 426);
            backupServerBtn.Name = "backupServerBtn";
            backupServerBtn.Size = new Size(145, 23);
            backupServerBtn.TabIndex = 15;
            backupServerBtn.Text = "Backup";
            backupServerBtn.UseVisualStyleBackColor = false;
            backupServerBtn.Click += backupServerBtn_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            listBox1.BackColor = Color.FromArgb(81, 81, 81);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(673, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 289);
            listBox1.TabIndex = 16;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(800, 460);
            Controls.Add(listBox1);
            Controls.Add(backupServerBtn);
            Controls.Add(richLogBox);
            Controls.Add(panel2);
            Controls.Add(saveGamesListBox);
            Controls.Add(showGameServer);
            Controls.Add(panel1);
            ForeColor = SystemColors.HighlightText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Scrap Mechanic Dedicated";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void startServerButton_EnabledChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

#endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem Exit;
        private Button startServerButton;
        private Button stopServerButton;
        private Panel panel1;
        private Button showGameServer;
        private ListBox saveGamesListBox;
        private Label label1;
        private Label saveGameLabel;
        private Label label2;
        private Label gameTickLabel;
        private Label label3;
        private Label saveGameLabelVersion;
        private Label label4;
        private Panel panel2;
        private Label gameDayLabel;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label gameLastModifiedDate;
        private Label gamePlaytimeLabel;
        private Label gameLastModifiedTime;
        private Label gameTimeLabel;
        private RichTextBox richLogBox;
        private Button suspendServerButton;
        private Button resumeServerButton;
        private Button backupServerBtn;
        private ImageList imageList1;
        private ListBox listBox1;
        public NotifyIcon notifyIcon1;
        public ToolStripMenuItem stopGameServerCtx;
        public ToolStripMenuItem startGameServerCtx;
    }
}
