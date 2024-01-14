using NUnit.Framework;
using Mako.Cloud;
using Mako.Common;
using System;
using System.Linq;

namespace Mako.Test.Cloud;

public class MongoDbTests
{
    public class GameResult
    {
        public string? Winner { get; set; }
        public string? Game { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string[]? OtherPlayers { get; set; } 
    }

    MongoDb<GameResult> _db;

    [SetUp]
    public void Setup()
    {
        _db = new MongoDb<GameResult>("mongodb+srv://matt:<password>@gamecloset1.3cjtvrf.mongodb.net/?retryWrites=true&w=majority", "game_data", "finished_results");
    }

    [Test]
    public void AddResultToDatabase()
    {
        var now = DateTime.UtcNow;
        now = DateTime.Parse(now.ToString("u")).ToUniversalTime();
        _db.Insert(new GameResult
        {
            Winner = "matt",
            Game = "TicTacToe",
            StartTime = now.Subtract(TimeSpan.FromMinutes(5)),
            EndTime = now,
            OtherPlayers = new [] {"loser" }
        });

        var results = _db.Find().Where(r => r.Content.EndTime == now).StripId().ToList();
        Assert.That(results.Count.Equals(1));
    }

    [Test]
    public void ListAll()
    {
        var results = _db.Find().StripId().ToList();
        Console.WriteLine($"{results.Count} entries in collection");
        foreach (var r in results)
        {
            Console.WriteLine(r.ToJson());
        }
    }

    [Test]
    public void DeleteAll()
    {
        var results = _db.Find().ToList();
        Console.WriteLine($"Deleting {results.Count} entries");
        foreach (var r in results)
        {
            _db.Delete(r.Id);
        }
    }
}