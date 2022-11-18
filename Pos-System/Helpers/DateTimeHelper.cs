using Pos_System.API.Enums;

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
}