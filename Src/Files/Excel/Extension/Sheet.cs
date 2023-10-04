using System.Data;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Xylia.Workbook;

public static class SheetEx
{
	/// <summary>
	/// 保存为Excel文件 
	/// </summary>
	/// <param name="workbook"></param>
	/// <param name="fileName"></param>
	public static void Save(this IWorkbook workbook, string fileName)
	{
		MemoryStream stream = new MemoryStream();
		workbook.Write(stream, false);
		var buf = stream.ToArray();

		using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
		{
			fs.Write(buf, 0, buf.Length);
			fs.Flush();
			fs.Close();
		}
	}

	/// <summary>
	/// 创建风格
	/// </summary>
	/// <param name="Workbook"></param>
	/// <param name="Style"></param>
	/// <returns></returns>
	public static ICellStyle CreateStyle(this IWorkbook Workbook, IStyle Style = null)
	{
		if (Style is null) Style = new IStyle();


		ICellStyle CellStyle = Workbook.CreateCellStyle();
		CellStyle.Alignment = Style.HorizontalAlignment;
		CellStyle.VerticalAlignment = VerticalAlignment.Center;

		if (Style.Wrap) CellStyle.WrapText = true;

		IFont font = CreateFont(Workbook, Style.FontName, Style.FontHeight, Style.Bold);
		CellStyle.SetFont(font);

		return CellStyle;
	}

	public static IFont CreateFont(this IWorkbook workbook, string FontName = "微软雅黑", double FontHeight = 11.2, bool Bold = false)
	{
		IFont font = workbook.CreateFont();
		font.FontHeightInPoints = FontHeight;
		font.FontName = FontName;
		font.IsBold = Bold;

		return font;
	}


	private static int sheetCellNumMax = 12;


	public static string[] GetSheetName(this string filePath, out IWorkbook Workbook, bool IncludingEmpty = false)
	{
		#region 初始化
		var sheetNames = new List<string>();
		Workbook = null;

		//2007版本
		if (filePath.IndexOf(".xlsx") > 0) Workbook = new XSSFWorkbook(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

		//2003版本
		else if (filePath.IndexOf(".xls") > 0) Workbook = new HSSFWorkbook(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
		else return null;
		#endregion


		#region 获取信息不为空的表返回
		for (int i = 0; i < Workbook.NumberOfSheets; i++)
		{
			var sheet = Workbook.GetSheetAt(i);

			#region 判断内容是否为空 (只需在不包括空工作表时执行)
			bool hasContent = false;
			if (!IncludingEmpty)
			{
				var rowIndex = sheet.LastRowNum;
				for (int x = 0; x < rowIndex; i++)
				{
					var row = sheet.GetRow(x);

					//all cells are empty, so is a 'blank row' 
					if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

					hasContent = true;
					break;
				}
			}
			#endregion

			if (IncludingEmpty || hasContent) sheetNames.Add(sheet.SheetName);
		}

		return sheetNames.ToArray();
		#endregion
	}

	/// <summary>
	/// 根据表名获取表
	/// </summary>
	/// <param name="filePath"></param>
	/// <param name="sheetName"></param>
	/// <returns></returns>
	public static DataTable ExcelToDataTable(this string filePath, string sheetName)
	{
		string outMsg = "";
		var dt = new DataTable();
		string fileType = Path.GetExtension(filePath).ToLower();

		try
		{
			ISheet sheet = null;
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			if (fileType == ".xlsx")
			{
				//2007版
				XSSFWorkbook workbook = new(fs);
				sheet = workbook.GetSheet(sheetName);
				if (sheet != null)
				{
					dt = GetSheetDataTable(sheet, out outMsg);
				}
			}
			else if (fileType == ".xls")
			{
				//2003版
				HSSFWorkbook workbook = new HSSFWorkbook(fs);
				sheet = workbook.GetSheet(sheetName);
				if (sheet != null)
				{
					dt = GetSheetDataTable(sheet, out outMsg);
				}
			}


		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}


		return dt;
	}

	public static DataTable ExcelToDataTable(this IWorkbook Workbook, string sheetName)
	{
		string outMsg = "";
		var dt = new DataTable();

		try
		{
			ISheet sheet = null;

			//2007版
			if (Workbook is XSSFWorkbook XSSFWorkbook) sheet = XSSFWorkbook.GetSheet(sheetName);
			//2003版
			else if (Workbook is HSSFWorkbook HSSFWorkbook) sheet = HSSFWorkbook.GetSheet(sheetName);

			if (sheet != null) dt = GetSheetDataTable(sheet, out outMsg);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}


		return dt;
	}

	/// <summary>
	/// 获取sheet表对应的DataTable
	/// </summary>
	/// <param name="sheet">Excel工作表</param>
	/// <param name="strMsg"></param>
	/// <returns></returns>
	public static DataTable GetSheetDataTable(this ISheet sheet, out string strMsg)
	{
		strMsg = "";
		DataTable dt = new DataTable();
		string sheetName = sheet.SheetName;
		int startIndex = 0;// sheet.FirstRowNum;
		int lastIndex = sheet.LastRowNum;

		//最大列数
		int cellCount = 0;
		IRow maxRow = sheet.GetRow(0);
		for (int i = startIndex; i <= lastIndex; i++)
		{
			IRow row = sheet.GetRow(i);
			if (row != null && cellCount < row.LastCellNum)
			{
				cellCount = row.LastCellNum;
				maxRow = row;
			}
		}
		//列名设置
		try
		{
			//maxRow.LastCellNum = 12 // L
			for (int i = 0; i < sheetCellNumMax; i++)//maxRow.FirstCellNum
			{
				dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
				//DataColumn column = new DataColumn("Column" + (i + 1).ToString());
				//dt.Columns.Add(column);
			}
		}
		catch
		{
			strMsg = "工作表" + sheetName + "中无数据";
			return null;
		}
		//数据填充
		for (int i = startIndex; i <= lastIndex; i++)
		{
			IRow row = sheet.GetRow(i);
			DataRow drNew = dt.NewRow();
			if (row != null)
			{
				for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
				{
					if (row.GetCell(j) != null)
					{
						ICell cell = row.GetCell(j);
						switch (cell.CellType)
						{
							case CellType.Blank:
								drNew[j] = "";
								break;
							case CellType.Numeric:
								short format = cell.CellStyle.DataFormat;
								//对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
								if (format == 14 || format == 31 || format == 57 || format == 58)
									drNew[j] = cell.DateCellValue;
								else
									drNew[j] = cell.NumericCellValue;
								if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
									drNew[j] = cell.NumericCellValue.ToString("#0.00");
								break;
							case CellType.String:
								drNew[j] = cell.StringCellValue;
								break;
							case CellType.Formula:
								try
								{
									drNew[j] = cell.NumericCellValue;
									if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
										drNew[j] = cell.NumericCellValue.ToString("#0.00");
								}
								catch
								{
									try
									{
										drNew[j] = cell.StringCellValue;
									}
									catch { }
								}
								break;
							default:
								drNew[j] = cell.StringCellValue;
								break;
						}
					}
				}
			}
			dt.Rows.Add(drNew);
		}
		return dt;
	}
}