using MongoDB.Bson;
using MongoDB.Driver;

namespace ProductApis.DbContexts
{
    public class MongoDbContext: IMongoDbContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDataBase;
        private readonly IMongoDatabase _mongoDataBase2;
        //
        public MongoDbContext(
            IMongoDatabase mongoDataBase, 
            IMongoClient mongoClient)
        {
            _mongoDataBase = mongoDataBase;
            _mongoClient = mongoClient;            
            _mongoDataBase2 = mongoClient.GetDatabase("myproductdb");
        }

        public IMongoCollection<BsonDocument> GetCollection(string collectionName) 
        {
            return _mongoDataBase.GetCollection<BsonDocument>(collectionName); 
        }

        public IMongoCollection<BsonDocument> GetDocCollection(string collectionName)
        {
            return _mongoDataBase2.GetCollection<BsonDocument>(collectionName);
        }

    }
}
