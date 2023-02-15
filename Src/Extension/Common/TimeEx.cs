using System;

namespace Xylia.Extension
{
	/// <summary>
	/// 时间类扩展方法
	/// </summary>
	public static class TimeEx
	{
		#region 剑灵的时间戳比Unix时间戳少一小时
		/// <summary>
		/// 从时间格式文本返回 BNS时间戳
		/// </summary>
		/// <returns>对应的Unix时间戳</returns>
		public static long GetBNSTimeStamp(this string TimeStr) => Convert.ToDateTime(TimeStr).GetTimeStamp() - 3600;

		public static DateTime GetBNSTime(this long TimeStamp) => (TimeStamp + 3600).GetDateTime();
		#endregion



		#region DateTime
		/// <summary>
		/// 时间戳反转为时间
		/// </summary>
		/// <param name="TimeStamp">时间戳</param>
		/// <param name="AccurateToMilliseconds">是否精确到毫秒</param>
		/// <returns>返回一个日期时间</returns>
		public static DateTime GetDateTime(this long TimeStamp, bool AccurateToMilliseconds = false)
			=> new DateTime(1970, 1, 1).ToLocalTime().AddTicks(TimeStamp * (AccurateToMilliseconds ? 10000 : 10000000));

		public static long GetTimeStamp(this DateTime DateTime, bool AccurateToMilliseconds = false)
			=> DateTime.ToUniversalTime().Ticks / (AccurateToMilliseconds ? 10000 : 10000000) - 62135596800;


		public static string GetTimeStr(this DateTime DateTime) => DateTime.ToLocalTime().ToString().Replace("/", "-");
		#endregion
	}
}