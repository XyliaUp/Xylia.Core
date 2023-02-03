using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Xylia.Extension
{
	/// <summary>
	/// 枚举扩展方法
	/// </summary>
	public static partial class EnumExtension
	{
		#region 获取附加说明信息
		/// <summary>
		/// 获得附加属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="EnumItem"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(this object EnumItem) where T : System.Attribute
		{
			if (EnumItem is null) return default;

			//枚举处理
			else if (EnumItem is Enum @enum) return EnumItem.GetType().GetField(EnumItem.ToString()).GetAttribute<T>();

			//枚举反射处理
			//注意使用系统方法会出现继承类型错误
			else if (EnumItem is MemberInfo memberInfo)
			{
				var Attrs = System.Attribute.GetCustomAttributes(memberInfo, typeof(T));
				if (Attrs.Length == 0) return default;

				var tmp = Attrs.FirstOrDefault(a => a.GetType() == typeof(T));
				return tmp is null ? null : (T)tmp;
			}
			//其他处理
			else return EnumItem.GetType().GetCustomAttribute<T>();
		}

		/// <summary>
		/// 包含属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="EnumItem"></param>
		/// <returns></returns>
		public static bool ContainAttribute<T>(this object EnumItem) where T : System.Attribute => ContainAttribute(EnumItem, out T _);

		/// <summary>
		/// 包含属性并返回对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="EnumItemm"></param>
		/// <param name="Target"></param>
		/// <returns></returns>
		public static bool ContainAttribute<T>(this object EnumItemm, out T Target) where T : System.Attribute => (Target = GetAttribute<T>(EnumItemm)) != null;

		/// <summary>
		/// 获得描述属性
		/// </summary>
		/// <param name="EnumItem"></param>
		/// <param name="ReturnNull">错误时返回空文本或原信息</param>
		/// <returns></returns>
		public static string GetDescription(this object EnumItem, bool ReturnNull = false)
			=> EnumItem.GetAttribute<DescriptionAttribute>()?.Description ?? (ReturnNull ? null : EnumItem.ToString());
		#endregion


		#region 转为枚举对象
		/// <summary>
		/// 在指定<see langword="枚举类"/>的情况下，将枚举值文本转换为<see cref="Enum" />对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="EnumItem"></param>
		/// <param name="Extension"></param>
		/// <returns></returns>
		public static T ToEnum<T>(this string EnumItem, bool Extension = true) where T : Enum => EnumItem.TryParseToEnum(out T Val, Extension) ? Val : default;


		/// <summary>
		/// 在指定<see langword="枚举类"/>的情况下，将枚举值文本转换为<see cref="Enum" />对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="EnumItem"></param>
		/// <param name="Value"></param>
		/// <param name="Extension"></param>
		/// <param name="HideError"></param>
		/// <returns></returns>
		public static bool TryParseToEnum<T>(this string EnumItem, out T Value, bool Extension = true, bool HideError = false) where T : Enum
		{
			Value = default;

			if (TryParseToEnum(EnumItem, typeof(T), out var TempVal, Extension))
			{
				Value = (T)TempVal;
				return true;
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(EnumItem) && !HideError) Debug.WriteLine($"枚举转换失败: { EnumItem } => { typeof(T) }");   //提示DEBUG消息
				return false;
			}
		}

		/// <summary>
		/// 在指定<see langword="枚举类型"/>的情况下，将枚举值文本转换为<see cref="Enum" />对象
		/// </summary>
		/// <param name="EnumItem"></param>
		/// <param name="type"></param>
		/// <param name="Value"></param>
		/// <param name="Extension"></param>
		/// <returns></returns>
		public static bool TryParseToEnum(this string EnumItem, Type type, out object Value, bool Extension)
		{
			Value = default;
			if (string.IsNullOrWhiteSpace(EnumItem)) return false;

			#region 扩展模式
			if (Extension)
			{
				if (EnumItem.Contains('-'))
				{
					bool Status = false;

					object Result = default;
					if (!Status) Status = EnumItem.Replace("-", null).TryParseToEnum(type, out Result);
					if (!Status) Status = EnumItem.Replace("_", null).TryParseToEnum(type, out Result);
					if (!Status) Status = EnumItem.Replace("-", "_").TryParseToEnum(type, out Result);

					Value = Result;
					return Status;
				}

				//对数值类型枚举支持
				if (byte.TryParse(EnumItem, out _) && ("N" + EnumItem).TryParseToEnum(type, out Value)) return true;
			}
			#endregion

			return EnumItem.TryParseToEnum(type, out Value);
		}

		public static bool TryParseToEnum(this string EnumItem, Type type, out object Value)
		{
			try
			{
				Value = Enum.Parse(type, EnumItem, true);
				return true;
			}
			catch
			{
				Value = default;
				return false;
			}
		}
		#endregion
	}
}