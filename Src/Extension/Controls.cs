using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using HZH_Controls.Forms;

namespace Xylia.Extension
{
	public static class ControlEx
	{
		/// <summary>
		/// 调用事件
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="EventName"></param>
		/// <param name="e"></param>
		public static void CallEvent(this object obj, string EventName, EventArgs e = null)
		{
			//产生方法
			MethodInfo m = obj.GetType().GetMethod(EventName, ClassExtension.Flags);
			if (m is null) return;

			//调用方法  
			m.Invoke(obj, new object[1]
			{
				e is not null ? e : Type.GetType(m.GetParameters()[0].ParameterType.BaseType.FullName).GetProperty("Empty")
			});
		}

		public static void Remove<T>(this Control.ControlCollection collection) where T : Control =>
			collection.OfType<T>().ToList().ForEach(o => collection.Remove(o));


		public static void SetToolTip(this Control control, string Tip)
		{
			ToolTip tip = new()
			{
				AutoPopDelay = 5000,
				InitialDelay = 500,
				IsBalloon = true,
				ReshowDelay = 0,
			};


			tip.SetToolTip(control, Tip);
		}





		/// <summary>
		/// 将控件另存为图片
		/// </summary>
		/// <param name="Obj"></param>
		/// <param name="CopyRight"></param>
		/// <param name="Comparer"></param>
		/// <returns></returns>
		public static Bitmap DrawMeToBitmap(this Control Obj, bool CopyRight = true, IComparer<Control> Comparer = null)
		{
			DateTime dt = DateTime.Now;

			#region 初始化
			var Font = new Font("微软雅黑", 9.75F);

			List<Control> Controls = new List<Control>();
			foreach (Control item in Obj.Controls)
			{
				Controls.Add(item);
			}


			int w = 0, h = 0;
			foreach (var c in Controls.Where(c => c.Visible))
			{
				w = Math.Max(w, c.Right);
				h = Math.Max(h, c.Bottom);
			}

			if (CopyRight)
			{
				h += Font.Height + 2;
			}

			Bitmap bitmap = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(bitmap);
			#endregion

			#region 获取背景色
			var CurObj = Obj;
			Color BackColor = Color.Transparent;
			while (CurObj != null && BackColor == Color.Transparent)
			{
				//不要颠倒顺序
				BackColor = CurObj.BackColor;

				if (Obj.Parent is null) break;
				else CurObj = Obj.Parent;
			}
			#endregion


			using Brush brush = new SolidBrush(BackColor);
			g.FillRectangle(brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

			//绘制当前控件背景层
			if (Obj.BackgroundImage != null)
			{
				var TempImage = CurObj.BackgroundImage;
				g.DrawImage(TempImage, new Rectangle(0, 0, TempImage.Width, TempImage.Height));
			}

			//控件渲染层级排序
			Controls.Sort(Comparer ?? new ControlSort());
			foreach (var c in Controls.Where(c => c.Visible))
			{
				if (c.Width != 0 && c.Height != 0)
				{
					using (Bitmap bitmap2 = new Bitmap(c.Width, c.Height))
					{
						c.DrawToBitmap(bitmap2, c.ClientRectangle);
						g.DrawImage(bitmap2, new PointF(c.Location.X, c.Location.Y));
					}
				}
			}


			if (CopyRight)
			{
				//要求居中显示
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;

				g.DrawString("由剑灵预览工具生成截图  Powered by Xylia, 2022.", Font, new SolidBrush(Color.FromArgb(51, 204, 255)), bitmap.Width / 2, bitmap.Height - Font.Height - 1, sf);
			}

			return bitmap;
		}


		#region 剪切板
		public static void SetClipboard(this object sender, bool ShowTip = false)
		{
			if (sender is Control Ctl) SetClipboard(Ctl.Text, ShowTip);
			else SetClipboard(sender.ToString(), ShowTip);
		}

		public static void SetClipboard(this string text, bool ShowTip = false)
		{
			Clipboard.SetDataObject(text, true);
			if (ShowTip) FrmTips.ShowTipsSuccess("已粘贴至剪切板");
		}
		#endregion
	}


	public class ControlSort : IComparer<Control>
	{
		public int Compare(Control x, Control y)
		{
			if (x is PictureBox) return -1;
			else if (y is PictureBox) return 1;
			else return 0;
		}
	}
}