namespace System.Linq;

public static partial class Extensions
{
	/// <summary>
	///     Finds the index of the first item matching the predicate (-1 if no match)
	/// </summary>
	/// <typeparam name="T">The item type</typeparam>
	/// <param name="items">The item collection</param>
	/// <param name="predicate">a predicate such as i => f(i)==true</param>
	/// <returns>A 0-index value representing the position of the item first matching the predicate
	/// (-1 if no match)
	///  </returns>
	public static int FirstIndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
	{
		if (items == null) { return -1; }
		if (predicate == null) { return -1; }

		int index = 0;
		foreach (var item in items)
		{
			if (predicate(item)) { return index; }
			index++;
		}
		return -1;
	}

	/// <summary>
	///     Finds the index of an item within a collection
	/// </summary>
	/// <remarks>
	///     Assume the items does not contain duplicate entries
	///		In other words, this functions more like FirstIndexOf if the collection allows duplicates.
	/// </remarks>
	/// <typeparam name="T">The item type</typeparam>
	/// <param name="items">The item collection</param>
	/// <param name="item">The item to find</param>
	/// <returns>A 0-index value representing the position of the item (-1 if no match)</returns>
	public static int IndexOf<T>(this IEnumerable<T> items, T item)
	{
		return items.FirstIndexOf(i => EqualityComparer<T>.Default.Equals(item, i));
	}

	public static IEnumerable<int> AllIndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
	{
		if (items == null) { return []; }
		if (predicate == null) { return []; }

		var indexList = new List<int>();
		var index = 0;
		foreach (var item in items)
		{
			if (predicate(item)) { indexList.Add(index); }
			index++;
		}

		return indexList;
	}

	public static int NumberOfDifferences<T>(this IEnumerable<T> items, IEnumerable<T> otherItems)
	{
		if (items == null) { return -1; }
		if (otherItems == null) { return -1; }

		var index = 0;
		var differences = 0;
		foreach (var item in items)
		{
			if (!EqualityComparer<T>.Default.Equals(item, otherItems.ElementAt(index)))
				differences++;

			index++;
		}

		return differences;
	}
}