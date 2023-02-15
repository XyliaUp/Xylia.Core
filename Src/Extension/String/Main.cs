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
		/// 压缩文本
		/// </summary>
		/// <param name="Text"></param>
		/// <returns></returns>
		public static string StringZip(this string Text)
		{
			if (string.IsNullOrWhiteSpace(Text))
				throw new Exception("在压缩文本操作中，不允许传入空文本。");


			StringBuilder builder = new StringBuilder();

			char firstChar = Text[0];  //字符串中第一个字符                
			int count = 1;            //字符数量默认为1

			for (int i = 1; i < Text.Length; i++)
			{
				char s = Text[i];      //字符串中第2个字符

				if (firstChar == '0' && firstChar == s) count++; //存在多个0时
				else
				{
					if (count > 1)  //数量 >1 时
					{
						builder.Append($"[{count}]");
						count = 1;                //初始化
					}
					else builder.Append(firstChar);  //数量 ≤1 时
				}
				firstChar = s; //把第2个字符赋值给第一个字符
			}

			if (count > 1) builder.Append($"[{count}]");  //如果数量大于1，把count追加到StringBuilder中

			if (count <= 1 || firstChar != '0') builder.Append(firstChar);//最后把字符追加到StringBuilder中

			return builder.ToString();
		}

		/// <summary>
		/// 解压文本
		/// </summary>
		/// <param name="Cipher"></param>
		/// <returns></returns>
		public static string StringUnzip(this string Cipher)
		{
			if (string.IsNullOrWhiteSpace(Cipher))
				throw new Exception("在解压文本操作中，不允许传入空文本。");

			StringBuilder builder = new();
			for (int i = 0; i < Cipher.Length; i++)
			{
				char s = Cipher[i];   //字符串中第2个字符

				if (i + 1 != Cipher.Length && s == '[')
				{
					StringBuilder InsiderBuilder = new StringBuilder();

					int NextId = i + 1;
					char CurChar = Cipher[NextId];

					while (CurChar != ']')
					{
						InsiderBuilder.Append(CurChar);

						int NewId = ++NextId;

						if (Cipher.Length < NewId + 1) throw new Exception("压缩文本中缺失了后标(即 \"]\" 标识)。");
						CurChar = Cipher[NewId];
					}

					for (int f = 0; f < int.Parse(InsiderBuilder.ToString()); f++) builder.Append('0');
					InsiderBuilder.Clear();
					i = NextId;
				}
				else if (s == ']') throw new Exception("无效的压缩文本后标，请确认前标是否遗漏。");
				else builder.Append(s);
			}

			return builder.ToString().Replace(" ", null);
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