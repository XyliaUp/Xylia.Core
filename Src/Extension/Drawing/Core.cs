using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Xylia.Extension
{
	public static partial class Drawing
	{
		/// <summary>
		/// 将非透明区域全部改变为指定颜色
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="Target"></param>
		/// <returns></returns>
		public static Bitmap ChangeColor(this Bitmap bitmap, Color Target)
		{
			bitmap = new Bitmap(bitmap);

			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					var PointColor = bitmap.GetPixel(i, j);
					if (PointColor.A != 0) bitmap.SetPixel(i, j, Target);
				}
			}

			return bitmap;
		}

		/// <summary>
		/// 将指定的原颜色改变为另一种颜色
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="Target"></param>
		/// <param name="Original"></param>
		/// <returns></returns>
		public static Bitmap ChangeColor(this Bitmap bitmap, Color Target, Color Original)
		{
			bitmap = new Bitmap(bitmap);

			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
					if (bitmap.GetPixel(i, j) != Original) bitmap.SetPixel(i, j, Target);

			return bitmap;
		}


		/// <summary>
		/// 返回物品品质颜色
		/// </summary>
		/// <param name="Grade"></param>
		/// <returns></returns>
		public static Color ItemGrade(this byte Grade)
		{
			switch (Grade)
			{
				case 1: return Color.FromArgb(0x6c, 0x6c, 0x6c);
				case 2: return Color.FromArgb(0xff, 0xff, 0xff);
				case 3: return Color.FromArgb(0x58, 0xff, 0x77);
				case 4: return Color.FromArgb(0x46, 0xbe, 0xe1);
				case 5: return Color.FromArgb(0xd7, 0x39, 0xff);
				case 6: return Color.FromArgb(0xf1, 0xb2, 0x48);
				case 7: return Color.FromArgb(0xff, 0x77, 0x0a);
				case 8: return Color.FromArgb(0xff, 0x00, 0x84);

				default: return Color.White;
			}
		}


		public static void SaveDialog(this Bitmap Icon, string DefaultFilePath, string TitleName = null)
		{
			SaveFileDialog SaveDialog = new()
			{
				FileName = DefaultFilePath,
				Filter = "PNG格式|*.png|GIF格式|*.gif|JPEG格式|*.jpg|位图格式|*.bmp|ICO格式|*.ico",
				Title = TitleName
			};
			if (SaveDialog.ShowDialog() != DialogResult.OK) return;


			ImageFormat format = ImageFormat.Png;
			switch (SaveDialog.DefaultExt)
			{
				case ".png": format = ImageFormat.Png; break;
				case ".gif": format = ImageFormat.Gif; break;
				case ".jpg": format = ImageFormat.Jpeg; break;
				case ".bmp": format = ImageFormat.Bmp; break;
				case ".ico": format = ImageFormat.Icon; break;
			}

			Icon.Save(SaveDialog.FileName, format);
		}
	}
}