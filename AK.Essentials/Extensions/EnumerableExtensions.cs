using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Essentials.Extensions
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (var item in sequence)
			{
				action(item);
			}
		}
		
		public static IEnumerable<KeyValuePair<TIndex, TElement>> WithKey<TIndex, TElement>(
			this IEnumerable<TElement> sequence,
			TIndex startIndex,
			Func<TIndex, TIndex> nextIndex)
		{
			var index = startIndex;
			foreach (var item in sequence)
			{
				yield return new KeyValuePair<TIndex, TElement>(index, item);
				index = nextIndex(index);
			}
		}

		public static IEnumerable<KeyValuePair<int, TElement>> WithIndex<TElement>(this IEnumerable<TElement> sequence)
		{
			return WithKey(sequence, 0, i => i + 1);
		}

		public static IEnumerable<T> Execute<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (var item in sequence)
			{
				action(item);
				yield return item;
			}
		}
	}
}