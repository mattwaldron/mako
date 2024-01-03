using MongoDB.Bson;
using MongoDB.Driver;

namespace Mako.Cloud;

public class MongoDb<T> where T : Identifiable
{
    private IMongoCollection<T> _collection;
    public MongoDb(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        _collection = client.GetDatabase(databaseName).GetCollection<T>(collectionName);
    }

    public void Insert(T obj)
    {
        _collection.InsertOne(obj);
    }

    public IEnumerable<T> FindAll()
    {
        return _collection.AsQueryable<T>();
    }

    public void Delete(ObjectId id)
    {
        _collection.DeleteOne(a => a.Id == id);
    }
}
