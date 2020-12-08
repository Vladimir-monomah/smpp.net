using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SmppClientDemo
{

	public class AboutSmppClientDemo : Form
	{
		private Label l1;
		private Label label1;
		private Button bOK;
		private Label label2;
		private Label label3;
        /// <summary>
        /// Обязательная дизайнерская переменная.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public AboutSmppClientDemo()
		{
			Version ver = new Version();

            this.InitializeComponent();
            this.label1.Text = "Версия: "+Assembly.GetExecutingAssembly().GetName().Version.ToString();

		}

        /// <summary>
        /// Очистите все используемые ресурсы.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(this.components != null)
				{
                    this.components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        #region Windows Form Designer generated code
        /// <summary>
        /// Обязательный метод для поддержки дизайнера - не изменять
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutSmppClientDemo));
            this.l1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // l1
            // 
            this.l1.Dock = System.Windows.Forms.DockStyle.Top;
            this.l1.Location = new System.Drawing.Point(0, 0);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(346, 32);
            this.l1.TabIndex = 1;
            this.l1.Text = "SMPP SmppClient Демо";
            this.l1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Версия";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(111, 109);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(112, 23);
            this.bOK.TabIndex = 5;
            this.bOK.Text = "OK";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(346, 32);
            this.label2.TabIndex = 6;
            this.label2.Text = "Авторские права© 2020\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(346, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Все права защищены.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutSmppClientDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(346, 144);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.l1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutSmppClientDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "О программе";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
