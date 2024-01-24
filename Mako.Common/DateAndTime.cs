namespace Mako.Common;

public static class DateAndTime
{
    public static DateTime ChangeTimeZone(this DateTime datetime, string newTimeZone)
    {
        if (datetime.Kind != DateTimeKind.Utc)
        {
            datetime = datetime.ToUniversalTime();
        }
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(newTimeZone);
        return TimeZoneInfo.ConvertTimeFromUtc(datetime, tzi);
    }

    public static DateTime TransitionDateOnYear(TimeZoneInfo.TransitionTime transition, int year)
    {
        if (transition.IsFixedDateRule)
        {
            return new DateTime(year, transition.Month, transition.Day, transition.TimeOfDay.Hour, transition.TimeOfDay.Minute, transition.TimeOfDay.Second);
        }
        // Start at the first day of the associate month
        var dt = new DateTime(year, transition.Month, 1, 
                                transition.TimeOfDay.Hour, transition.TimeOfDay.Minute, transition.TimeOfDay.Second);
        var daysUntilTransition = (7 + (transition.DayOfWeek - dt.DayOfWeek)) % 7;
        daysUntilTransition += (transition.Week - 1) * 7;
        dt = dt.AddDays(daysUntilTransition);
        return dt;
    }

    public static DateTime? NextTimeZoneChange(TimeZoneInfo tzi, DateTime asOf)
    {
        foreach (var adj in tzi.GetAdjustmentRules())
        {
            if (adj.DateStart < asOf && asOf < adj.DateEnd)
            {
                var start = TransitionDateOnYear(adj.DaylightTransitionStart, asOf.Year);
                if (asOf < start)
                {
                    return start;
                }
                var end = TransitionDateOnYear(adj.DaylightTransitionEnd, asOf.Year);
                if (asOf < end)
                {
                    return end;
                }
                start = TransitionDateOnYear(adj.DaylightTransitionStart, asOf.Year + 1);
                return start;
            }
        }
        return null;
    }
}
