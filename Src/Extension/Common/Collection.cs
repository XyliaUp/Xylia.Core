using System;
using System.Collections.Generic;
using System.Linq;

namespace Xylia.Extension
{
	public static class CollectionEx
	{
		public static void ForEach<T>(this IEnumerable<T> collection,  Action<T> action)
		{
			foreach (var item in collection) 
				action(item);
		}

		public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
		{
			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
	}
}