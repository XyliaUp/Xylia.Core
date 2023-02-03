using System;
using System.Collections.Generic;
using System.Diagnostics;

using NPOI.POIFS.Crypt.Dsig;

namespace Xylia.Extension
{
	/// <summary>
	/// 字典扩展方法
	/// </summary>
	public static class DictionaryEx
	{
		/// <summary>
		/// 合并两个字典
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="First"></param>
		/// <param name="Second"></param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> First, Dictionary<TKey, TValue> Second)
		{
			if (First == null) First = new Dictionary<TKey, TValue>();
			if (Second == null) return First;

			foreach (TKey Key in Second.Keys)
			{
				if (!First.ContainsKey(Key)) First.Add(Key, Second[Key]);
				else Debug.WriteLine("已经包含" + Key);
			}

			return First;
		}


		public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Dictionary<TKey, List<TSource>> result = new();
			foreach (var item in source)
			{
				TKey key = default;
				if (!result.ContainsKey(key)) 
					result.Add(key, new List<TSource>());

				result[key].Add(item);
			}


			return result;
		}
	}
}