// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="FrmWithTitle.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;




namespace HZH_Controls.Forms
{
	/// <summary>
	/// Class FrmWithTitle.
	/// Implements the <see cref="HZH_Controls.Forms.FrmBase" />
	/// </summary>
	/// <seealso cref="HZH_Controls.Forms.FrmBase" />
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
	public partial class FrmWithTitle : FrmBase
	{
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		[Description("窗体标题"), Category("自定义")]
		public string Title
		{
			get
			{
				return lblTitle.Text;
			}
			set
			{
				lblTitle.Text = value;
			}
		}

		/// <summary>
		/// The is show close BTN
		/// </summary>
		private bool _isShowCloseBtn = false;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is show close BTN.
		/// </summary>
		/// <value><c>true</c> if this instance is show close BTN; otherwise, <c>false</c>.</value>
		[Description("是否显示右上角关闭按钮"), Category("自定义")]
		public bool IsShowCloseBtn
		{
			get
			{
				return _isShowCloseBtn;
			}
			set
			{
				_isShowCloseBtn = value;
				btnClose.Visible = value;
				if (value)
				{
					btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
					btnClose.BringToFront();
				}
			}
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="FrmWithTitle" /> class.
		/// </summary>
		public FrmWithTitle()
		{
			InitializeComponent();

			this.ShowInTaskbar = true;
			InitFormMove(this.lblTitle);
		}


		/// <summary>
		/// Handles the Shown event of the FrmWithTitle control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		private void FrmWithTitle_Shown(object sender, EventArgs e)
		{
			if (IsShowCloseBtn)
			{
				btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
				btnClose.BringToFront();
			}
		}


		/// <summary>
		/// Handles the VisibleChanged event of the FrmWithTitle control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		private void FrmWithTitle_VisibleChanged(object sender, EventArgs e)
		{
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FrmWithTitle_SizeChanged(object sender, EventArgs e)
		{
			// 切换窗体状态时变更按钮颜色
			// this.btn_Max.BackgroundImage = (this.WindowState == FormWindowState.Maximized) ? Properties.Resources.Rect_gray : Properties.Resources.Rect;

			btnClose.Location = new Point(this.Width - btnClose.Width - 10, btnClose.Location.Y);
			btn_Max.Location = new Point(this.Width - btnClose.Width - btn_Max.Width - 20, btnClose.Location.Y);
			btn_Min.Location = new Point(this.Width - btnClose.Width - btn_Max.Width - btn_Min.Width - 30, btnClose.Location.Y);
		}

		private void btn_Min_Paint(object sender, PaintEventArgs e)
		{

		}

		private void btn_Min_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void FrmWithTitle_Load(object sender, EventArgs e)
		{
			SetStyle
			(
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw | ControlStyles.UserPaint
				, true
			);

			//AnimateWindow(this.Handle, 500, AW_BLEND | AW_CENTER | AW_ACTIVATE);
		}

		private void FrmWithTitle_Paint(object sender, PaintEventArgs e)
		{
			if ((SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
			{
				using (SolidBrush b = new SolidBrush(Color.AliceBlue))
				{
					Size resizeHandleSize = new Size(2, 2);
					e.Graphics.FillRectangles(b, new Rectangle[] {
										new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
										new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
										new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
										new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
										new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
										new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
										 });
				}
			}
		}

		private void lblTitle_Click(object sender, EventArgs e)
		{

		}

		#region 成员
		//[Browsable(false)]
		//[EditorBrowsable(EditorBrowsableState.Never)]
		#endregion

		private void btn_Max_Click(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal;
			else this.WindowState = FormWindowState.Maximized;
		}
	}
}
