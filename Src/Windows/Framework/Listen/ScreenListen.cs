using System;
using System.Drawing;
using System.Windows.Forms;
namespace Xylia.Windows.Framework
{
	public static partial class Screen
	{
		public static Bitmap GetWindow(IntPtr Handle)
		{
			IntPtr hscrdc = Win32API.GetWindowDC(Handle);
			Win32API.GetWindowRect(Handle, out RECT rect);

			IntPtr hbitmap = Win32API.CreateCompatibleBitmap(hscrdc, rect.right - rect.left, rect.bottom - rect.top);
			IntPtr hmemdc = Win32API.CreateCompatibleDC(hscrdc);

			Win32API.SelectObject(hmemdc, hbitmap);
			Win32API.PrintWindow(Handle, hmemdc, 0);

			Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
			Win32API.DeleteDC(hscrdc);
			Win32API.DeleteDC(hmemdc);
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
			//获取工作区左上角坐标信息
			Win32API.GetWindowRect(Handle, out RECT Rect);
			LocX -= Rect.left;
			LocY -= Rect.top;


			int v_Color = (int)Win32API.GetPixel(Win32API.GetWindowDC(Handle), LocX, LocY);
			int v_Red = v_Color & 0xff;
			int v_Green = (v_Color & 0xff00) / 256;
			int v_Blue = (v_Color & 0xff0000) / 65536;

			return Color.FromArgb(v_Red, v_Green, v_Blue);
		}
	}
}