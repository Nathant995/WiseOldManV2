namespace wiseoldmanV2
{
    partial class frmAdminPanel
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
            btnStartBot = new Button();
            btnStopBot = new Button();
            btnStatusSet = new Button();
            txtStatusInput = new TextBox();
            lblAppVersion = new Label();
            rtbMessageInput = new RichTextBox();
            btnSendMessage = new Button();
            gbxMessageType = new GroupBox();
            rdoTest = new RadioButton();
            rdoUpdate = new RadioButton();
            rdoAnnouncement = new RadioButton();
            lblPanelTime = new Label();
            tmrPanelTime = new System.Windows.Forms.Timer(components);
            gbxMessageType.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartBot
            // 
            btnStartBot.Location = new Point(12, 12);
            btnStartBot.Name = "btnStartBot";
            btnStartBot.Size = new Size(62, 33);
            btnStartBot.TabIndex = 0;
            btnStartBot.Text = "Start Bot";
            btnStartBot.UseVisualStyleBackColor = true;
            btnStartBot.Click += btnStartBot_Click;
            // 
            // btnStopBot
            // 
            btnStopBot.Location = new Point(80, 12);
            btnStopBot.Name = "btnStopBot";
            btnStopBot.Size = new Size(41, 33);
            btnStopBot.TabIndex = 1;
            btnStopBot.Text = "Stop";
            btnStopBot.UseVisualStyleBackColor = true;
            btnStopBot.Click += btnStopBot_Click_1;
            // 
            // btnStatusSet
            // 
            btnStatusSet.Enabled = false;
            btnStatusSet.Location = new Point(247, 104);
            btnStatusSet.Name = "btnStatusSet";
            btnStatusSet.Size = new Size(40, 28);
            btnStatusSet.TabIndex = 2;
            btnStatusSet.Text = "SET";
            btnStatusSet.UseVisualStyleBackColor = true;
            btnStatusSet.Click += btnStatusSet_Click;
            // 
            // txtStatusInput
            // 
            txtStatusInput.Location = new Point(22, 108);
            txtStatusInput.Name = "txtStatusInput";
            txtStatusInput.PlaceholderText = "Set a status";
            txtStatusInput.Size = new Size(219, 23);
            txtStatusInput.TabIndex = 3;
            // 
            // lblAppVersion
            // 
            lblAppVersion.AutoSize = true;
            lblAppVersion.ForeColor = SystemColors.ButtonHighlight;
            lblAppVersion.Location = new Point(446, 502);
            lblAppVersion.Name = "lblAppVersion";
            lblAppVersion.Size = new Size(43, 15);
            lblAppVersion.TabIndex = 4;
            lblAppVersion.Text = "v0.6.18";
            // 
            // rtbMessageInput
            // 
            rtbMessageInput.BorderStyle = BorderStyle.FixedSingle;
            rtbMessageInput.Location = new Point(22, 241);
            rtbMessageInput.MaxLength = 2000;
            rtbMessageInput.Name = "rtbMessageInput";
            rtbMessageInput.Size = new Size(444, 199);
            rtbMessageInput.TabIndex = 5;
            rtbMessageInput.Text = "";
            // 
            // btnSendMessage
            // 
            btnSendMessage.Location = new Point(391, 446);
            btnSendMessage.Name = "btnSendMessage";
            btnSendMessage.Size = new Size(75, 28);
            btnSendMessage.TabIndex = 6;
            btnSendMessage.Text = "SEND";
            btnSendMessage.UseVisualStyleBackColor = true;
            btnSendMessage.Click += btnSendMessage_Click;
            // 
            // gbxMessageType
            // 
            gbxMessageType.Controls.Add(rdoTest);
            gbxMessageType.Controls.Add(rdoUpdate);
            gbxMessageType.Controls.Add(rdoAnnouncement);
            gbxMessageType.ForeColor = SystemColors.ButtonFace;
            gbxMessageType.Location = new Point(12, 446);
            gbxMessageType.Name = "gbxMessageType";
            gbxMessageType.Size = new Size(152, 77);
            gbxMessageType.TabIndex = 7;
            gbxMessageType.TabStop = false;
            gbxMessageType.Text = "Type";
            // 
            // rdoTest
            // 
            rdoTest.AutoSize = true;
            rdoTest.Location = new Point(79, 50);
            rdoTest.Name = "rdoTest";
            rdoTest.Size = new Size(45, 19);
            rdoTest.TabIndex = 2;
            rdoTest.TabStop = true;
            rdoTest.Text = "Test";
            rdoTest.UseVisualStyleBackColor = true;
            // 
            // rdoUpdate
            // 
            rdoUpdate.AutoSize = true;
            rdoUpdate.Location = new Point(10, 49);
            rdoUpdate.Name = "rdoUpdate";
            rdoUpdate.Size = new Size(63, 19);
            rdoUpdate.TabIndex = 1;
            rdoUpdate.TabStop = true;
            rdoUpdate.Text = "Update";
            rdoUpdate.UseVisualStyleBackColor = true;
            // 
            // rdoAnnouncement
            // 
            rdoAnnouncement.AutoSize = true;
            rdoAnnouncement.Location = new Point(10, 25);
            rdoAnnouncement.Name = "rdoAnnouncement";
            rdoAnnouncement.Size = new Size(108, 19);
            rdoAnnouncement.TabIndex = 0;
            rdoAnnouncement.TabStop = true;
            rdoAnnouncement.Text = "Announcement";
            rdoAnnouncement.UseVisualStyleBackColor = true;
            // 
            // lblPanelTime
            // 
            lblPanelTime.AutoSize = true;
            lblPanelTime.ForeColor = SystemColors.ButtonFace;
            lblPanelTime.Location = new Point(216, 9);
            lblPanelTime.Name = "lblPanelTime";
            lblPanelTime.Size = new Size(71, 15);
            lblPanelTime.TabIndex = 8;
            lblPanelTime.Text = "{DATE:TIME}";
            // 
            // tmrPanelTime
            // 
            tmrPanelTime.Interval = 1000;
            tmrPanelTime.Tick += tmrPanelTime_Tick;
            // 
            // frmAdminPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSlateBlue;
            ClientSize = new Size(496, 526);
            Controls.Add(lblPanelTime);
            Controls.Add(gbxMessageType);
            Controls.Add(btnSendMessage);
            Controls.Add(rtbMessageInput);
            Controls.Add(lblAppVersion);
            Controls.Add(txtStatusInput);
            Controls.Add(btnStatusSet);
            Controls.Add(btnStopBot);
            Controls.Add(btnStartBot);
            Name = "frmAdminPanel";
            Text = "WiseOldMan Admin Panel";
            Load += frmAdminPanel_Load;
            gbxMessageType.ResumeLayout(false);
            gbxMessageType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartBot;
        private Button btnStopBot;
        private Button btnStatusSet;
        private TextBox txtStatusInput;
        private Label lblAppVersion;
        private RichTextBox rtbMessageInput;
        private Button btnSendMessage;
        private GroupBox gbxMessageType;
        private RadioButton rdoTest;
        private RadioButton rdoUpdate;
        private RadioButton rdoAnnouncement;
        private Label lblPanelTime;
        private System.Windows.Forms.Timer tmrPanelTime;
    }
}