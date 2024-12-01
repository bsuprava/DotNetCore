using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductApis.DbContexts;
using ProductApis.DTO;
using ProductApis.Models;

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

        //get document by key name
        public List<BsonDocument> GetDocumentsByKey(string keyName)
        {
            var filter = Builders<BsonDocument>.Filter.Exists(keyName);
            var documents = _mongoCollection.Find(filter).ToList();
            return documents;
        }

        //create new document
        public async Task CreateDocumentAsync(BsonDocument newDocument)
        {
            await _mongoCollection.InsertOneAsync(newDocument);
        }

        // Update a document
        public async Task<bool> UpdateDocumentAsync(string id, List<Product> products)
        {
            Dictionary<string, List<Product>> keyValuePairs = new Dictionary<string, List<Product>>();
            keyValuePairs.Add(id, products);
            var filter = Builders<BsonDocument>.Filter.Exists(id);
            var result = await _mongoCollection.ReplaceOneAsync(filter, keyValuePairs.ToBsonDocument());

            return result.ModifiedCount > 0;
        }
    }
}
