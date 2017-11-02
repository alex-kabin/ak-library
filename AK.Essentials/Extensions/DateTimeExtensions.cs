using System;

namespace AK.Essentials.Extensions
{
	public static class DateTimeExtensions
	{
		public static long AsUnixTimestamp(this DateTime dateTime)
		{
			TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds);
		}
	}
}