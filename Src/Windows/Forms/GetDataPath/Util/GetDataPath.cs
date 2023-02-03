using System;
using System.IO;
using System.Windows.Forms;

using Xylia.bns.Read.Util;
using Xylia.CustomException;
using Xylia.Windows.CustomException;

namespace Xylia.bns.Read
{
	/// <summary>
	/// 获取数据文件路径
	/// </summary>
	public class GetDataPath : IDisposable
	{
		#region 构造
		public GetDataPath(string FolderPath, bool? is64 = null, bool? SelectBin = false)
		{
			this.Init(FolderPath, is64, SelectBin);
		}
		#endregion

		#region 字段
		/// <summary>
		/// 目标 local.dat 文件路径
		/// </summary>
		public string TargetLocal;

		/// <summary>
		/// 目标 xml.dat 文件路径
		/// </summary>
		public string TargetXml;

		public bool bit64;
		#endregion


		#region 方法
		/// <summary>
		/// 读取Dat文件路径
		/// </summary>
		/// <param name="FolderPath"></param>
		/// <param name="is64"></param>
		/// <param name="SelectBin"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ReadException"></exception>
		/// <exception cref="UserExitException"></exception>
		public void Init(string FolderPath, bool? is64, bool? SelectBin)
		{
			if (string.IsNullOrWhiteSpace(FolderPath))
			{
				throw new ArgumentNullException("游戏路径未设置或并不存在");

				//FolderPath = Ini.ReadValue("Game", "BnsFolder");
			}

			//用于指示是否成功的函数
			bool State = false;

			if (Directory.Exists(FolderPath))
			{
				//尝试获取dat路径
				var dataPathes = new DataPathes(FolderPath);

				var Xml_Pathes = dataPathes.GetFiles(DatType.xml, SelectBin);
				var Local_Pathes = dataPathes.GetFiles(DatType.local, SelectBin);

				//
				if (Xml_Pathes.Count == 0) throw new ReadException("xml文件位置获取失败，请确认当前目录是否为游戏目录。");
				if (Local_Pathes.Count == 0)
				{
					Console.WriteLine("local文件位置获取失败，请确认当前目录是否为游戏目录。");
				}

				if (Xml_Pathes.Count == 1 && Local_Pathes.Count == 1)
				{
					TargetXml = Xml_Pathes[0].FullName;
					TargetLocal = Local_Pathes[0].FullName;

					this.bit64 = Path.GetFileNameWithoutExtension(TargetXml).Contains("64");
					State = true;
				}
				else
				{
					using var select = new DataSelect(Xml_Pathes, Local_Pathes);
					if (select.ShowDialog() != DialogResult.OK)
						throw new UserExitException();


					this.TargetXml = select.XML_Select;
					this.TargetLocal = select.Local_Select;

					this.bit64 = select.Chk_64bit.Checked;

					State = true;
				}

			}
			else throw new ReadException("检测到未设置游戏根目录或不存在，请先设置");


			if (!State)
			{
				throw new ReadException("获取data文件路径失败");
			}
		}
		#endregion


		#region IDisposable Support
		private bool disposedValue = false; // 要检测冗余调用

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)。
				}

				// TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
				// TODO: 将大型字段设置为 null。

				disposedValue = true;
			}
		}

		void IDisposable.Dispose()
		{
			// 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
			Dispose(true);
			// TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
