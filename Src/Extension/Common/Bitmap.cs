using System.Drawing;

using Xylia.Extension;

namespace Xylia.Extension
{
	public static class BitmapEx
	{
		public static Bitmap Clone(this Bitmap source, Rectangle rect) => source.Clone(rect, source.PixelFormat);
	}
}
