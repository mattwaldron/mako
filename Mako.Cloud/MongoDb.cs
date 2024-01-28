using MongoDB.Driver;

namespace Mako.Cloud;

public class MongoDb<T>
{
    private IMongoCollection<MongoWrapper<T>> _collection;
    public MongoDb(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        _collection = client.GetDatabase(databaseName).GetCollection<MongoWrapper<T>>(collectionName);
    }

    public void Insert(T obj)
    {
        _collection.InsertOne(new MongoWrapper<T> { Content = obj});
    }

    public IEnumerable<Identified<T>> Find()
    {
        return _collection.AsQueryable<MongoWrapper<T>>();
    }

    public void Delete(string id)
    {
        _collection.DeleteOne(a => a.Id == id);
    }
}
