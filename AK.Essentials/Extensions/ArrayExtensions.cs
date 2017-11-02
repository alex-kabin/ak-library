using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Essentials.Extensions
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Swaps array values at indices <paramref name="index1"/> and <paramref name="index2"/> />
		/// </summary>
		/// <typeparam name="T">Array element type</typeparam>
		/// <param name="array">Array</param>
		/// <param name="index1">Index1</param>
		/// <param name="index2">Index2</param>
		public static void SwapValues<T>(this T[] array, int index1, int index2)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			if (index1 < 0 || index1 >= array.Length)
				throw new ArgumentOutOfRangeException("index1");
			if (index2 < 0 || index2 >= array.Length)
				throw new ArgumentOutOfRangeException("index2");

			T temp = array[index1];
			array[index2] = array[index1];
			array[index1] = temp;
		}

		/// <summary>
		/// Возвращает случайно перетасованный массив
		/// </summary>
		/// <typeparam name="T">Тип элемента массива</typeparam>
		/// <param name="array">Массив элементов</param>
		/// <returns>Массив</returns>
		public static T[] Shuffle<T>(this T[] array)
		{
			var random = new Random();
			var result = array;
			int cycles = random.Next(result.Length / 2, result.Length);
			while (cycles-- > 0)
			{
				var i1 = random.Next(result.Length);
				var i2 = random.Next(result.Length);

				result.SwapValues(i1, i2);
			}

			return result;
		}
	}
}