using System;

namespace Xylia.Extension
{
	/// <summary>
	/// 时间间隔扩展方法
	/// </summary>
	public static class TimeSpanEx
	{
		#region	将毫秒转为时间戳
		/// <summary>
		/// 将毫秒转为时间戳
		/// </summary>
		/// <param name="MilliSeconds"></param>
		/// <returns></returns>
		public static TimeSpan MSToTimeSpan(this long MilliSeconds) => TimeSpan.FromMilliseconds(MilliSeconds);

		public static TimeSpan MSToTimeSpan(this string MilliSeconds) => MSToTimeSpan(MilliSeconds.ToLong());
		#endregion


		/// <summary>
		/// 时间间隔转为文本
		/// </summary>
		/// <param name="ts"></param>
		/// <returns></returns>
		public static string ToMyString(this TimeSpan ts)
		{
			string result = null;

			if (ts.Days > 0) result += ts.Days + "天";
			if (ts.Hours > 0) result += ts.Hours + "小时";
			if (ts.Minutes > 0) result += ts.Minutes + "分钟";
			if (ts.Seconds > 0) result += ts.Seconds + "秒";

			return result;
		}
	}
}
