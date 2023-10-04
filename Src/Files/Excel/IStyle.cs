namespace Xylia.Workbook;
public class IStyle
{
	public NPOI.SS.UserModel.HorizontalAlignment HorizontalAlignment { get; set; } = NPOI.SS.UserModel.HorizontalAlignment.Center;

	public string FontName { get; set; } = "微软雅黑";

	public double FontHeight { get; set; } = 11.2F;

	public bool Bold { get; set; } = false;

	public bool Wrap { get; set; } = false;
}