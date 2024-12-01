using MongoDB.Bson;
using MongoDB.Driver;
using ProductApis.DbContexts;

namespace ProductApis.Services
{
    public class ProductService
    {
        private readonly IMongoDbContext _mongoContext;
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        public ProductService(IMongoDbContext mongoContext) 
        {
            _mongoContext = mongoContext;//new MongoDBContext("your_connection_string", "your_database_name");
            _mongoCollection = _mongoContext.GetCollection("products");
        }

        public List<BsonDocument> GetAllDocuments()
        {           
            var collection = _mongoContext.GetCollection("products");
            var documents = collection.Find(new BsonDocument()).ToList();
            return documents;
        }

        public List<BsonDocument> GetDocumentsByKey(string keyName)
        {
            var filter = Builders<BsonDocument>.Filter.Exists(keyName);
            var documents = _mongoCollection.Find(filter).ToList();
            return documents;
        }
    }
}
