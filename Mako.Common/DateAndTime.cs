using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    /*public static DateTime NextTimeZoneChange(string timeZone)
    {
        var tzi = TimeZoneInfo.FromSerializedString(timeZone);
        tzi.TransitionTime.CreateFixedDateRule()
    }*/
}
