using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Xylia.Windows.Framework.Enum;

namespace Xylia.Windows.Framework
{
	public class MouseHook
	{

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
		public const int WH_MOUSE_LL = 14;
		public Win32API.HookProc hProc;

		public MouseHook()
		{
			this.Point = new Point();
		}

		public IntPtr SetHook()
		{
			hProc = new Win32API.HookProc(MouseHookProc);
			GCHandle.Alloc(hProc);

			hHook = Win32API.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
			return hHook;
		}

		public void UnHook()
		{
			Win32API.UnhookWindowsHookEx(hHook);
		}

		private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
			if (nCode < 0) return Win32API.CallNextHookEx(hHook, nCode, wParam, lParam);
			else
			{
				if (MouseClickEvent != null)
				{
					MouseButtons button = MouseButtons.None;
					int clickCount = 0;
					switch ((Int32)wParam)
					{
						case (int)wMsg.WM_LBUTTONDOWN:
							button = MouseButtons.Left;
							clickCount = 1;
							break;
					}

					var e = new MouseEventArgs(button, clickCount, point.X, point.Y, 0);
					MouseClickEvent(this, e);
				}

				this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
				return Win32API.CallNextHookEx(hHook, nCode, wParam, lParam);
			}
		}

		//委托+事件（把钩到的消息封装为事件，由调用者处理）
		public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
		public event MouseMoveHandler MouseMoveEvent;

		public delegate void MouseClickHandler(object sender, MouseEventArgs e);
		public event MouseClickHandler MouseClickEvent;
	}
}
