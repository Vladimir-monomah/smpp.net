namespace SmppServerDemo
{
    partial class SendMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendMessage));
            this.label17 = new System.Windows.Forms.Label();
            this.cbDataCoding = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDestAdrNPI = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDestAdrTON = new System.Windows.Forms.TextBox();
            this.tbDestAdr = new System.Windows.Forms.TextBox();
            this.bSubmit = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbSrcAdrNPI = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSrcAdrTON = new System.Windows.Forms.TextBox();
            this.tbSrcAdr = new System.Windows.Forms.TextBox();
            this.tbSend = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lClient = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(9, 113);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 20);
            this.label17.TabIndex = 58;
            this.label17.Text = "Кодирование данных";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbDataCoding
            // 
            this.cbDataCoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataCoding.FormattingEnabled = true;
            this.cbDataCoding.Items.AddRange(new object[] {
            "По умолчанию",
            "Latin1",
            "OctetUnspecified",
            "UCS2",
            "UnicodeFlashSMS",
            "По умолчанию FlashSMS"});
            this.cbDataCoding.Location = new System.Drawing.Point(131, 114);
            this.cbDataCoding.Name = "cbDataCoding";
            this.cbDataCoding.Size = new System.Drawing.Size(144, 21);
            this.cbDataCoding.TabIndex = 57;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(9, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 20);
            this.label12.TabIndex = 56;
            this.label12.Text = "Dest_Addr";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(389, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 20);
            this.label13.TabIndex = 55;
            this.label13.Text = "Dest_Addr_NPI";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDestAdrNPI
            // 
            this.tbDestAdrNPI.Location = new System.Drawing.Point(485, 87);
            this.tbDestAdrNPI.Name = "tbDestAdrNPI";
            this.tbDestAdrNPI.Size = new System.Drawing.Size(24, 20);
            this.tbDestAdrNPI.TabIndex = 54;
            this.tbDestAdrNPI.Text = "1";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(253, 87);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(104, 20);
            this.label14.TabIndex = 53;
            this.label14.Text = "Dest_Addr_TON";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDestAdrTON
            // 
            this.tbDestAdrTON.Location = new System.Drawing.Point(357, 87);
            this.tbDestAdrTON.Name = "tbDestAdrTON";
            this.tbDestAdrTON.Size = new System.Drawing.Size(24, 20);
            this.tbDestAdrTON.TabIndex = 52;
            this.tbDestAdrTON.Text = "1";
            // 
            // tbDestAdr
            // 
            this.tbDestAdr.Location = new System.Drawing.Point(111, 87);
            this.tbDestAdr.Name = "tbDestAdr";
            this.tbDestAdr.Size = new System.Drawing.Size(134, 20);
            this.tbDestAdr.TabIndex = 51;
            // 
            // bSubmit
            // 
            this.bSubmit.Location = new System.Drawing.Point(373, 140);
            this.bSubmit.Name = "bSubmit";
            this.bSubmit.Size = new System.Drawing.Size(116, 23);
            this.bSubmit.TabIndex = 50;
            this.bSubmit.Text = "Отправить";
            this.bSubmit.Click += new System.EventHandler(this.bSubmit_Click);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(9, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 20);
            this.label11.TabIndex = 49;
            this.label11.Text = "Адрес источника";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(9, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 20);
            this.label10.TabIndex = 48;
            this.label10.Text = "Текст";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(389, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 20);
            this.label9.TabIndex = 47;
            this.label9.Text = "Source_Addr_NPI";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSrcAdrNPI
            // 
            this.tbSrcAdrNPI.Location = new System.Drawing.Point(485, 63);
            this.tbSrcAdrNPI.Name = "tbSrcAdrNPI";
            this.tbSrcAdrNPI.Size = new System.Drawing.Size(24, 20);
            this.tbSrcAdrNPI.TabIndex = 46;
            this.tbSrcAdrNPI.Text = "0";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(253, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 45;
            this.label8.Text = "Source_Addr_TON";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSrcAdrTON
            // 
            this.tbSrcAdrTON.Location = new System.Drawing.Point(357, 63);
            this.tbSrcAdrTON.Name = "tbSrcAdrTON";
            this.tbSrcAdrTON.Size = new System.Drawing.Size(24, 20);
            this.tbSrcAdrTON.TabIndex = 44;
            this.tbSrcAdrTON.Text = "0";
            // 
            // tbSrcAdr
            // 
            this.tbSrcAdr.Location = new System.Drawing.Point(111, 63);
            this.tbSrcAdr.Name = "tbSrcAdr";
            this.tbSrcAdr.Size = new System.Drawing.Size(134, 20);
            this.tbSrcAdr.TabIndex = 43;
            // 
            // tbSend
            // 
            this.tbSend.Location = new System.Drawing.Point(81, 38);
            this.tbSend.MaxLength = 0;
            this.tbSend.Name = "tbSend";
            this.tbSend.Size = new System.Drawing.Size(527, 20);
            this.tbSend.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 60;
            this.label1.Text = "Клиент";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lClient
            // 
            this.lClient.Location = new System.Drawing.Point(80, 12);
            this.lClient.Name = "lClient";
            this.lClient.Size = new System.Drawing.Size(71, 20);
            this.lClient.TabIndex = 61;
            this.lClient.Text = "Клиент";
            this.lClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SendMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(620, 177);
            this.Controls.Add(this.lClient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.cbDataCoding);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbDestAdrNPI);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbDestAdrTON);
            this.Controls.Add(this.tbDestAdr);
            this.Controls.Add(this.bSubmit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbSrcAdrNPI);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbSrcAdrTON);
            this.Controls.Add(this.tbSrcAdr);
            this.Controls.Add(this.tbSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SendMessage";
            this.Text = "SendMessage";
            this.Load += new System.EventHandler(this.SendMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbDataCoding;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDestAdrNPI;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbDestAdrTON;
        private System.Windows.Forms.TextBox tbDestAdr;
        private System.Windows.Forms.Button bSubmit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbSrcAdrNPI;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSrcAdrTON;
        private System.Windows.Forms.TextBox tbSrcAdr;
        private System.Windows.Forms.TextBox tbSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lClient;
    }
}