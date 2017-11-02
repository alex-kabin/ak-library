using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Maths
{
	public static class Combinatorics
	{
		#region Permutations

		private static bool NextPermutation(int[] array)
		{
			int i;
			for (i = array.Length - 1; i > 0 && array[i - 1] >= array[i]; --i) ;
			if (i == 0)
			{
				return false;
			}
			else
			{
				int j;
				for (j = array.Length - 1; array[j] <= array[i - 1]; --j) ;
				int temp = array[i - 1];
				array[i - 1] = array[j];
				array[j] = temp;
				Array.Reverse(array, i, array.Length - i);
				return true;
			}
		}

		/// <summary>
		/// Возвращает все перестановки элементов массива
		/// </summary>
		/// <typeparam name="T">Тип элемента массива</typeparam>
		/// <param name="set">Множество элементов для перстановки</param>
		/// <returns>Перестановки</returns>
		/// <remarks>Число возможных перестановок = [Кол-во элементов массива]!</remarks>
		/// <example>Для входа {1,2,3} будут сгенерированы (возможно в другом порядке): 
		/// {1,2,3}, {1,3,2}, {3,2,1}, {2,1,3}, {2,3,1}, {3,1,2}
		/// </example>
		public static IEnumerable<T[]> Permutations<T>(this T[] set)
		{
			if (set == null)
				throw new ArgumentNullException("set");

			if (set.Length == 0)
				throw new ArgumentException("Set should be non empty", "set");

			int length = set.Length;
			int[] indices = new int[length];
			for (int i = 0; i < length; i++)
				indices[i] = i;

			do
			{
				var result = new T[length];
				for (int i = 0; i < length; i++)
					result[i] = set[indices[i]];

				yield return result;
			}
			while (NextPermutation(indices));
		}

		public static IEnumerable<T[]> Permutations<T>(this T[] set, int k)
		{
			if (set == null)
				throw new ArgumentNullException("set");

			if (set.Length == 0)
				throw new ArgumentException("Set should be non empty", "set");

			if(k <= 0)
				throw new ArgumentOutOfRangeException("k should be >= 1");

			return Combinations(set, k).SelectMany(c => Permutations(c));
		}

		public static ulong PermutationsCount<T>(this T[] set)
		{
			return Calc.Factorial(set.Length);
		}

		public static ulong PermutationsCount<T>(this T[] set, int k)
		{
			int n = set.Length;
			ulong r = (ulong)n;
			for (int i = 1; i < k; i++)
				r *= (ulong)(--n);
			return r;
		}
		#endregion // Permutations

		#region Multicombinations
		private static bool NextIndicesMulticombination(int[] indices, int m)
		{
			int i = indices.Length - 1;
			for (; i >= 0 && indices[i] == m; i--)
				indices[i] = 0;

			if (i >= 0)
			{
				indices[i]++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Возвращает все возможные последовательности длины <paramref name="k"/>, образованные из элементов массива 
		/// A multicombination is a combination that can contain duplicates (i.e., the combination is a multiset). 
		/// </summary>
		/// <typeparam name="T">Тип элемента массива</typeparam>
		/// <param name="set">Массив элементов</param>
		/// <param name="k">Длина последовательности.</param>
		/// <returns>Последовательности</returns>
		/// <remarks>Всего возможных последовательностей = [Кол-во элементов массива]^<paramref name="k>"/></remarks>
		/// <example>Для входа {1,2,3} будут сгенерированы: 
		/// {1,1,1}, {1,1,2}, {1,1,3}, {1,2,1}, {1,2,2}, {1,2,3}, {1,3,1}, {1,3,2},{1,3,3},
		/// {2,1,1}, {2,1,2}, {2,1,3}, {2,2,1}, {2,2,2}, {2,2,3}, {2,3,1}, {2,3,2},{2,3,3},
		/// {3,1,1}, {3,1,2}, {3,1,3}, {3,2,1}, {3,2,2}, {3,2,3}, {3,3,1}, {3,3,2},{3,3,3}
		/// </example>
		public static IEnumerable<T[]> Multicombinations<T>(T[] set, int k)
		{
			if (set == null)
				throw new ArgumentNullException("set");

			if (set.Length == 0)
				throw new ArgumentException("Set should be non empty", "set");

			if (k <= 0)
				throw new ArgumentOutOfRangeException("k");

			int[] indices = new int[k];
			int maxIndex = set.Length - 1;
			do
			{
				var result = new T[k];
				for (int i = 0; i < k; i++)
					result[i] = set[indices[i]];

				yield return result;
			}
			while (NextIndicesMulticombination(indices, maxIndex));
		}

		public static ulong MulticombinationsCount<T>(T[] set, int k)
		{
			return (ulong)Math.Pow(set.Length, k);
		}
		#endregion // Multicombinations

		/// <summary>
		/// Combinations, or k-combinations, are the unordered sets of k elements chosen from a set of size n.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="set"></param>
		/// <param name="k"></param>
		/// <returns></returns>
		/// <example>For example, there are 10 3-combinations of the 5-set {0, 1, 2, 3, 4}: {0, 1, 2}, {0, 1, 3}, {0, 1, 4}, {0, 2, 3}, {0, 2, 4}, {0, 3, 4}, {1, 2, 3}, {1, 2, 4}, {1, 3, 4}, {2, 3, 4}</example>
		public static IEnumerable<T[]> Combinations<T>(T[] set, int k)
		{
			if(set == null)
				throw new ArgumentNullException("set");
			
			int n = set.Length;
			if(k <= 0 || k > n)
				throw new ArgumentOutOfRangeException("k should be > 0 and <= set length");
			
			var indices = Enumerable.Range(0, n).ToArray();
			bool changed;

			do
			{
				changed = false;
				for (int i = k - 1; i > 0; i--)
				{
					if (indices[i] < (n - 1) - (k - 1) + i)
					{
						/* Increment this element */
						indices[i]++;
						if (i < k - 1)
						{
							/* Turn the elements after it into a linear sequence */
							for (int j = i + 1; j < k; j++)
							{
								indices[j] = indices[j - 1] + 1;
							}
						}
						
						var result = new T[n];
						for (int c = 0; c < n; c++)
							result[c] = set[indices[c]];
						yield return result;

						changed = true;
						break;
					}
				}
			}
			while (changed);
		}
	}
}