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