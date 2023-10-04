using NPOI.SS.UserModel;

namespace Xylia.Workbook;
public static class RowEx
{
	public static IRow CreateRow(this ISheet sheet, int rownum = -1, ICellStyle style = null)
	{
		if (rownum == -1)
			rownum = sheet.LastRowNum + 1;

		var row = sheet.CreateRow(rownum);
		row.RowStyle = style;
		return row;
	}


	public static ICell CreateCell(this IRow row, int column, object value, ICellStyle style = null)
	{
		var c = row.CreateCell(column);

		c.SetCellValue(value);
		c.CellStyle = style ?? row.RowStyle;

		return c;
	}

	public static void AddCell(this IRow row, object value, DisplayType displayType, ICellStyle style = null)
	{
		//判断是否是默认数值
		bool IsDefaultValue = false;
		if (value is int o && o == 0) IsDefaultValue = true;
		else if (value.ToString() == "None") IsDefaultValue = true;


		if (IsDefaultValue)
		{
			if (displayType == DisplayType.unDisplay) return;
			else if (displayType == DisplayType.HideValue)
			{
				row.AddCell("", style);
				return;
			}
		}

		row.AddCell(value, style);
	}

	public static void AddCell(this IRow row, object value, ICellStyle style = null)
	{
		var num = row.LastCellNum;
		if (num == -1) num = 0;

		row.CreateCell(num, value, style);
	}
}

/// <summary>
/// 默认值显示类型
/// </summary>
public enum DisplayType
{
	/// <summary>
	/// 按常用处理
	/// </summary>
	None,

	/// <summary>
	/// 不显示此单元格
	/// </summary>
	unDisplay,

	/// <summary>
	/// 隐藏数值
	/// </summary>
	HideValue,
}
