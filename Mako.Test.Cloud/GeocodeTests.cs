using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Mako.Cloud;

namespace Mako.Test.Cloud;

internal class GeocodeTests
{
    private Geocode gc;

    [SetUp]
    public void Setup()
    {
        gc = new Geocode("<api key>");
    }

    [Test]
    public async Task CoordinatesExample()
    {
        var sacramento = await gc.Coordinates("Sacramento, CA");
    }

}
