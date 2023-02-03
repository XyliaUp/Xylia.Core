using System.Runtime.InteropServices;
using System.Text;

namespace Xylia.Configure
{
	/// <summary>
	/// 接口
	/// </summary>
	public static class Api
	{
		#region Ini接口
		[DllImport("kernel32")]
		public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		/// <summary>
		///  获取配置文件字符串
		/// </summary>
		/// <param name="section"></param>
		/// <param name="key"></param>
		/// <param name="def"></param>
		/// <param name="retVal"></param>
		/// <param name="size"></param>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		[DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
		public static extern uint GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string FilePath);

		public static string GetProfileString(object SectionName, object key, object FilePath)
		{
			int Size = 2000;

			StringBuilder tmp = new StringBuilder(Size);
			GetPrivateProfileString(SectionName.ToString(), key is null ? null : key.ToString(), null, tmp, Size, FilePath.ToString());

			return tmp.ToString();
		}
		#endregion
	}
}
