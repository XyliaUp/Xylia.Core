using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Xylia.Windows.Framework
{
	public abstract class GlobalHook
	{
		[DllImport("user32")]
		public static extern int EnumWindows(CallBack x, int y);
		public delegate bool CallBack(int hwnd, int lParam);



		#region Private Variables  

		protected int _hookType;
		protected IntPtr _handleToHook;
		protected bool _isStarted;
		protected Win32API.HookProc _hookCallback;

		#endregion

		#region Properties  
		public bool IsStarted => _isStarted;
		#endregion

		#region Constructor  
		public GlobalHook()
		{
			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
		}

		#endregion


		#region Methods  
		public void Start()
		{
			if (!_isStarted && _hookType != 0)
			{

				// Make sure we keep a reference to this delegate!  
				// If not, GC randomly collects it, and a NullReference exception is thrown  
				_hookCallback = new Win32API.HookProc(HookCallbackProcedure);

				_handleToHook = Win32API.SetWindowsHookEx(
					_hookType,
					_hookCallback,
					 Win32API.GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName),
					0);

				//GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName
				//GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName)
				// Were we able to sucessfully start hook?  
				if (_handleToHook != IntPtr.Zero)
				{
					_isStarted = true;
				}

			}

		}

		public void Stop()
		{
			if (_isStarted)
			{

				Win32API.UnhookWindowsHookEx(_handleToHook);

				_isStarted = false;
			}
		}

		protected virtual IntPtr HookCallbackProcedure(int nCode, IntPtr wParam, IntPtr lParam)
		{

			// This method must be overriden by each extending hook  
			return IntPtr.Zero;
		}




		protected void Application_ApplicationExit(object sender, EventArgs e)
		{

			if (_isStarted)
			{
				Stop();
			}

		}

		#endregion
	}
}