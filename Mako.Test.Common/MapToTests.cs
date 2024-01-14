using NUnit.Framework;
using System.ComponentModel;
using System.Collections.Generic;
using Mako.Common;

namespace Mako.Test.Common;

public class Manager
{
    public string Name { get; set; }
    public int Id;
    public Manager Supervisor { get; set; }
    public List<Employee> Reports { get; set; }
}

public class Employee
{
    public string? Name { get; set; }
    public int Id;
    public Manager? Supervisor { get; set; }
}

public class MapToTests
{
    [Test]
    public void Promote()
    {
        var b = new Manager
        {
            Name = "boss"
        };

        var e = new Employee
        {
            Name = "matt",
            Id = 123456,
            Supervisor = b
        };

        var m = MapTo<Manager>.From(e);
        Assert.That(m.Name.Equals(e.Name));
        Assert.That(m.Id.Equals(e.Id));
        Assert.That(m.Supervisor.Equals(b));
        Assert.IsNull(m.Reports);
    }
}
