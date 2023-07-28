using System.Drawing.Imaging;

namespace Xylia.Extension;

public static partial class Drawing
{
	public static Bitmap Clone(this Bitmap source, Rectangle rect) => source.Clone(rect, source.PixelFormat);

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

	public static Bitmap ChangeColor(this Bitmap bitmap, Color Target, Color Original)
	{
		bitmap = new Bitmap(bitmap);

		for (int i = 0; i < bitmap.Width; i++)
			for (int j = 0; j < bitmap.Height; j++)
				if (bitmap.GetPixel(i, j) != Original) bitmap.SetPixel(i, j, Target);

		return bitmap;
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