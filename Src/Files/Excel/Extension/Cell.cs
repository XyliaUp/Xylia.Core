using System;

using NPOI.SS.UserModel;

namespace Xylia.Workbook;

public static class CellEx
{
	/// <summary>
	/// 设置单元格值
	/// </summary>
	/// <param name="cell"></param>
	/// <param name="value"></param>
	public static void SetCellValue(this ICell cell, object value)
	{
		if (value is null) return;
		else if (value is double @double) cell.SetCellValue(@double);
		else if (value is bool @bool) cell.SetCellValue(@bool);
		else if (value is DateTime @datetime) cell.SetCellValue(@datetime);
		else cell.SetCellValue(value.ToString());
	}
}
