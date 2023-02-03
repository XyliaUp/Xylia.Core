using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;


namespace Xylia.Extension
{
	/// <summary>
	/// 实例类扩展方法
	/// </summary>
	public static partial class ClassExtension
	{
		#region 字段
		/// <summary>
		/// 绑定标志
		/// </summary>
		public static BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		/// <summary>
		/// 不重复提示消息
		/// </summary>
		public static bool UseUniqueMsg = true;

		/// <summary>
		/// 消息缓存器
		/// </summary>
		public static List<string> CacheMsg = new();
		#endregion


		#region 获得对象信息
		/// <summary>
		/// 获得 <see cref="FieldInfo"/> (字段) 对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="FieldName"></param>
		/// <param name="IgnoreCase"></param>
		/// <returns></returns>
		public static FieldInfo GetFieldInfo<T>(this T Case, string FieldName, bool IgnoreCase = false)
		{
			var Info = GetMemberInfo(Case, FieldName, IgnoreCase);

			if (Info is FieldInfo) return Info as FieldInfo;
			else return null;
		}

		/// <summary>
		/// 获得 <see cref="PropertyInfo"/> (属性) 对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="PropertyName"></param>
		/// <param name="IgnoreCase"></param>
		/// <returns></returns>
		public static PropertyInfo GetPropertyInfo<T>(this T Case, string PropertyName, bool IgnoreCase = false)
		{
			var Info = GetMemberInfo(Case, PropertyName, IgnoreCase);

			if (Info is PropertyInfo) return Info as PropertyInfo;
			else return null;
		}

		/// <summary>
		/// 获得 <see cref="MemberInfo"/> (成员) 对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="Name"></param>
		/// <param name="IgnoreCase"></param>
		/// <returns></returns>
		public static MemberInfo GetMemberInfo<T>(this T Case, object Name, bool IgnoreCase = false)
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




		/// <summary>
		/// 通过字段名称设置字段类型（泛型）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="PropertyName"></param>
		/// <param name="IgnoreCase"></param>
		/// <returns></returns>
		public static Type GetMemberType<T>(this T Case, string PropertyName, bool IgnoreCase = false)
			=> Case.GetMemberInfo(PropertyName, IgnoreCase)?.GetMemberType();

		public static Type GetMemberType(this MemberInfo MemberInfo)
		{
			if (MemberInfo is PropertyInfo) return (MemberInfo as PropertyInfo).PropertyType;
			else if (MemberInfo is FieldInfo) return (MemberInfo as FieldInfo).FieldType;

			throw new Exception($"[GetMemberType] 参数错误 ({MemberInfo.GetType()})");
		}

		/// <summary>
		/// 通过字段名称获取字段数值（泛型）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="PropertyName"></param>
		/// <param name="IgnoreCase"></param>
		/// <returns></returns>
		public static object GetMemberVal<T>(this T Case, object PropertyName, bool IgnoreCase = false)
			=> Case.GetMemberInfo(PropertyName, IgnoreCase)?.GetValue(Case);



		/// <summary>
		/// 通过字段名称设置字段数值（泛型）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Case"></param>
		/// <param name="Name"></param>
		/// <param name="Value"></param>
		/// <param name="IgnoreError">忽略错误（<see langword="flase" />抛出错误，<see langword="true" />输出调试信息，<see langword="null" />不做处理)</param>
		/// <param name="IgnoreCase">忽略大小写</param>
		/// <param name="convetor"></param>
		/// <returns>转换是否成功</returns>
		public static bool SetMember<T>(this T Case, string Name, object Value, bool IgnoreCase = false, bool? IgnoreError = false, ConvertClass convetor = null)
		{
			//判断是否存在目标对象
			var TarMember = Case.GetMemberInfo(Name, IgnoreCase);
			if (TarMember != null) return Case.SetMember(TarMember, Value, IgnoreError, convetor);

			#region 无效处理
			if (IgnoreError != null)
			{
				string Msg = $"不存在字段或属性 { Case.GetType().Name } -> { Name }";

				if (IgnoreError == false) throw new Exception(Msg + $" ({ Value })");
				else if (IgnoreError == true)
				{
					//如果使用相同的消息仅显示一次设置
					if (UseUniqueMsg)
					{
						if (!CacheMsg.Contains(Msg)) CacheMsg.Add(Msg);
						else return false;
					}

					Debug.WriteLine("[debug] " + Msg + $" ({ Value })");
				}
			}

			return false;
			#endregion
		}

