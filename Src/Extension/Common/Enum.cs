using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Xylia.Extension;

public static partial class EnumExtension
{
	#region Attribute
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

	public static bool ContainAttribute<T>(this object EnumItem) where T : System.Attribute => ContainAttribute(EnumItem, out T _);

	public static bool ContainAttribute<T>(this object EnumItemm, out T Target) where T : System.Attribute => (Target = GetAttribute<T>(EnumItemm)) != null;

	public static string GetDescription(this object EnumItem, bool ReturnNull = false)
		=> EnumItem.GetAttribute<DescriptionAttribute>()?.Description ?? (ReturnNull ? null : EnumItem.ToString());
	#endregion

	#region Cast
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
	/// <param name="value"></param>
	/// <param name="Extension"></param>
	/// <returns></returns>
	public static bool TryParseToEnum(this string EnumItem, Type type, out object? value, bool Extension)
	{
		value = default;
		if (string.IsNullOrWhiteSpace(EnumItem)) return false;

		bool flag = byte.TryParse(EnumItem, out var number);


		#region 扩展模式
		if (Extension)
		{
			if (EnumItem.Contains('-'))
				return Enum.TryParse(type, EnumItem.Replace("-", null), true, out value);

			// 对数值类型枚举支持
			if (flag && Enum.TryParse(type, "N" + EnumItem, true, out value))
				return true;
		}
		#endregion

		return Enum.TryParse(type, EnumItem, true, out value);
	}
	#endregion
}