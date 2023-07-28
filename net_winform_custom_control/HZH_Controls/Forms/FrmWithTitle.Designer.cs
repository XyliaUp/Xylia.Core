// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="FrmWithTitle.Designer.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
namespace HZH_Controls.Forms
{
    /// <summary>
    /// Class FrmWithTitle.
    /// Implements the <see cref="HZH_Controls.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="HZH_Controls.Forms.FrmBase" />
    partial class FrmWithTitle
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWithTitle));
			this.lblTitle = new System.Windows.Forms.Label();
			this.ucSplitLine_H1 = new HZH_Controls.Controls.UCSplitLine_H();
			this.btnClose = new System.Windows.Forms.Panel();
			this.btn_Min = new System.Windows.Forms.Panel();
			this.btn_Max = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.Azure;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 17F);
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(629, 61);
			this.lblTitle.TabIndex = 5;
			this.lblTitle.Text = "标题";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
			// 
			// ucSplitLine_H1
			// 
			this.ucSplitLine_H1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.ucSplitLine_H1.Dock = System.Windows.Forms.DockStyle.Top;
			this.ucSplitLine_H1.Location = new System.Drawing.Point(0, 61);
			this.ucSplitLine_H1.Name = "ucSplitLine_H1";
			this.ucSplitLine_H1.Size = new System.Drawing.Size(629, 1);
			this.ucSplitLine_H1.TabIndex = 0;
			this.ucSplitLine_H1.TabStop = false;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.BackColor = System.Drawing.Color.Azure;
			this.btnClose.BackgroundImage = global::HZH_Controls.Properties.Resources.dialog_close;
			this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnClose.Location = new System.Drawing.Point(601, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(28, 60);
			this.btnClose.TabIndex = 6;
			this.btnClose.Visible = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btn_Min
			// 
			this.btn_Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Min.BackColor = System.Drawing.Color.Azure;
			this.btn_Min.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Min.BackgroundImage")));
			this.btn_Min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btn_Min.Location = new System.Drawing.Point(522, 1);
			this.btn_Min.Name = "btn_Min";
			this.btn_Min.Size = new System.Drawing.Size(28, 60);
			this.btn_Min.TabIndex = 7;
			this.btn_Min.Click += new System.EventHandler(this.btn_Min_Click);
			this.btn_Min.Paint += new System.Windows.Forms.PaintEventHandler(this.btn_Min_Paint);
			// 
			// btn_Max
			// 
			this.btn_Max.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Max.BackColor = System.Drawing.Color.Azure;
			this.btn_Max.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Max.BackgroundImage")));
			this.btn_Max.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btn_Max.Location = new System.Drawing.Point(563, 1);
			this.btn_Max.Name = "btn_Max";
			this.btn_Max.Size = new System.Drawing.Size(28, 60);
			this.btn_Max.TabIndex = 8;
			this.btn_Max.Click += new System.EventHandler(this.btn_Max_Click);
			// 
			// FrmWithTitle
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.BorderStyleSize = 2;
			this.ClientSize = new System.Drawing.Size(629, 433);
			this.Controls.Add(this.btn_Max);
			this.Controls.Add(this.btn_Min);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.ucSplitLine_H1);
			this.Controls.Add(this.lblTitle);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsFullSize = false;
			this.IsShowMaskDialog = true;
			this.IsShowRegion = true;
			this.Name = "FrmWithTitle";
			this.Redraw = true;
			this.ShowIcon = false;
			this.Text = "FrmWithTitle";
			this.Load += new System.EventHandler(this.FrmWithTitle_Load);
			this.Shown += new System.EventHandler(this.FrmWithTitle_Shown);
			this.SizeChanged += new System.EventHandler(this.FrmWithTitle_SizeChanged);
			this.VisibleChanged += new System.EventHandler(this.FrmWithTitle_VisibleChanged);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmWithTitle_Paint);
			this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// The uc split line h1
        /// </summary>
        private Controls.UCSplitLine_H ucSplitLine_H1;
		private System.Windows.Forms.Panel btn_Min;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel btnClose;
		private System.Windows.Forms.Panel btn_Max;
	}
}