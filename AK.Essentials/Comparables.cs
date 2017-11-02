using System;
using System.Linq;

namespace AK.Essentials
{
	public static class Comparables
	{
		public static T Max<T>(params T[] values) where T : IComparable<T>
		{
			return values.Max();
		}

		public static T Min<T>(params T[] values) where T : IComparable<T>
		{
			return values.Min();
		}
	}
}