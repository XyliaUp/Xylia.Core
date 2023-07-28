using System.Reflection;

using Xylia.Extension.Class;

namespace Xylia.Extension;
public static class ControlEx
{
	public static void CallEvent(this object obj, string EventName, EventArgs e = null)
	{
		MethodInfo m = obj.GetType().GetMethod(EventName, ClassExtension.Flags);
		if (m is null) return;

		m.Invoke(obj, new object[1]
		{
			e is not null ? e : Type.GetType(m.GetParameters()[0].ParameterType.BaseType.FullName).GetProperty("Empty")
		});
	}

	public static void Remove<T>(this Control.ControlCollection collection) where T : Control =>
		collection.OfType<T>().ForEach(collection.Remove);


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
				using Bitmap bitmap2 = new Bitmap(c.Width, c.Height);
				c.DrawToBitmap(bitmap2, c.ClientRectangle);
				g.DrawImage(bitmap2, new PointF(c.Location.X, c.Location.Y));
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


	#region	ChangeView
	public static void ChangeView<T>(this Control ParentControl) where T : Control, new()
	{
		try
		{
			ParentControl.Controls.Clear();

			T find = new T();
			find.Parent = ParentControl;
			find.Dock = DockStyle.Fill;
			find.BringToFront();
		}
		catch
		{

		}
	}

	public static void ChangeView(this Control ParentControl, Control T)
	{
		try
		{
			ParentControl.Controls.Clear();

			T.Parent = ParentControl;
			T.Dock = DockStyle.Fill;
			T.BringToFront();
		}
		catch
		{

		}
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