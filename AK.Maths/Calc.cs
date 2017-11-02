using System;

namespace AK.Maths
{
	public static class Calc
	{
		public static ulong Factorial(int n)
		{
			if(n < 0)
				throw new ArgumentOutOfRangeException("n should be >= 0");

			ulong r = 1;
			for (int i = 2; i <= n; i++)
				r *= (ulong)i;

			return r;
		}
	}
}