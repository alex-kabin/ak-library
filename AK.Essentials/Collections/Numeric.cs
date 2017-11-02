using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Essentials.Collections
{
	public static class Numeric
	{
		private static readonly Random _random = new Random();

		public static IEnumerable<double> Randoms(double minimum = 0.0, double maximum = 1.0)
		{
			while (true)
			{
				yield return minimum + (maximum - minimum) * _random.NextDouble();
			}
		}

		public static IEnumerable<int> Randoms(int minimum, int maximum)
		{
			while (true)
			{
				yield return _random.Next(minimum, maximum);
			}
		}

		public static IEnumerable<int> Range(int minimum, int maximum)
		{
			return Enumerable.Range(minimum, maximum - minimum);
		}
	}
}