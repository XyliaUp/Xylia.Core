using System.Runtime.InteropServices;

using Vanara.PInvoke;

namespace Xylia.Windows.Framework;
public static partial class Screen
{
	public static Bitmap GetWindow(IntPtr Handle)
	{
		var hscrdc = User32.GetWindowDC(Handle);
		User32.GetWindowRect(Handle, out RECT rect);

		var hbitmap = Gdi32.CreateCompatibleBitmap(hscrdc, rect.right - rect.left, rect.bottom - rect.top);
		var hmemdc = Gdi32.CreateCompatibleDC(hscrdc);

		Gdi32.SelectObject(hmemdc, hbitmap);
		User32.PrintWindow(Handle, hmemdc.DangerousGetHandle(), 0);

		Bitmap bmp = Bitmap.FromHbitmap(hbitmap.DangerousGetHandle());
		Gdi32.DeleteDC(hscrdc);
		Gdi32.DeleteDC(hmemdc);
		return bmp;
	}

	/// <summary>
	/// 获得鼠标所在位置颜色
	/// </summary>
	/// <param name="Handle"></param>
	/// <returns></returns>
	public static Color GetMousePointColor(IntPtr Handle)
	{
		return GetPointColor(Handle, Control.MousePosition.X, Control.MousePosition.Y);
	}

	public static Color GetPointColor(IntPtr Handle, int LocX, int LocY)
	{
		User32.GetWindowRect(Handle, out RECT Rect);
		LocX -= Rect.left;
		LocY -= Rect.top;


		int v_Color = (int)GetPixel(User32.GetWindowDC(Handle).DangerousGetHandle(), LocX, LocY);
		int v_Red = v_Color & 0xff;
		int v_Green = (v_Color & 0xff00) / 256;
		int v_Blue = (v_Color & 0xff0000) / 65536;

		return Color.FromArgb(v_Red, v_Green, v_Blue);
	}


	[DllImport("gdi32.dll")]
	static public extern uint GetPixel(IntPtr hDC, int XPos, int YPos);
}