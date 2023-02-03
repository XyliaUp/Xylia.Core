using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xylia.bns.Read
{
	/// <summary>
	/// 文件集合
	/// </summary>
	public class FileCollection	: IEnumerable<FileInfo>
	{
		#region 构造
		public FileCollection(List<FileInfo> FileInfos)
		{
			if (FileInfos is null) throw new NullReferenceException();

			this.FileInfos = FileInfos;
		}

		public FileCollection()
		{
			this.FileInfos = new List<FileInfo>();
		}
		#endregion

		#region 字段
		public List<FileInfo> FileInfos;

		public int Count => this.FileInfos.Count;

		/// <summary>
		/// 存在64位文件
		/// </summary>
		public bool Has64bit => this.FileInfos.Find(f => Judge64Bit(f)) != null;

		public bool Has32bit => this.FileInfos.Find(f => !Judge64Bit(f)) != null;
		#endregion

		public static bool Judge64Bit(FileInfo FileInfo) => Path.GetFileNameWithoutExtension(FileInfo.Name).Contains("64");




		#region 方法
		/// <summary>
		/// 获取文件
		/// </summary>
		/// <param name="is64"></param>
		/// <returns></returns>
		public IEnumerable<FileInfo> GetFiles(bool? is64 = null)
		{
			if (is64 is null) return FileInfos;
			else if(is64.Value) return FileInfos.Where(f => Judge64Bit(f));
			else return FileInfos.Where(f => !Judge64Bit(f));
		}


		public void AddRange(IEnumerable<FileInfo> FileInfo)
		{
			this.FileInfos.AddRange(FileInfo);
		}

		public void Add(FileInfo FileInfo)
		{
			this.FileInfos.Add(FileInfo);
		}

		public FileInfo this[int Idx]
		{
			get => FileInfos[Idx];
		}
		#endregion

		#region 接口方法
		public IEnumerator<FileInfo> GetEnumerator()
		{
			foreach (var info in this.FileInfos) yield return info;

			//结束迭代
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}