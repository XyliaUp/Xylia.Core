namespace Xylia.Windows.Forms
{
    partial class Announcement
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
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
			this.Btn_OK = new MetroFramework.Controls.MetroButton();
			this.Btn_Exit = new MetroFramework.Controls.MetroButton();
			this.panel1.SuspendLayout();
			this.metroPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// metroLabel1
			// 
			this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.metroLabel1.Location = new System.Drawing.Point(0, 0);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(741, 370);
			this.metroLabel1.TabIndex = 0;
			this.metroLabel1.Text = "暂无公告";
			this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
			//this.metroLabel1.WrapToLine = true;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.metroLabel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(1, 30);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(741, 370);
			this.panel1.TabIndex = 1;
			// 
			// metroPanel1
			// 
			this.metroPanel1.Controls.Add(this.Btn_OK);
			this.metroPanel1.Controls.Add(this.Btn_Exit);
			this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.metroPanel1.HorizontalScrollbarBarColor = true;
			this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
			this.metroPanel1.HorizontalScrollbarSize = 10;
			this.metroPanel1.Location = new System.Drawing.Point(1, 494);
			this.metroPanel1.Name = "metroPanel1";
			this.metroPanel1.Size = new System.Drawing.Size(741, 26);
			this.metroPanel1.TabIndex = 0;
			this.metroPanel1.VerticalScrollbarBarColor = true;
			this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
			this.metroPanel1.VerticalScrollbarSize = 10;
			// 
			// Btn_OK
			// 
			this.Btn_OK.Location = new System.Drawing.Point(19, 3);
			this.Btn_OK.Name = "Btn_OK";
			this.Btn_OK.Size = new System.Drawing.Size(75, 23);
			this.Btn_OK.TabIndex = 4;
			this.Btn_OK.Text = "确定";
			//this.Btn_OK.UseSelectable = true;
			this.Btn_OK.Click += new System.EventHandler(this.MetroButton1_Click);
			// 
			// Btn_Exit
			// 
			this.Btn_Exit.Location = new System.Drawing.Point(139, 3);
			this.Btn_Exit.Name = "Btn_Exit";
			this.Btn_Exit.Size = new System.Drawing.Size(75, 23);
			this.Btn_Exit.TabIndex = 5;
			this.Btn_Exit.Text = "退出";
			//this.Btn_Exit.UseSelectable = true;
			this.Btn_Exit.Click += new System.EventHandler(this.Btn_Demo_Click);
			// 
			// Announcement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(743, 521);
			this.Controls.Add(this.metroPanel1);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.Name = "Announcement";
			this.ShowIcon = false;
			this.Style = MetroFramework.MetroColorStyle.Teal;
			this.Text = "提示";
			this.Load += new System.EventHandler(this.Announcement_Load);
			this.SizeChanged += new System.EventHandler(this.Announcement_SizeChanged);
			this.TextChanged += new System.EventHandler(this.Announcement_TextChanged);
			this.MouseEnter += new System.EventHandler(this.Announcement_MouseEnter);
			this.panel1.ResumeLayout(false);
			this.metroPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.Panel panel1;
     
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroButton Btn_Exit;
        private MetroFramework.Controls.MetroButton Btn_OK;
    }
}