using System.Runtime.InteropServices;

using Vanara.PInvoke;

namespace Xylia.Windows.Framework;

public class MouseHook
{
	[StructLayout(LayoutKind.Sequential)]
	public class MouseHookStruct
	{
		public POINT pt;
		public int hwnd;
		public int wHitTestCode;
		public int dwExtraInfo;
	}



	private Point point;
	private Point Point
	{
		get { return point; }
		set
		{
			if (point != value)
			{
				point = value;
				if (MouseMoveEvent != null)
				{
					var e = new MouseEventArgs(MouseButtons.Left, 0, point.X, point.Y, 0);
					MouseMoveEvent(this, e);
				}
			}
		}
	}
	private IntPtr hHook;
	public User32.HookProc hProc;

	public MouseHook()
	{
		this.Point = new Point();
	}

	public IntPtr SetHook()
	{
		hProc = new User32.HookProc(MouseHookProc);
		GCHandle.Alloc(hProc);

		hHook = User32.SetWindowsHookEx(User32.HookType.WH_MOUSE_LL, hProc, IntPtr.Zero, 0).DangerousGetHandle();
		return hHook;
	}

	public void UnHook()
	{
		User32.UnhookWindowsHookEx(hHook);
	}

	private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
	{
		var MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
		if (nCode < 0) return User32.CallNextHookEx(hHook, nCode, wParam, lParam);
		else
		{
			if (MouseClickEvent != null)
			{
				MouseButtons button = MouseButtons.None;
				int clickCount = 0;
				switch ((User32.WindowMessage)(int)wParam)
				{
					case User32.WindowMessage.WM_LBUTTONDOWN:
						button = MouseButtons.Left;
						clickCount = 1;
						break;
				}

				var e = new MouseEventArgs(button, clickCount, point.X, point.Y, 0);
				MouseClickEvent(this, e);
			}

			this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
			return User32.CallNextHookEx(hHook, nCode, wParam, lParam);
		}
	}


	public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
	public event MouseMoveHandler MouseMoveEvent;

	public delegate void MouseClickHandler(object sender, MouseEventArgs e);
	public event MouseClickHandler MouseClickEvent;
}