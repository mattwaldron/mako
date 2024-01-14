using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mako.Cloud;

[BsonIgnoreExtraElements]
public class MongoWrapper<T> : Identified<T>
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public T Content { get; set; }
}
