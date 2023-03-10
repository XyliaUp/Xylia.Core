using System.Text.RegularExpressions;

namespace Xylia.Extension
{
	public static partial class String
	{
		/// <summary>
		/// 匹配正则表达式
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="pattern"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static bool RegexMatch(this string Self, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
		{
			return new Regex(pattern, options).Match(Self).Success;
		}

		/// <summary>
		/// 匹配正则表达式并返回结果
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="pattern"></param>
		/// <param name="result"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static bool RegexMatch(this string Self, string pattern, out GroupCollection result, RegexOptions options = RegexOptions.IgnoreCase)
		{
			var match = new Regex(pattern, options).Match(Self);
			bool Success = match.Success;

			if (Success) result = match.Groups;
			else result = null;

			return Success;
		}

		/// <summary>
		/// 匹配正则表达式并返回首个结果
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="pattern"></param>
		/// <param name="FirstResult"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static bool RegexMatch(this string Self, string pattern, out string FirstResult, RegexOptions options = RegexOptions.IgnoreCase)
		{
			if (RegexMatch(Self, pattern, out GroupCollection Result, options))
			{
				FirstResult = Result[0].Value;
				return true;
			}
			else
			{
				FirstResult = null;
				return false;
			}
		}

		/// <summary>
		/// 正则替换
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="pattern"></param>
		/// <param name="Replacement"></param>
		/// <returns></returns>
		public static string RegexReplace(this string Self, string pattern, string Replacement = "")
		{
			return new Regex(pattern).Replace(Self, Replacement);
		}
	}
}
