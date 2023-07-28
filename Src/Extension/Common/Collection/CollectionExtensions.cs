namespace Xylia.Extension;
public static class CollectionExtensions
{
	public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
		=> dictionary.GetValueOrDefault(key, default);



	public static void AddItem<T>(this List<T> List, T item)
	{
		if (item != null)
			List.Add(item);
	}

	public static List<T> ConvertAll<T>(this IEnumerable<object> objs) => objs.Select(o => (T)o).ToList();
}
