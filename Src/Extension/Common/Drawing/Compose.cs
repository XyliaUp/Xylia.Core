using System.Drawing;
using System.Drawing.Drawing2D;

namespace Xylia.Extension;
public static partial class Drawing
{
	public static Bitmap Combine(this Bitmap imgBack, Bitmap img, DrawLocation drawLocation = default, bool Thumbnail = true, int xDeviation = 0, int yDeviation = 0)
	{
		if (imgBack is null) return null;

		#region 创建新位图
		var w = imgBack.Width;
		var h = imgBack.Height;

		Bitmap bmp = new Bitmap(w, h);
		Graphics g = Graphics.FromImage(bmp);

		g.Clear(Color.Transparent);
		g.DrawImage(imgBack, 0, 0, w, h);
		#endregion


		if (img is not null)
		{
			if (Thumbnail) img = img.Thumbnail(w, h);

			var iw = img.Width;
			var ih = img.Height;

			int LocationX = 0;
			int LocationY = 0;
			switch (drawLocation)
			{
				case DrawLocation.Centre:
					LocationX = w / 2 - iw / 2;
					LocationY = h / 2 - ih / 2;
					break;

				case DrawLocation.TopLeft:
					LocationX = 0;
					LocationY = 0;
					break;

				case DrawLocation.TopRight:
					LocationX = w - iw;
					LocationY = 0;
					break;

				case DrawLocation.BottomLeft:
					LocationX = 0;
					LocationY = h - ih;
					break;

				case DrawLocation.BottomRight:
					LocationX = w - iw;
					LocationY = h - ih;
					break;
			}

			g.DrawImage(img, LocationX + xDeviation, LocationY + yDeviation, iw, ih);
		}

		return bmp;
	}

	public static Bitmap Combine(this Bitmap imgBack, Bitmap img, DrawLocation drawLocation, Point Location, bool Thumbnail = true) => imgBack.Combine(img, drawLocation, Thumbnail, Location.X, Location.Y);


	public static Bitmap Thumbnail(this Bitmap bitmap, double ratio) => bitmap.Thumbnail((int)(bitmap.Width * ratio), (int)(bitmap.Height * ratio));

	public static Bitmap Thumbnail(this Bitmap bitmap, int width, int height)
	{
		lock (bitmap)
		{
			var w = bitmap.Width;
			var h = bitmap.Height;
			if (w == width && h == height) return bitmap;

			if (width == 0) width = height * w / h;
			if (height == 0) height = width * h / w;


			#region 绘制
			Bitmap outBmp = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(outBmp);
			g.Clear(Color.Transparent);

			// 设置画布的描绘质量   
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(bitmap, new Rectangle(0, 0, width, height + 1), 0, 0, w, h, GraphicsUnit.Pixel);
			g.Dispose();
			#endregion

			return outBmp;
		}
	}
}


public enum DrawLocation
{
	Centre, 
	TopLeft,    
	TopRight,   
	BottomLeft, 
	BottomRight, 
}