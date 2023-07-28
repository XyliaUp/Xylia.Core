using System.Runtime.InteropServices;

using Vanara.PInvoke;

namespace Xylia.Windows.Framework;
public class KeyboardHook : GlobalHook
{
	[StructLayout(LayoutKind.Sequential)]
	public class KeyboardHookStruct
	{
		public int vkCode;  //Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
		public int scanCode; // Specifies a hardware scan code for the key. 
		public int flags;  // Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
		public int time; // Specifies the time stamp for this message.
		public int dwExtraInfo; // Specifies extra information associated with the message. 
	}


	#region Events  
	public event KeyEventHandler KeyDown;
	public event KeyEventHandler KeyUp;
	public event KeyPressEventHandler KeyPress;
	#endregion

	#region Constructor  
	public KeyboardHook()
	{
		_hookType = User32.HookType.WH_KEYBOARD_LL;
	}
	#endregion

	#region Methods  
	protected override IntPtr HookCallbackProcedure(int nCode, IntPtr wParam, IntPtr lParam)
	{
		bool handled = false;

		if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null))
		{
			KeyboardHookStruct keyboardHookStruct =
				(KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

			// Is Control being held down?  
			bool control = ((User32.GetKeyState((int)User32.VK.VK_LCONTROL) & 0x80) != 0) ||
						   ((User32.GetKeyState((int)User32.VK.VK_RCONTROL) & 0x80) != 0);

			// Is Shift being held down?  
			bool shift = ((User32.GetKeyState((int)User32.VK.VK_LSHIFT) & 0x80) != 0) ||
						 ((User32.GetKeyState((int)User32.VK.VK_RSHIFT) & 0x80) != 0);

			// Is Alt being held down?  
			bool alt = ((User32.GetKeyState((int)User32.VK.VK_LMENU) & 0x80) != 0) ||
					   ((User32.GetKeyState((int)User32.VK.VK_RMENU) & 0x80) != 0);

			// Is CapsLock on?  
			bool capslock = (User32.GetKeyState((int)User32.VK.VK_CAPITAL) != 0);

			// Create event using keycode and control/shift/alt values found above  
			KeyEventArgs e = new KeyEventArgs(
				(Keys)(
					keyboardHookStruct.vkCode |
					(control ? (int)Keys.Control : 0) |
					(shift ? (int)Keys.Shift : 0) |
					(alt ? (int)Keys.Alt : 0)
					));


			// Handle KeyDown and KeyUp events  
			switch ((User32.WindowMessage)wParam.ToInt32())
			{
				case User32.WindowMessage.WM_KEYDOWN:
				case User32.WindowMessage.WM_SYSKEYDOWN:
					if (KeyDown != null)
					{
						KeyDown(this, e);
						handled = handled || e.Handled;
					}
					break;
				case User32.WindowMessage.WM_KEYUP:
				case User32.WindowMessage.WM_SYSKEYUP:
					if (KeyUp != null)
					{
						KeyUp(this, e);
						handled = handled || e.Handled;
					}
					break;

			}

			// Handle KeyPress event  
			if (wParam.ToInt32() == (int)User32.WindowMessage.WM_KEYDOWN && !handled && !e.SuppressKeyPress && KeyPress != null)
			{
				byte[] keyState = new byte[256];
				User32.GetKeyboardState(keyState);

				if (User32.ToAscii((uint)keyboardHookStruct.vkCode,
						  (uint)keyboardHookStruct.scanCode,
						  keyState,
						  out ushort inBuffer,
						  (uint)keyboardHookStruct.flags) == 1)
				{

					char key = (char)inBuffer;
					if ((capslock ^ shift) && Char.IsLetter(key))
						key = Char.ToUpper(key);
					KeyPressEventArgs e2 = new KeyPressEventArgs(key);
					KeyPress(this, e2);
					handled = handled || e.Handled;

				}
			}
		}

		if (handled) return new IntPtr(1);

		return User32.CallNextHookEx(_handleToHook.DangerousGetHandle(), nCode, wParam, lParam);
	}
	#endregion
}