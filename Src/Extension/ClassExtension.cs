using System.Reflection;

namespace Xylia.Extension;
public static partial class ClassExtension
{
    public static BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;


    public static MemberInfo GetInfo<T>(this T Case, object Name, bool IgnoreCase = false)
    {
        #region init
        if (Name is null) return null;
        var _Name = Name.ToString();

        BindingFlags MyFlags = Flags;
        if (IgnoreCase) MyFlags |= BindingFlags.IgnoreCase;    //判断是否需要忽略大小写

        var type = Case.GetType();
        #endregion

        var Members = type.GetMember(_Name.Replace("-", null), MyFlags);
        if (Members.Length == 0) Members = type.GetMember(_Name.Replace("-", "_"), MyFlags);

        return Members.Where(m => m is FieldInfo || m is PropertyInfo).FirstOrDefault();
    }

    public static Type GetMemberType(this MemberInfo MemberInfo)
    {
        if (MemberInfo is PropertyInfo property) return property.PropertyType;
        else if (MemberInfo is FieldInfo field) return field.FieldType;
        else throw new InvalidDataException();
    }

    public static object GetValue<T>(this T Case, object Name, bool IgnoreCase = false) => Case.GetInfo(Name, IgnoreCase)?.GetValue(Case);

    public static object GetValue(this MemberInfo member, object obj = null)
    {
        if (member is FieldInfo field) return field.GetValue(obj);
        else if (member is PropertyInfo property) return property.GetValue(obj, null);
        else throw new InvalidDataException();
    }

    public static void SetValue<T>(this MemberInfo member, T Case, object Value)
    {
        if (member is PropertyInfo property) property.SetValue(Case, Value);
        else if (member is FieldInfo field) field.SetValue(Case, Value);
        else throw new InvalidDataException();
    }
}