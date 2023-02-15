using System;
using System.Drawing;

namespace Xylia.Extension
{
	/// <summary>
	/// 绘制扩展方法
	/// </summary>
	public static partial class GraphicsEnetension
	{
		#region 测量文本大小 
		public static SizeF MeasureString(this char Txt, Font Font) => MeasureString(Txt.ToString(), Font);

		public static SizeF MeasureString(this string Txt, Font Font)
		{
			using Graphics g = Graphics.FromHwnd(IntPtr.Zero);

			var sf = StringFormat.GenericTypographic;
			sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

			var Result = g.MeasureString(Txt, Font ?? SystemFonts.DefaultFont, PointF.Empty, sf);

			return Result;
		}
		#endregion
	}
}
