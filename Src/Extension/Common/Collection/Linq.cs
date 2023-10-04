﻿namespace Xylia.Extension;
public static class Linq
{
	#region Array
	public static void For(int Num, Action<int> func)
	{
		for (int INDEX = 0; INDEX < Num; INDEX++)
			func(INDEX);
	}

	public static void For<T>(ref T[] array, Func<int, T> func)
	{
		ArgumentNullException.ThrowIfNull(array);
		array = For(array.Length, func);
	}

	public static T[] For<T>(int Num, Func<int, T> func)
	{
		var array = new T[Num];
		for (int INDEX = 0; INDEX < Num; INDEX++)
			array[INDEX] = func(INDEX + 1);

		return array;
	}
	#endregion



	public static string Aggregate(this IEnumerable<string> source, string comma, Func<string, string> func = null)
	{
		ArgumentNullException.ThrowIfNull(source);

		using var e = source.GetEnumerator();
		if (!e.MoveNext()) return null;

		string result = null;
		while (true)
		{
			result += func is null ? e.Current : func(e.Current);

			bool HasNext = e.MoveNext();
			if (HasNext) result += comma;
			else break;
		}

		return result;
	}

	public static IEnumerable<string> Split(this IEnumerable<string> strings, char separator)
	{
		ArgumentNullException.ThrowIfNull(strings);
		return strings.Where(o => !string.IsNullOrEmpty(o)).SelectMany(o => o.Split(separator));
	}



	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
	{
		foreach (var item in collection)
			action(item);
	}

	public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
	{
		while (enumerator.MoveNext())
			yield return enumerator.Current;
	}




	public static Dictionary<TKey, TSource> ToDistinctDictionary<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);

		var dict = new Dictionary<TKey, TSource>();
		foreach (var current in source)
		{
			var key = keySelector(current);
			if (key is null || dict.ContainsKey(key)) continue;

			dict[key] = current;
		}
		return dict;
	}

	public static Dictionary<TKey, TElement> ToDistinctDictionary<TSource, TKey, TElement>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(keySelector);
		ArgumentNullException.ThrowIfNull(elementSelector);

		var dict = new Dictionary<TKey, TElement>();
		foreach (var current in source)
		{
			var key = keySelector(current);
			if (key is null || dict.ContainsKey(key)) continue;

			dict[key] = elementSelector(current);
		}
		return dict;
	}
}