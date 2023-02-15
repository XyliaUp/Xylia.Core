using System;
using System.Linq;
using System.Reflection;


namespace Xylia.Extension
{
	/// <summary>
	/// 实例类扩展方法
	/// </summary>
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


		/// <summary>
		/// 使用指定参数调用由当前实例表示的方法或构造函数。
		/// </summary>
		/// <param name="method"></param>
		/// <param name="obj"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static object Invoke(this MethodBase method, object obj, params object[] parameters) => method.Invoke(obj, parameters);
	}
}