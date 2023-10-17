using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Xylia.Extension;
public static partial class EnumExtension
{
    #region Attribute
    public static T GetAttribute<T>(this object EnumItem) where T : Attribute
    {
        if (EnumItem is null) return default;

        //枚举处理
        else if (EnumItem is Enum @enum) return EnumItem.GetType().GetField(EnumItem.ToString()).GetAttribute<T>();

        //枚举反射处理
        else if (EnumItem is MemberInfo memberInfo)
        {
            var Attrs = Attribute.GetCustomAttributes(memberInfo, typeof(T));
            if (Attrs.Length == 0) return default;

            var tmp = Attrs.FirstOrDefault(a => a.GetType() == typeof(T));
            return tmp is null ? null : (T)tmp;
        }
        //其他处理
        else return EnumItem.GetType().GetCustomAttribute<T>();
    }

    public static bool ContainAttribute<T>(this object EnumItem) where T : Attribute => EnumItem.ContainAttribute(out T _);

    public static bool ContainAttribute<T>(this object EnumItemm, out T Target) where T : Attribute => (Target = EnumItemm.GetAttribute<T>()) != null;

    public static string GetDescription(this object EnumItem, bool ReturnNull = false)
        => EnumItem.GetAttribute<DescriptionAttribute>()?.Description ?? (ReturnNull ? null : EnumItem.ToString());
    #endregion


    #region Cast
    public static T ToEnum<T>(this string EnumItem, bool Extension = true) where T : Enum => EnumItem.TryParseToEnum(out T Val, Extension) ? Val : default;

    public static bool TryParseToEnum<T>(this string EnumItem, out T Value, bool Extension = true, bool HideError = false) where T : Enum
    {
        Value = default;

        if (EnumItem.TryParseToEnum(typeof(T), out var TempVal, Extension))
        {
            Value = (T)TempVal;
            return true;
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(EnumItem) && !HideError) Debug.WriteLine($"cast enum failed: {EnumItem} => {typeof(T)}"); 
            return false;
        }
    }

    public static bool TryParseToEnum(this string EnumItem, Type type, out object value, bool Extension)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(EnumItem)) return false;

        bool flag = byte.TryParse(EnumItem, out var number);

        #region extra
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