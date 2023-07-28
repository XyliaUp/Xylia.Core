namespace Xylia.Extension;
public static class TimeEx
{
	public static DateTime GetDateTime(this long TimeStamp, bool AccurateToMilliseconds = false)
		=> new DateTime(1970, 1, 1).AddTicks(TimeStamp * (AccurateToMilliseconds ? 10000 : 10000000));

	public static long GetTimeStamp(this DateTime DateTime, bool AccurateToMilliseconds = false)
		=> DateTime.ToUniversalTime().Ticks / (AccurateToMilliseconds ? 10000 : 10000000) - 62135596800;
}