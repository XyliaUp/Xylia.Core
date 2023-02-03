using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Xylia.Extension
{
	public static partial class Drawing
	{
		public static Bitmap Combine(this Bitmap imgBack, Bitmap img, DrawLocation drawLocation = default, bool Thumbnail = true, int? xDeviation = null, int? yDeviation = null)
		{
			#region 创建新位图
			var w = imgBack.Width;
			var h = imgBack.Height;

			Bitmap bmp = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(bmp);

			g.Clear(Color.Transparent);
			g.DrawImage(imgBack, 0, 0, w, h);
			#endregion




			if (Thumbnail) img = img.Thumbnail(w, h);

			var iw = img.Width;
			var ih = img.Height;
			int LocationX = 0;
			int LocationY = 0;

			#region 根据附加类型计算位置
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
			#endregion

			g.DrawImage(img, LocationX + (xDeviation ?? 0), LocationY + (yDeviation ?? 0), iw, ih);
			return bmp;
		}

		public static Bitmap Combine(this Bitmap imgBack, Bitmap img, DrawLocation drawLocation, Point Location, bool Thumbnail = true) => imgBack.Combine(img, drawLocation, Thumbnail, Location.X, Location.Y);


		public static Bitmap Thumbnail(this Bitmap bitmap, double ratio) => bitmap.Thumbnail((int)(bitmap.Width * ratio), (int)(bitmap.Height * ratio));

		/// <summary>
		/// 图片缩放
		/// </summary>
		/// <param name="bmp">图片</param>
		/// <param name="width">目标宽度，若为0，表示宽度按比例缩放</param>
		/// <param name="height">目标长度，若为0，表示长度按比例缩放</param>
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
		Centre,  //中间
		TopLeft,     //左上角
		TopRight,    //右上角
		BottomLeft,  //左下角
		BottomRight, //右下角
	}
}