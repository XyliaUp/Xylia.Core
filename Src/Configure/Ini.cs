using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static Xylia.Configure.Api;
namespace Xylia.Configure
{
	public static class Ini
	{
		/// <summary>
		/// 修改文档读取位置
		/// </summary>
		public static string SetPublicPath
		{
			set
			{
				PathDefine.DefaultPath = value ?? PathDefine.CfgFolder + @"\Xylia.conf";
			}
		}

		/// <summary>
		/// 清除指定项
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Filepath"></param>
		public static void ClearSection(object Section, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			Directory.CreateDirectory(Path.GetDirectoryName(PathDefine.DefaultPath));
			WritePrivateProfileString(Section.ToString(), null, null, Filepath);
		}

		/// <summary>
		/// 清除指定键
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Key"></param>
		/// <param name="Filepath"></param>
		public static void ClearKey(object Section, object Key, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;
			Directory.CreateDirectory(Path.GetDirectoryName(PathDefine.DefaultPath));
			WritePrivateProfileString(Section.ToString(), Key.ToString(), null, Filepath);
		}


		/// <summary>
		/// 读取所有项
		/// </summary>
		/// <param name="Filepath"></param>
		/// <returns></returns>
		public static List<string> ReadSections(string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			List<string> result = new List<string>();
			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileString(null, null, null, buf, buf.Length, Filepath);
			int j = 0;

			for (int i = 0; i < len; i++)
				if (buf[i] == 0)
				{
					result.Add(Encoding.Default.GetString(buf, j, i - j));
					j = i + 1;
				}
			return result;
		}

		/// <summary>
		/// 读取所有键
		/// </summary>
		/// <param name="SectionName"></param>
		/// <param name="Filepath"></param>
		/// <returns></returns>
		public static List<string> ReadKeys(object SectionName, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			List<string> result = new List<string>();

			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileString(SectionName.ToString(), null, null, buf, buf.Length, Filepath);

			int j = 0;
			for (int i = 0; i < len; i++)
			{
				if (buf[i] == 0)
				{
					string Key = Encoding.Default.GetString(buf, j, i - j);

					result.Add(Key);

					j = i + 1;
				}
			}

			return result;
		}

		/// <summary>
		/// 获取键总数量
		/// </summary>
		/// <param name="SectionName"></param>
		/// <param name="Filepath"></param>
		/// <returns></returns>
		public static int GetKeyCount(object SectionName, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			List<string> result = new List<string>();

			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileString((string)SectionName, null, null, buf, buf.Length, Filepath);

			return buf.Count(b => b != 0);
		}




		/// <summary>
		/// 存储数值
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Key"></param>
		/// <param name="Value"></param>
		/// <param name="Filepath"></param>
		public static void WriteValue(object Section, object Key, object Value, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;
			Directory.CreateDirectory(Path.GetDirectoryName(PathDefine.DefaultPath));

			if (Key is null) Key = GetKeyCount(Section, Filepath) + 1;

			WritePrivateProfileString(Section.ToString(), Key.ToString(), Value?.ToString(), Filepath);
		}

		/// <summary>
		/// 存储数值
		/// </summary>
		/// <param name="SectionAndKey"></param>
		/// <param name="Value"></param>
		/// <param name="Filepath"></param>
		public static void WriteValue(KeyValuePair<object, object> SectionAndKey, object Value, string Filepath = null)
		{
			WriteValue(SectionAndKey.Key, SectionAndKey.Value, Value, Filepath);
		}




		#region 读取数据
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Key"></param>
		/// <param name="Filepath"></param>
		/// <returns></returns>
		public static string ReadValue(object Section, object Key, string Filepath = null)
		{
			//PathDefine.MoveToNew();
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			string ValInfo = GetProfileString(Section.ToString(), Key.ToString(), Filepath);

			if (string.IsNullOrWhiteSpace(ValInfo)) return null;
			else return ValInfo;
		}

		public static string ReadValue(KeyValuePair<object, object> SectionAndKey, string Filepath = null)
		{
			return ReadValue(SectionAndKey.Key, SectionAndKey.Value, Filepath);
		}

		/// <summary>
		/// 读取所有键与对应键值
		/// </summary>
		/// <param name="SectionName"></param>
		/// <param name="Filepath"></param>
		/// <returns></returns>
		public static Dictionary<string, string> ReadAllKeyWithVal(object SectionName, string Filepath = null)
		{
			if (Filepath == null) Filepath = PathDefine.DefaultPath;

			Dictionary<string, string> result = new Dictionary<string, string>();

			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileString((string)SectionName, null, null, buf, buf.Length, Filepath);

			int j = 0;
			for (int i = 0; i < len; i++)
			{
				if (buf[i] == 0)
				{
					string Key = Encoding.Default.GetString(buf, j, i - j);
					result.Add(Key, GetProfileString(SectionName, Key, Filepath));

					j = i + 1;
				}
			}

			return result;
		}
		#endregion

	}
}
