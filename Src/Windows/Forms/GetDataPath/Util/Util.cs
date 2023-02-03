using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Xylia.CustomException;
using Xylia.Extension;

namespace Xylia.bns.Read.Util
{
	/// <summary>
	/// 类型
	/// </summary>
	public enum DatType
	{
		local64 = 0,
		local = 1,

		xml = 2,
		xml64 = 4,

		config = 8,
		config64 = 16,
	}

	/// <summary>
	/// dat文件集合
	/// </summary>
	public class DataPathes
	{
		#region 字段
		private readonly Dictionary<DatType, List<FileInfo>> DataPathMenu;
		#endregion

		#region 构造
		public DataPathes(string RootPath)
		{
			this.DataPathMenu = new Dictionary<DatType, List<FileInfo>>();
			this.InitPara(RootPath);
		}
		#endregion




		/// <summary>
		/// 初始化路径
		/// </summary>
		/// <param name="RootPath"></param>
		public void InitPara(string RootPath)
		{
			DataPathMenu.Clear();

			if (!Directory.Exists(RootPath))
				throw new Exception("由于设置的文件夹目录不存在，自动寻找Dat文件失败。");

			foreach (var FileItem in GetDatFile(RootPath))
			{
				string FileName = Path.GetFileNameWithoutExtension(FileItem.FullName).ToLower();
				DatType datType = DatType.xml;

				switch (FileName)
				{
					case "xml":
					case "datafile":
						datType = DatType.xml;
						break;

					case "xml64":
					case "datafile64":
						datType = DatType.xml64;
						break;

					case "config": datType = DatType.config; break;
					case "config64": datType = DatType.config; break;

					case "local":
					case "localfile":
						datType = DatType.local; break;

					case "local64":
					case "localfile64":
						datType = DatType.local64; break;

					default: continue;
				}

				//add
				if (DataPathMenu.ContainsKey(datType)) DataPathMenu[datType].Add(FileItem);
				else DataPathMenu.Add(datType, new List<FileInfo>() { FileItem });
			}
		}

		/// <summary>
		/// 获取dat路径
		/// </summary>
		/// <param name="Type"></param>
		/// <param name="AutoHandle">自动进行处理</param>
		/// <returns></returns>
		public List<FileInfo> GetDatPath(DatType Type, bool AutoHandle = true)
		{

			if (DataPathMenu.ContainsKey(Type)) return DataPathMenu[Type];
			else if (AutoHandle)
			{
				Trace.WriteLine("未查询到32位文件，开始自动搜索64位文件");

				//由于虚幻4版本不存在32位客户端，所以在搜索不到32位文件时直接使用64位文件
				if (Type == DatType.xml) return GetDatPath(DatType.xml64);
				else if (Type == DatType.local) return GetDatPath(DatType.local64);
				else if (Type == DatType.config) return GetDatPath(DatType.config64);
			}

			throw new ReadException(Type.ToString() + "不存在");
		}


		/// <summary>
		/// 获取文件
		/// </summary>
		/// <param name="Type"></param>
		/// <param name="SelectBin"></param>
		/// <returns></returns>
		/// <exception cref="ReadException"></exception>
		public FileCollection GetFiles(DatType Type, bool? SelectBin)
		{
			var Result = new FileCollection();

			if (Type == DatType.xml || Type == DatType.xml64)
			{
				if (DataPathMenu.ContainsKey(DatType.xml64)) Result.AddRange(DataPathMenu[DatType.xml64]);
				if (DataPathMenu.ContainsKey(DatType.xml)) Result.AddRange(DataPathMenu[DatType.xml]);
			}
			else if (Type == DatType.config || Type == DatType.config64)
			{
				if (DataPathMenu.ContainsKey(DatType.config64)) Result.AddRange(DataPathMenu[DatType.config64]);
				if (DataPathMenu.ContainsKey(DatType.config)) Result.AddRange(DataPathMenu[DatType.config]);
			}
			else if (Type == DatType.local || Type == DatType.local64)
			{
				if (DataPathMenu.ContainsKey(DatType.local64)) Result.AddRange(DataPathMenu[DatType.local64]);
				if (DataPathMenu.ContainsKey(DatType.local)) Result.AddRange(DataPathMenu[DatType.local]);
			}
			else throw new ReadException(Type.ToString() + "不存在");



			//True 只筛选bin, False 只筛选dat
			if (SelectBin == true) return new FileCollection(Result.Where(r => r.Extension == ".bin").ToList());
			if (SelectBin == false) return new FileCollection(Result.Where(r => r.Extension == ".dat").ToList());

			return Result;
		}







		/// <summary>
		/// 获取数据文件总集合
		/// </summary>
		/// <param name="Folder"></param>
		/// <returns></returns>
		public static IEnumerable<FileInfo> GetDatFile(string Folder)
		{
			List<FileInfo> Result = new();

			var DirInfo = new DirectoryInfo(Folder);
			Result.AddRange(DirInfo.GetFiles("*.dat", SearchOption.AllDirectories));
			Result.AddRange(DirInfo.GetFiles("*.bin", SearchOption.AllDirectories));

			return Result;
		}

		/// <summary>
		/// 获取对应区域
		/// </summary>
		/// <param name="Folder"></param>
		/// <returns></returns>
		public static string GetRegion(string Folder)
		{
			try
			{
				foreach (var item in GetDatFile(Folder))
				{
					if (item.Name.MyStartsWith("local"))
					{
						string SubPath = Path.GetDirectoryName(Path.GetDirectoryName(item.FullName));
						string MainPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(item.FullName)));

						return ReplaceRegion(SubPath.Replace(MainPath + @"\", ""));
					}
				}
			}
			catch
			{

			}

			return "未知或未能确定";
		}

		/// <summary>
		/// 替换区域性特征文本
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ReplaceRegion(string str)
		{
			List<KeyValuePair<string, string>> Region = new();
			Region.Add(new KeyValuePair<string, string>("CHINESES", "中国大陆"));
			Region.Add(new KeyValuePair<string, string>("CHINESET", "中国台湾"));
			Region.Add(new KeyValuePair<string, string>("Korean", "韩国"));

			Region.ForEach(r => str = Regex.Replace(str, r.Key, r.Value, RegexOptions.IgnoreCase));
			return str;
		}
	}
}