		public static bool SetMember<T>(this T Case, MemberInfo Member, object Value, bool? IgnoreError = false, ConvertClass convetor = null)
		{
			#region 初始化
			//判断是否可以写入
			if (Member is PropertyInfo PInfo && !PInfo.CanWrite) return false;

			//值未设置时，直接将对象值赋空即可
			else if (Value is null)
			{
				Member.SetValue(Case, null);
				return true;
			}


			//数据类型相同时，直接传递
			var ValueType = Value.GetType();
			var MemberType = Member.GetMemberType();
			if (ValueType.HasImplementedRawGeneric(MemberType)) return Member.SetValue(Case, Value);
			#endregion


			try
			{
				// 枚举数据类型处理
				if (MemberType.IsEnum)
				{
					bool Status = Value.ToString().TryParseToEnum(MemberType, out var Result, true);
					if (!Status) throw new FormatException($"不存在枚举值 { Value }");

					Member.SetValue(Case, Result);
				}

				// 数组处理
				else if (MemberType.IsArray)
				{
					var elementType = MemberType.GetElementType();
					var subs = Value.ToString().Trim(';').Split(';');

					Array myArray = Array.CreateInstance(elementType, subs.Length);
					for (int i = 0; i < subs.Length; ++i)
					{
						//基础数据类型处理
						//if (!MemberType.IsValueType || MemberType.IsPrimitive)

						myArray.SetValue((convetor ?? new ConvertClass()).Construct(elementType, subs[i]), i);
					}

					Member.SetValue(Case, myArray);
				}

				// 集合处理 List<>
				//else if (MemberType.IsGenericType && MemberType.GetGenericTypeDefinition().Equals(typeof(List<>)))
				//{
				//	var modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type.GenericTypeArguments[0] }));
				//	var addMethod = modelList.GetType().GetMethod("Add");
				//	addMethod.Invoke(modelList, new object[] { model });
				//}

				#region 基础数据类型处理 (!MemberType.IsValueType || MemberType.IsPrimitive)
				// 无法判断 ? 类型  & 资源浪费
				else if (MemberType == typeof(int) || MemberType == typeof(int?)) Member.SetValue(Case, int.Parse(Value.ToString()));
				else if (MemberType == typeof(uint) || MemberType == typeof(uint?)) Member.SetValue(Case, uint.Parse(Value.ToString()));
				else if (MemberType == typeof(long) || MemberType == typeof(long?)) Member.SetValue(Case, long.Parse(Value.ToString()));
				else if (MemberType == typeof(short) || MemberType == typeof(short?)) Member.SetValue(Case, short.Parse(Value.ToString()));
				else if (MemberType == typeof(bool) || MemberType == typeof(bool?)) Member.SetValue(Case, Value.ToString().ToBoolOrNull());
				else if (MemberType == typeof(float) || MemberType == typeof(float?)) Member.SetValue(Case, float.Parse(Value.ToString()));
				else if (MemberType == typeof(double) || MemberType == typeof(double?)) Member.SetValue(Case, double.Parse(Value.ToString()));
				else if (MemberType == typeof(byte) || MemberType == typeof(byte?)) Member.SetValue(Case, byte.Parse(Value.ToString()));
				else if (MemberType == typeof(sbyte) || MemberType == typeof(sbyte?)) Member.SetValue(Case, sbyte.Parse(Value.ToString()));
				else if (MemberType == typeof(DateTime) || MemberType == typeof(DateTime?))
				{
					#region 获取对象
					DateTime obj = new();

					if (Value is DateTime time) obj = time;

					var @value = Value.ToString();
					if (long.TryParse(@value, out var number)) obj = number.GetBNSTime();
					else obj = DateTime.Parse(@value);
					#endregion

					Member.SetValue(Case, obj);
				}
				else if (MemberType == typeof(string))
				{
					if (Value is string @string) Member.SetValue(Case, @string);
					else Member.SetValue(Case, Value.ToString());
				}
				#endregion

				// 实例处理
				else Member.SetValue(Case, (convetor ?? new ConvertClass()).Construct(MemberType, Value));
			}
			catch (Exception ee)
			{
				string MsgError = $"字段名 { Member.Name } => {Value} ({ MemberType.Name })，{ ee.Message }";

				if (IgnoreError == true) Trace.WriteLine(MsgError);
				else if (IgnoreError == false)
				{
					if (ee is FormatException) throw;
					else throw new Exception(MsgError, ee);
				}
			}

			return true;
		}




		/// <summary>
		/// 获取对象数值
		/// 注意调用函数与本函数名称一致，给出的参数错误会导致异常
		/// </summary>
		/// <param name="member"></param>
		/// <param name="obj">对象实例</param>
		/// <returns></returns>
		public static object GetValue(this MemberInfo member, object obj = null)
		{
			// 注意调用函数与本函数名称一致，给出的参数错误会导致异常
			if (member is FieldInfo field) return field.GetValue(obj);
			else if (member is PropertyInfo property) return property.GetValue(obj, null);

			throw new ArgumentException(member + "是未支持的对象");
		}

		/// <summary>
		/// 设置对象数值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Info"></param>
		/// <param name="Case"></param>
		/// <param name="Value"></param>
		public static bool SetValue<T>(this MemberInfo Info, T Case, object Value)
		{
			if (Info is PropertyInfo) (Info as PropertyInfo).SetValue(Case, Value);
			else if (Info is FieldInfo) (Info as FieldInfo).SetValue(Case, Value);

			else throw new Exception("[SetValue] 参数错误");

			return true;
		}
		#endregion


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