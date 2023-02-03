using System.Collections.Generic;
using System.Linq;

namespace Xylia.Extension
{
	/// <summary>
	/// 数组扩展方法
	/// </summary>
	public static class EnumerableEx
	{
		/// <summary>
		/// 仅在对象不为空指针时追加对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="List"></param>
		/// <param name="item"></param>
		public static void AddItem<T>(this List<T> List, T item)
		{
			if (item != null) List.Add(item);
		}

		/// <summary>
		/// 将当前元素集合对象转换为<see cref="List&lt;T&gt;"/>。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objs"></param>
		/// <returns></returns>
		public static List<T> ConvertAll<T>(this IEnumerable<object> objs) => objs.ToList().ConvertAll(s => (T)s);
	}
}
