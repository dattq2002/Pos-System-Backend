using Pos_System.API.Enums;
using Pos_System.API.Utils;

namespace Pos_System.API.Helpers;

public static class DateTimeHelper
{
	public static DateFilter GetDateFromDateTime(DateTime datetime)
	{
		var date = datetime.DayOfWeek;
		switch (date)
		{
			case DayOfWeek.Sunday:
				return DateFilter.Sunday;
			case DayOfWeek.Monday:
				return DateFilter.Monday;
			case DayOfWeek.Tuesday:
				return DateFilter.Tuesday;
			case DayOfWeek.Wednesday:
				return DateFilter.Wednesday;
			case DayOfWeek.Thursday:
				return DateFilter.Thursday;
			case DayOfWeek.Friday:
				return DateFilter.Friday;
			case DayOfWeek.Saturday:
				return DateFilter.Saturday;
			default:
				return DateFilter.Monday;
		}
	}

	public static List<DateFilter> GetDatesFromDateFilter(int? dateFilter)
	{
		List<DateFilter> dateFilters = new List<DateFilter>();
		if (dateFilter.HasValue)
		{
			foreach (var date in EnumUtil.GetValues<DateFilter>())
			{
				if ((dateFilter.Value & (int)date) > 0) dateFilters.Add(date);
			}
		}

		return dateFilters;
	}

	public static TimeOnly? ConvertIntToTimeOnly(int? timeIntFormat)
	{
		if (timeIntFormat.HasValue)
		{
			int hour = (int)timeIntFormat.Value / 60;
			int minute = ((int) timeIntFormat.Value % 60) * 60;
			return new TimeOnly(hour, minute);
		}

		return null;
	}
}