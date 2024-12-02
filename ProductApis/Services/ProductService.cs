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

        // patch a document
        public async Task<long> PatchDocumentAsync(string id, List<Product> products)
        {
            //Dictionary<string, List<Product>> keyValuePairs = new Dictionary<string, List<Product>>();
            //keyValuePairs.Add(id, products.ToBsonDocument());//here products.ToBsonDocument() gives compilation error for list to bson conversion

            //var filter = Builders<BsonDocument>.Filter.Eq(id, new BsonDocument());
            //var updateDefinition = Builders<BsonDocument>.Update.Combine(
            //    keyValuePairs.Select(update => Builders<BsonDocument>.Update.Set(update.Key, BsonValue.Create(update.Value)))
            //);

            //create filter
            var filter = Builders<BsonDocument>.Filter.Exists(id);

            // Create update definitions
            var updateDefinitions = new List<UpdateDefinition<BsonDocument>>();

            // Handle lists of Product objects
            var bsonArray = new BsonArray(products.Select(p => p.ToBsonDocument()));
            updateDefinitions.Add(Builders<BsonDocument>.Update.Set(id, bsonArray));

            // Combine all update definitions
            var updateDefinition = Builders<BsonDocument>.Update.Combine(updateDefinitions);
           

            var result = await _mongoCollection.UpdateOneAsync(filter, updateDefinition);
            return result.ModifiedCount;
        }

        //delete document by key name
        public async Task<DeleteResult> DeleteDocumentsByKey(string keyName)
        {
            var filter = Builders<BsonDocument>.Filter.Exists(keyName);
            var deleteResult = await _mongoCollection.DeleteOneAsync(filter);
            return deleteResult;
        }
    }
}
