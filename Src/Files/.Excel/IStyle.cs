using NPOI.SS.UserModel;


namespace Xylia.Files.Excel
{
	/// <summary>
	/// 风格参数
	/// </summary>
	public class IStyle
	{
		public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

		public string FontName { get; set; } = "微软雅黑";

		public double FontHeight { get; set; } = 11.2F;

		public bool Bold { get; set; } = false;

		public bool Wrap { get; set; } = false;
	}
}
