namespace Xylia.Extension;
public static class TimeSpanEx
{
	public static TimeSpan MSToTimeSpan(this long MilliSeconds) => TimeSpan.FromMilliseconds(MilliSeconds);

	public static TimeSpan MSToTimeSpan(this string MilliSeconds) => MSToTimeSpan(MilliSeconds.ToLong());
}
