using System;
using System.Text;

namespace Xylia.Extension
{
	public static partial class String
	{
		public static long ToLong(this string Self) => long.TryParse(Self, out var result) ? result : 0;

		public static int ToInt(this string Self) => int.TryParse(Self, out var result) ? result : 0;

		public static byte ToByte(this string Self) => byte.TryParse(Self, out var result) ? result : (byte)0;

		public static short ToShort(this string Self) => short.TryParse(Self, out var result) ? result : (byte)0;


		/// <summary>
		/// 转换为逻辑类型 
		/// </summary>
		/// <param name="Self"></param>
		/// <param name="Result"></param>
		/// <returns>分别返回转换是否成功与转换结果</returns>
		public static bool ToBool(this string Self, out bool Result)
		{
			Result = false;
			if (string.IsNullOrWhiteSpace(Self)) return false;

			//如果是数字类型，只判断是否为0
			else if (int.TryParse(Self, out int result))
			{
				Result = result != 0;
				return true;
			}


			Self = Self.ToLower();
			if (Self == "y" || Self == "true" || Self == "t")
			{
				Result = true;
				return true;
			}
			else if (Self == "n" || Self == "false" || Self == "f")
			{
				Result = false;
				return true;
			}
			else return false;
		}

		/// <summary>
		///  返回转换结果
		/// </summary>
		/// <param name="Self"></param>
		/// <returns>失败或者为False 都返回为False</returns>
		public static bool ToBool(this string Self) => ToBool(Self, out bool Result) && Result;

		/// <summary>
		/// 返回转换结果
		/// </summary>
		/// <param name="Self"></param>
		/// <returns>失败返回Null</returns>
		public static bool? ToBoolOrNull(this string Self)
		{
			if (string.IsNullOrWhiteSpace(Self)) return null;

			//转换为bool类型成功
			if (ToBool(Self, out bool Result)) return Result;

			//失败返回null
			else return null;
		}
	}
}
