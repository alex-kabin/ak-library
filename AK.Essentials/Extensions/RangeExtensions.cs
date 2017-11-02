namespace AK.Essentials.Extensions
{
	public static class RangeExtensions
	{
		public static int Length(this Range<int> range)
		{
			return range.Max - range.Min;
		}

		public static double Length(this Range<double> range)
		{
			return range.Max - range.Min;
		}
	}
}