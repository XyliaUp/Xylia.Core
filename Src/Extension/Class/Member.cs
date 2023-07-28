using System.Diagnostics;
using System.Reflection;

namespace Xylia.Extension.Class;
public static partial class ClassExtension
{
	public static BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;


	public static MemberInfo GetInfo<T>(this T Case, object Name, bool IgnoreCase = false)
	{
		#region 初始化
		if (Name is null) return null;
		var _Name = Name.ToString();

		BindingFlags MyFlags = Flags;
		if (IgnoreCase) MyFlags |= BindingFlags.IgnoreCase;    //判断是否需要忽略大小写

		var type = Case.GetType();
		#endregion

		#region 获取对象
		var Members = type.GetMember(_Name.Replace("-", null), MyFlags);
		if (Members.Length == 0) Members = type.GetMember(_Name.Replace("-", "_"), MyFlags);

		return Members.Where(m => m is FieldInfo || m is PropertyInfo).FirstOrDefault();
		#endregion
	}

	public static object GetValue<T>(this T Case, object Name, bool IgnoreCase = false) => Case.GetInfo(Name, IgnoreCase)?.GetValue(Case);

	public static object GetValue(this MemberInfo member, object obj = null)
	{
		// 注意调用函数与本函数名称一致，给出的参数错误会导致异常
		if (member is FieldInfo field) return field.GetValue(obj);
		else if (member is PropertyInfo property) return property.GetValue(obj, null);
		else throw new ArgumentException(member + "是未支持的对象");
	}

	public static void SetValue<T>(this MemberInfo Info, T Case, object Value)
	{
		if (Info is PropertyInfo property) property.SetValue(Case, Value);
		else if (Info is FieldInfo field) field.SetValue(Case, Value);

		else throw new Exception($"[SetValue] 参数错误 ,{Info.GetType()}");
	}


	public static bool SetMember<T>(this T Case, string Name, string Value, bool IgnoreCase = false, ValueCreator convetor = null)
	{
		//判断是否存在目标对象
		var TarMember = Case.GetInfo(Name, IgnoreCase);
		if (TarMember != null) return Case.SetMember(TarMember, Value, convetor);

		#region 无效处理
		//string Msg = $"不存在字段或属性 { Case.GetType().Name } -> { Name }";

		//if (IgnoreError == false) throw new Exception(Msg + $" ({ Value })");
		//else if (IgnoreError == true)
		//{
		//	//如果使用相同的消息仅显示一次设置
		//	if (UseUniqueMsg)
		//	{
		//		if (!CacheMsg.Contains(Msg)) CacheMsg.Add(Msg);
		//		else return false;
		//	}

		//	Debug.WriteLine("[debug] " + Msg + $" ({ Value })");
		//}

		return false;
		#endregion
	}

	public static bool SetMember<T>(this T Case, MemberInfo Member, string Value, ValueCreator convetor = null)
	{
		if (Member is PropertyInfo PInfo && !PInfo.CanWrite) return false;
		else if (Value is null)
		{
			Member.SetValue(Case, null);
			return true;
		}

		convetor ??= new();
		var MemberType = Member.GetMemberType();

		try
		{
			if (MemberType.IsEnum)
			{
				bool Status = Value.TryParseToEnum(MemberType, out var result, true);
				if (!Status) throw new FormatException($"不存在枚举值 {Value}");

				Member.SetValue(Case, result);
			}
			else if (MemberType.IsArray)
			{
				var elementType = MemberType.GetElementType();
				var subs = string.IsNullOrEmpty(Value) ?
					Array.Empty<string>() :
					Value.Trim(';').Split(';');

				Array myArray = Array.CreateInstance(elementType, subs.Length);
				for (int i = 0; i < subs.Length; ++i)
				{
					myArray.SetValue(convetor.Construct(elementType, subs[i]), i);
				}

				Member.SetValue(Case, myArray);
			}

			//(!MemberType.IsValueType || MemberType.IsPrimitive)
			else if (MemberType == typeof(int) || MemberType == typeof(int?)) Member.SetValue(Case, int.Parse(Value));
			else if (MemberType == typeof(uint) || MemberType == typeof(uint?)) Member.SetValue(Case, uint.Parse(Value));
			else if (MemberType == typeof(long) || MemberType == typeof(long?)) Member.SetValue(Case, long.Parse(Value));
			else if (MemberType == typeof(short) || MemberType == typeof(short?)) Member.SetValue(Case, short.Parse(Value));
			else if (MemberType == typeof(bool) || MemberType == typeof(bool?)) Member.SetValue(Case, Value.ToBoolOrNull());
			else if (MemberType == typeof(float) || MemberType == typeof(float?)) Member.SetValue(Case, float.Parse(Value));
			else if (MemberType == typeof(double) || MemberType == typeof(double?)) Member.SetValue(Case, double.Parse(Value));
			else if (MemberType == typeof(byte) || MemberType == typeof(byte?)) Member.SetValue(Case, byte.Parse(Value));
			else if (MemberType == typeof(sbyte) || MemberType == typeof(sbyte?)) Member.SetValue(Case, sbyte.Parse(Value));
			else if (MemberType == typeof(DateTime) || MemberType == typeof(DateTime?))
			{
				DateTime obj = new();
				if (long.TryParse(Value, out var number)) obj = number.GetDateTime();
				else obj = DateTime.Parse(Value);

				Member.SetValue(Case, obj);
			}
			else if (MemberType == typeof(string))
			{
				if (Value is string @string) Member.SetValue(Case, @string);
				else Member.SetValue(Case, Value);
			}

			else Member.SetValue(Case, convetor.Construct(MemberType, Value));
		}
		catch (Exception ee)
		{
			string MsgError = $"Field {Member.Name} => {Value} ({MemberType.Name})，{ee.Message}";
			Trace.WriteLine(MsgError);
		}

		return true;
	}

	public static Type GetMemberType(this MemberInfo MemberInfo)
	{
		if (MemberInfo is PropertyInfo property) return property.PropertyType;
		else if (MemberInfo is FieldInfo field) return field.FieldType;
		else throw new InvalidDataException();
	}
}