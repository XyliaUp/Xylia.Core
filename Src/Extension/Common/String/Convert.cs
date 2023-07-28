namespace Xylia.Extension;
public static partial class String
{
	public static long ToLong(this string s) => long.TryParse(s, out var result) ? result : 0;

	public static int ToInt(this string s) => int.TryParse(s, out var result) ? result : 0;

	public static byte ToByte(this string s) => byte.TryParse(s, out var result) ? result : (byte)0;

	public static short ToShort(this string s) => short.TryParse(s, out var result) ? result : (short)0;

	public static float ToFloat(this string s) => float.TryParse(s, out var result) ? result : 0;

	public static double ToDouble(this string s) => double.TryParse(s, out var result) ? result : 0;



	public static bool ToBool(this string s, out bool Result)
	{
		Result = false;
		if (string.IsNullOrWhiteSpace(s)) return false;
		else if (int.TryParse(s, out int result))
		{
			Result = result != 0;
			return true;
		}


		s = s.ToLower();
		if (s == "y" || s == "true" || s == "t")
		{
			Result = true;
			return true;
		}
		else if (s == "n" || s == "false" || s == "f")
		{
			Result = false;
			return true;
		}
		else return false;
	}

	public static bool ToBool(this string s) => ToBool(s, out bool Result) && Result;

	public static bool? ToBoolOrNull(this string s)
	{
		if (string.IsNullOrWhiteSpace(s)) return null;

		if (ToBool(s, out bool Result)) return Result;
		else return null;
	}
}