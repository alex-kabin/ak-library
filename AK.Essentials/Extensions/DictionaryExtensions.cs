using System;
using System.Collections.Generic;

namespace AK.Essentials.Extensions
{
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Adds or sets dictionary item
		/// </summary>
		/// <typeparam name="TKey">Key type</typeparam>
		/// <typeparam name="TValue">Value type</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		/// <returns><c>true</c> if item was added, <c>false</c> if item was replaced</returns>
		public static bool AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if(dictionary == null)
				throw new ArgumentNullException("dictionary", "Dictionary should not be null");

			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
				return false;
			}
			
			dictionary.Add(key, value);
			return true;
		}
	}
}