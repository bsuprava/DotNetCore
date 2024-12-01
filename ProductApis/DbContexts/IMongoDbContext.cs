using MongoDB.Bson;
using MongoDB.Driver;

namespace ProductApis.DbContexts
{
    public interface IMongoDbContext
    {
        public IMongoCollection<BsonDocument> GetCollection(string collectionName);
        public IMongoCollection<BsonDocument> GetDocCollection(string collectionName);
       
    }
}
