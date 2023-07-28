// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="UCBtnFillet.cs">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HZH_Controls.Controls
{
	/// <summary>
	/// Class UCBtnFillet.
	/// Implements the <see cref="HZH_Controls.Controls.UCControlBase" />
	/// </summary>
	/// <seealso cref="HZH_Controls.Controls.UCControlBase" />
	[DefaultEvent("Click")]
	public partial class UCBtnFillet : UCControlBase
	{
		[Description("按钮点击事件"), Category("自定义")]
		public event EventHandler Click;

		[Description("鼠标进入按钮事件"), Category("自定义")]
		public event EventHandler BtnMouseEnter;

		[Description("鼠标离开按钮事件"), Category("自定义")]
		public event EventHandler BtnMouseLeave;


		/// <summary>
		/// 按钮图片
		/// </summary>
		/// <value>The BTN image.</value>
		[Description("按钮图片"), Category("自定义")]
		public Image BtnImage
		{
			get
			{
				return lbl.Image;
			}
			set
			{
				lbl.Image = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Description("按钮文字"), Category("自定义")]
		public override string Text
		{
			get => lbl.Text;
			set	=> lbl.Text = value;
		}

		public Font BtnFont
		{
			get { return lbl.Font; }
			set
			{
				lbl.Font = value;
			}
		}

		//[Browsable(false)]
		//public new Font Font;



		/// <summary>
		/// Initializes a new instance of the <see cref="UCBtnFillet" /> class.
		/// </summary>
		public UCBtnFillet()
		{
			InitializeComponent();
		}


		private void UCBtnFillet_MouseEnter(object sender, EventArgs e)
		{
			if (BtnMouseEnter != null) BtnMouseEnter(this, e);
		}

		private void lbl_Click(object sender, EventArgs e)
		{
			if (Click != null) Click(this, e);
		}

		private void UCBtnFillet_MouseLeave(object sender, EventArgs e)
		{
			if (BtnMouseLeave != null) BtnMouseLeave(this, e);
		}
	}
}
