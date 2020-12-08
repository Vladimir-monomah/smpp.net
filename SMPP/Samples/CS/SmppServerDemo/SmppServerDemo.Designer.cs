namespace SmppServerDemo
{
    public partial class SmppServerDemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmppServerDemo));
            this.label19 = new System.Windows.Forms.Label();
            this.cbSSL = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.bStartServer = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.bStopServer = new System.Windows.Forms.Button();
            this.comboCertList = new System.Windows.Forms.ComboBox();
            this.bSendMessage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bDisconnect = new System.Windows.Forms.Button();
            this.lbClients = new System.Windows.Forms.ListBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(12, 32);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(100, 20);
            this.label19.TabIndex = 34;
            this.label19.Text = "Use SSL";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSSL
            // 
            this.cbSSL.AutoSize = true;
            this.cbSSL.Location = new System.Drawing.Point(124, 36);
            this.cbSSL.Name = "cbSSL";
            this.cbSSL.Size = new System.Drawing.Size(15, 14);
            this.cbSSL.TabIndex = 33;
            this.cbSSL.UseVisualStyleBackColor = true;
            this.cbSSL.CheckedChanged += new System.EventHandler(this.cbSSL_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "Порт";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(124, 9);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(100, 20);
            this.tbPort.TabIndex = 31;
            this.tbPort.Text = "7777";
            // 
            // bStartServer
            // 
            this.bStartServer.Location = new System.Drawing.Point(12, 68);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(127, 23);
            this.bStartServer.TabIndex = 35;
            this.bStartServer.Text = "Запустить сервер";
            this.bStartServer.UseVisualStyleBackColor = true;
            this.bStartServer.Click += new System.EventHandler(this.bStartServer_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tbLog);
            this.groupBox2.Location = new System.Drawing.Point(6, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(675, 253);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Журнал";
            // 
            // tbLog
            // 
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(669, 234);
            this.tbLog.TabIndex = 17;
            // 
            // bStopServer
            // 
            this.bStopServer.Enabled = false;
            this.bStopServer.Location = new System.Drawing.Point(145, 68);
            this.bStopServer.Name = "bStopServer";
            this.bStopServer.Size = new System.Drawing.Size(115, 23);
            this.bStopServer.TabIndex = 37;
            this.bStopServer.Text = "Остановить сервер";
            this.bStopServer.UseVisualStyleBackColor = true;
            this.bStopServer.Click += new System.EventHandler(this.bStopServer_Click);
            // 
            // comboCertList
            // 
            this.comboCertList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCertList.Enabled = false;
            this.comboCertList.FormattingEnabled = true;
            this.comboCertList.Location = new System.Drawing.Point(161, 31);
            this.comboCertList.Name = "comboCertList";
            this.comboCertList.Size = new System.Drawing.Size(346, 21);
            this.comboCertList.TabIndex = 38;
            // 
            // bSendMessage
            // 
            this.bSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSendMessage.Enabled = false;
            this.bSendMessage.Location = new System.Drawing.Point(521, 13);
            this.bSendMessage.Name = "bSendMessage";
            this.bSendMessage.Size = new System.Drawing.Size(148, 23);
            this.bSendMessage.TabIndex = 39;
            this.bSendMessage.Text = "Отправить сообщение";
            this.bSendMessage.UseVisualStyleBackColor = true;
            this.bSendMessage.Click += new System.EventHandler(this.bSendMessage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.bDisconnect);
            this.groupBox1.Controls.Add(this.lbClients);
            this.groupBox1.Controls.Add(this.bSendMessage);
            this.groupBox1.Location = new System.Drawing.Point(6, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 117);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connected Clients";
            // 
            // bDisconnect
            // 
            this.bDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDisconnect.Enabled = false;
            this.bDisconnect.Location = new System.Drawing.Point(426, 13);
            this.bDisconnect.Name = "bDisconnect";
            this.bDisconnect.Size = new System.Drawing.Size(89, 23);
            this.bDisconnect.TabIndex = 42;
            this.bDisconnect.Text = "Отключиться";
            this.bDisconnect.UseVisualStyleBackColor = true;
            this.bDisconnect.Click += new System.EventHandler(this.bDisconnect_Click);
            // 
            // lbClients
            // 
            this.lbClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbClients.FormattingEnabled = true;
            this.lbClients.Location = new System.Drawing.Point(3, 42);
            this.lbClients.Name = "lbClients";
            this.lbClients.Size = new System.Drawing.Size(666, 56);
            this.lbClients.TabIndex = 41;
            this.lbClients.SelectedIndexChanged += new System.EventHandler(this.lbClients_SelectedIndexChanged);
            // 
            // SmppServerDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(693, 485);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboCertList);
            this.Controls.Add(this.bStopServer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bStartServer);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cbSSL);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbPort);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SmppServerDemo";
            this.Text = "SMPP SmppServer Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SmppServerDemo_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox cbSSL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Button bStartServer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button bStopServer;
        private System.Windows.Forms.ComboBox comboCertList;
        private System.Windows.Forms.Button bSendMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbClients;
        private System.Windows.Forms.Button bDisconnect;
    }
}

