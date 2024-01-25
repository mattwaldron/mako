using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Mako.Cloud;

namespace Mako.Test.Cloud;

internal class GoogleGeocodeTests
{
    private GoogleGeocode ggc;

    [SetUp]
    public void Setup()
    {
        ggc = new GoogleGeocode("AIzaSyBItIh62R0I474t7wg7PBEvEbuahfPR2jA");
    }

    [Test]
    public async Task Coordinates_Sacramento()
    {
        var (lat, lng) = await ggc.Coordinates("Sacramento, CA");
        Assert.That(lat > 38.5);
        Assert.That(lat < 39);
        Assert.That(lng < -121);
        Assert.That(lng > -122);
    }

    [Test]
    public async Task TimeZone_Sacramento()
    {
        var offset = await ggc.CoordinatesToTimeOffset(38.5815719, -121.4943996);
        Assert.That(offset == -28800 || offset == -25200);
    }

    [Test]
    public async Task TimeZoneName_Sacramento()
    {
        var timezone = await ggc.CoordinatesToTimeZoneName(38.5815719, -121.4943996);
        Assert.That(timezone.Contains("Pacific"));
    }
}
