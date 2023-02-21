using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xylia.Extension
{
	/// <summary>
	/// 对字符串进行扩展的类
	/// </summary>
	public static partial class String
	{
		#region 文本操作类方法
		public static string GetExtraParam(this string Alias, int? ExtraParam = null)
		{
			if (ExtraParam is not null)
			{
				if (Alias[^1] == '-') Alias += ExtraParam;
				else if (Alias[^1] == '*') Alias = Alias.Remove(Alias.Length - 1, 1) + ExtraParam;
			}

			return Alias;
		}

		/// <summary>
		/// 对比 <see langword="Self"/> 与 <see langword="Value"/> 是否相等
		/// </summary>
		/// <param name="self"></param>
		/// <param name="value"></param>
		/// <param name="EqualType">规则</param>
		/// <returns></returns>
		public static bool MyEquals(this string self, string value, EqualType EqualType = EqualType.IgnoreCase & EqualType.AllowNulls)
		{
			bool NonSelf = string.IsNullOrWhiteSpace(self);
			bool NonValue = string.IsNullOrWhiteSpace(value);

			if (NonSelf || NonValue)
			{
				//判断是否可以为空
				if ((EqualType & EqualType.AllowNulls) != EqualType.AllowNulls) return false;

				//则比对两者是否均为空
				else if (NonSelf && NonValue) return true;

				else return false;
			}


			if ((EqualType & EqualType.IgnoreCase) == EqualType.IgnoreCase)
			{
				return self.Equals(value, StringComparison.OrdinalIgnoreCase);
			}
			else return self.Equals(value);
		}

		public static bool MyStartsWith(this string self, string value) => self.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);

		public static bool MyEndsWith(this string self, string value) => self.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);

		public static bool MyContains(this string self, string value)
		{
			if (string.IsNullOrWhiteSpace(self)) return false;

			return self?.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		/// <summary>
		/// 移除前缀字符串
		/// </summary>
		/// <param name="self"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RemovePrefixString(this string self, string str)
		{
			if (string.IsNullOrWhiteSpace(self))
				return null;

			string strRegex = @"^(" + Regex.Escape(str) + ")";
			return Regex.Unescape(Regex.Replace(self, strRegex, ""));
		}

		/// <summary>
		/// 移除后缀字符串
		/// </summary>
		/// <param name="self"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string RemoveSuffixString(this string self, string str)
		{
			if (string.IsNullOrWhiteSpace(self))
				return null;

			string strRegex = @"(" + Regex.Escape(str) + ")" + "$";
			return Regex.Unescape(Regex.Replace(self, strRegex, ""));
		}

		public static string RemovePrefixString(this string self, char str) => RemovePrefixString(self, str.ToString());

		public static string RemoveSuffixString(this string self, char str) => RemoveSuffixString(self, str.ToString());


		/// <summary>
		/// 首字母大写
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public static string TitleCase(this string line)
		{
			StringBuilder sb = new();
			for (int i = 0; i < line.Length; i++)
			{
				//特定位置字符修改为大写
				if ((i == 0 || (line[i - 1] == '-')) && line[i] >= 'a' && line[i] <= 'z') sb.Append((char)(line[i] - 32));
				else if (line[i] != '-') sb.Append(line[i]);
			}

			return sb.ToString();
		}

		public static string TitleLowerCase(this string line)
		{
			StringBuilder sb = new();
			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] >= 'A' && line[i] <= 'Z')
				{
					if (i != 0) sb.Append('-');

				   sb.Append((char)(line[i] + 32));
				}
				else sb.Append(line[i]);
			}

			return sb.ToString();
		}



		public static string JudgeLineFeed(this string Self) => string.IsNullOrEmpty(Self) ? null : (Self + "\n");

		/// <summary>
		/// 返回判断换行后的文本
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="Type"></param>
		/// <returns></returns>
		public static string JudgeLineFeed(this string Self, JudegeLineType Type)
		{
			if (string.IsNullOrEmpty(Self)) return null;

			switch (Type)
			{
				case JudegeLineType.NoEmpty: return Self + "\n";
				case JudegeLineType.Signature: return Self + (Self.Contains('\n') ? null : "\n");

				default: throw new Exception("未知类型");
			}
		}
		#endregion
	}
}