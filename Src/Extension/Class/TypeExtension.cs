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
		#region 类型判断
		/// <summary>
		/// 判断指定的类型 <paramref name="type"/> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
		/// </summary>
		/// <param name="type">需要测试的类型。</param>
		/// <param name="generic">泛型接口类型，传入 typeof(IXxx&lt;&gt;)</param>
		/// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
		public static bool HasImplementedRawGeneric(this Type type, Type generic)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (generic == null) throw new ArgumentNullException(nameof(generic));

			//测试接口。
			var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
			if (isTheRawGenericType) return true;

			//测试类型。
			while (type != null && type != typeof(object))
			{
				isTheRawGenericType = IsTheRawGenericType(type);
				if (isTheRawGenericType) return true;
				type = type.BaseType;
			}

			// 没有找到任何匹配的接口或类型。
			return false;

			// 测试某个类型是否是指定的原始接口。
			bool IsTheRawGenericType(Type test) => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
		}

		public static bool HasImplementedRawGeneric(this MemberInfo member, Type generic)
		{
			Type CurType;
			if (member is FieldInfo field) CurType = field.FieldType;
			else if (member is PropertyInfo property) CurType = property.PropertyType;
			//else if (member is MethodInfo method) CurType = method.ReturnType;
			else return false;
			// throw new ArgumentException(member + "是未支持的对象");

			return HasImplementedRawGeneric(CurType, generic);
		}
		#endregion
	}
}