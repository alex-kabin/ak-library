using System.Collections.Generic;
//using System.Diagnostics.Contracts;
using System.Linq;

namespace AK.Essentials
{
	public static class Enumerables
	{
		private static IEnumerable<T[]> Merge<T>(IEnumerable<T>[] sources, bool stopEarly)
		{
			//Contract.Requires(sources != null);
			//Contract.Requires(sources.Length > 0);

			var enumerators = sources.Select(e => e.GetEnumerator()).ToArray();
			var count = sources.Length;
			while (true)
			{
				var values = new T[count];

				bool shouldStop = !stopEarly;
				for (int i = 0; i < enumerators.Length; i++)
				{
					if (enumerators[i].MoveNext())
					{
						values[i] = enumerators[i].Current;
						
						shouldStop = false;
					}
					else
					{
						if (stopEarly)
						{
							shouldStop = true;
							break;
						}

						values[i] = default(T);
					}
				}

				if (shouldStop)
					yield break;
				
				yield return values;
			}
		}

		public static IEnumerable<T[]> MergeAll<T>(params IEnumerable<T>[] sources)
		{
			return Merge(sources, true);
		}

		public static IEnumerable<T[]> MergeAny<T>(params IEnumerable<T>[] sources)
		{
			return Merge(sources, false);
		}
	}
}