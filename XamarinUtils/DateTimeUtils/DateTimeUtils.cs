using System;

namespace XamarinUtils
{
	public static class DateTimeUtils
	{
		public static DateTime epoch = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime StartOfYear (this DateTime date)
		{
			return new DateTime (date.Year, 0, 0, 0, 0, 0);
		}

		public static DateTime EndOfYear (this DateTime date)
		{
			return date.StartOfYear ().AddYears (1).AddDays (-1);
		}

		public static DateTime StartOfMonth (this DateTime date)
		{
			return date.AddDays (1 - date.Day);
		}

		public static DateTime EndOfMonth (this DateTime dt)
		{
			return dt.StartOfMonth ().AddMonths (1).AddDays (-1);
		}

		public static DateTime StartOfWeek (this DateTime date)
		{
			int diff = date.DayOfWeek - DayOfWeek.Monday;

			if (diff < 0) {
				diff += 7;
			}

			return date.AddDays (-1 * diff).Date;
		}

		public static DateTime EndOfWeek (this DateTime date)
		{
			return date.StartOfWeek ().AddDays (6);
		}

		public static DateTime StartOfDay (this DateTime date)
		{
			return new DateTime (date.Year, date.Month, date.Day, 0, 0, 0);
		}

		public static DateTime EndOfDay (this DateTime date)
		{
			return date.StartOfDay ().AddDays (1).AddSeconds (-1);
		}

		public static DateTime SecondsToDateTime (long seconds)
		{
			return epoch.AddSeconds (seconds);
		}

		public static long ToSeconds (this DateTime dt)
		{
			DateTime date = TimeZoneInfo.ConvertTimeToUtc (dt);
			return (long)(date - epoch).TotalSeconds;
		}
	}
}