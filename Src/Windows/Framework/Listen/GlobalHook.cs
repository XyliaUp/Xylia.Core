using System.Runtime.InteropServices;

using Vanara.PInvoke;



namespace Xylia.Windows.Framework;
public abstract class GlobalHook
{
	#region Private Variables  
	protected User32.HookType _hookType;
	protected User32.SafeHHOOK _handleToHook;
	protected bool _isStarted;
	protected User32.HookProc _hookCallback;
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
			_hookCallback = new User32.HookProc(HookCallbackProcedure);

			_handleToHook = User32.SetWindowsHookEx(
				_hookType,
				_hookCallback,
				 Kernel32.GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName),
				0);

			//GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName
			//GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName)
			// Were we able to sucessfully start hook?  
			if (!_handleToHook.IsInvalid)
			{
				_isStarted = true;
			}
		}
	}

	public void Stop()
	{
		if (_isStarted)
		{
			User32.UnhookWindowsHookEx(_handleToHook.DangerousGetHandle());
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