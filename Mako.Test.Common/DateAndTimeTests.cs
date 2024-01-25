using System;
using NUnit.Framework;
using Mako.Common;
using static System.TimeZoneInfo;
using NuGet.Frameworks;

namespace Mako.Test.Common;

internal class DateAndTimeTests
{
    [Test]
    public void TransitionToDate_MLK2021()
    {
        var mlkRule = TransitionTime.CreateFloatingDateRule(new DateTime(new DateOnly(1, 1, 1), new TimeOnly(12, 0)), 1, 3, DayOfWeek.Monday);
        var mlkDate = DateAndTime.TransitionDateOnYear(mlkRule, 2021);
        Assert.That(mlkDate.Day.Equals(18));
        Assert.That(mlkDate.Month.Equals(1));
        Assert.That(mlkDate.Year.Equals(2021));
        Assert.That(mlkDate.DayOfWeek.Equals(DayOfWeek.Monday));
    }

    [Test]
    public void TransitionToDate_Thanksgiving2010()
    {
        var turkeyRule = TransitionTime.CreateFloatingDateRule(new DateTime(new DateOnly(1, 1, 1), new TimeOnly(12, 0)), 11, 4, DayOfWeek.Thursday);
        var turkeyDay = DateAndTime.TransitionDateOnYear(turkeyRule, 2010);
        Assert.That(turkeyDay.Day.Equals(25));
        Assert.That(turkeyDay.Month.Equals(11));
        Assert.That(turkeyDay.Year.Equals(2010));
        Assert.That(turkeyDay.DayOfWeek.Equals(DayOfWeek.Thursday));
    }

    [Test]
    public void TransitionToDate_July4th2009()
    {
        var jul4Rule = TransitionTime.CreateFixedDateRule(new DateTime(new DateOnly(1, 1, 1), new TimeOnly(12, 0)), 7, 4);
        var july4Date = DateAndTime.TransitionDateOnYear(jul4Rule, 2009);
        Assert.That(july4Date.Day.Equals(4));
        Assert.That(july4Date.Month.Equals(7));
        Assert.That(july4Date.Year.Equals(2009));
        Assert.That(july4Date.DayOfWeek.Equals(DayOfWeek.Saturday));
    }

    [Test]
    public void TransitionToDate_LaborDay2015()
    {
        var laborDayRule = TransitionTime.CreateFloatingDateRule(new DateTime(new DateOnly(1, 1, 1), new TimeOnly(12, 0)), 9, 1, DayOfWeek.Monday);
        var laborDay = DateAndTime.TransitionDateOnYear(laborDayRule, 2015);
        Assert.That(laborDay.Day.Equals(7));
        Assert.That(laborDay.Month.Equals(9));
        Assert.That(laborDay.Year.Equals(2015));
        Assert.That(laborDay.DayOfWeek.Equals(DayOfWeek.Monday));
    }

    [Test]
    public void DaylightChange_ESTMid2024()
    {
        var asOf = new DateTime(2024, 6, 30);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        var timeChange = DateAndTime.NextTimeZoneChange(timeZone, asOf).Value;
        Assert.That(timeChange.Year.Equals(2024));
        Assert.That(timeChange.Month.Equals(11));
        Assert.That(timeChange.Day.Equals(3));
        Assert.That(timeChange.DayOfWeek.Equals(DayOfWeek.Sunday));
        Assert.That(timeChange.Hour.Equals(2));
    }

    [Test]
    public void DaylightChange_PSTStart2024()
    {
        var asOf = new DateTime(2024, 1, 1);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var timeChange = DateAndTime.NextTimeZoneChange(timeZone, asOf).Value;
        Assert.That(timeChange.Year.Equals(2024));
        Assert.That(timeChange.Month.Equals(3));
        Assert.That(timeChange.Day.Equals(10));
        Assert.That(timeChange.DayOfWeek.Equals(DayOfWeek.Sunday));
        Assert.That(timeChange.Hour.Equals(2));
    }

    [Test]
    public void DaylightChange_CSTStart2024()
    {
        var asOf = new DateTime(2024, 12, 31);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        var timeChange = DateAndTime.NextTimeZoneChange(timeZone, asOf).Value;
        Assert.That(timeChange.Year.Equals(2025));
        Assert.That(timeChange.Month.Equals(3));
        Assert.That(timeChange.Day.Equals(9));
        Assert.That(timeChange.DayOfWeek.Equals(DayOfWeek.Sunday));
        Assert.That(timeChange.Hour.Equals(2));
    }

    [Test]
    public void CheckDaylightTransition()
    {
        var start = DateTimeOffset.FromUnixTimeSeconds(1730610000);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        for(var h = 0; h < 2; h++)
        {
            var dto = start.AddHours(h);
            var dtadj = DateAndTime.GetTimeAtZone(timeZone, dto);
            Assert.That(dtadj.Hour.Equals(1));
        }
    }
}
