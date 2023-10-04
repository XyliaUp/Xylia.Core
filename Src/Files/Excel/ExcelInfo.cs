using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Xylia.Workbook;

public sealed class ExcelInfo 
{
	#region Fields
	public readonly IWorkbook Workbook;

	public ISheet MainSheet;

	public ICellStyle style;

	public ExcelInfo(string SheetName = "数据")
	{
		this.Workbook = new XSSFWorkbook();

		//创建默认工作表
		CreateSheet(SheetName);

		//创建字体通用风格
		style = Workbook.CreateStyle();
	}
	#endregion

	#region 方法
	int ColumnIndex = 0;

	public ISheet CreateSheet(string sheetname)
	{
		ColumnIndex = 0;
		return this.MainSheet = Workbook.CreateSheet(sheetname);
	}

	public void SetColumn(string title, int width = 10, ISheet sheet = null)
	{
		sheet ??= MainSheet;
		sheet.SetColumnWidth(ColumnIndex, width * 256);

		var trow = sheet.GetRow(0);
		if (trow is null)
		{
			trow = sheet.CreateRow(0);
			trow.RowStyle = style;
		}
		trow.AddCell(title);


		ColumnIndex++;
	}


	public IRow CreateRow(int rownum = -1, ISheet sheet = null) => (sheet ?? MainSheet).CreateRow(rownum, this.style);




	public void Save(string Folder, string RelativePath, bool Relative = false, int RetryTime = 20)
	   => this.Save(Folder + @"\" + RelativePath, Relative, null, RetryTime);

	public void Save(string FilePath, bool ShowRelativePath = false, string RelativePrefix = null, int RetryTime = 20)
	{
		//for (int s = 0; s < this.Workbook.NumberOfSheets; s++)
		//{
		//	var sheet = this.Workbook.GetSheetAt(s);
		//	for (int c = 0; c < sheet.GetRow(0).LastCellNum; c++)
		//	{
		//		sheet.AutoSizeColumn(c);
		//		sheet.SetColumnWidth(c, (int)(1.2F * sheet.GetColumnWidth(c)));
		//	}
		//}


		//设置重试路径
		string RetryPath =
			System.IO.Path.GetDirectoryName(FilePath) +
			System.IO.Path.GetFileNameWithoutExtension(FilePath) + $" (#Time)" +
			System.IO.Path.GetExtension(FilePath);

		for (int i = 1; i <= RetryTime; i++)
		{
			try
			{
				if (i != 1) this.Workbook.Save(RetryPath.Replace("#Time", i.ToString()));
				else this.Workbook.Save(FilePath);

				//执行成功结束循环
				break;
			}
			catch (Exception ee)
			{
				//如果最后一次仍未保存成功
				if (i == RetryTime) throw;
				else Console.WriteLine($"正在尝试保存文件，但是{ee.Message} (第{i}次)");
			}
		}
	}
	#endregion


	#region Dispose
	private bool disposedValue;

	void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: 释放托管状态(托管对象)
				this.Workbook.Dispose();
			}

			// TODO: 释放未托管的资源(未托管的对象)并替代终结器
			// TODO: 将大型字段设置为 null
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	#endregion
}