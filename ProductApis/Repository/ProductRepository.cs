using MongoDB.Driver;
using ProductApis.Models;

namespace ProductApis.Repository
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Object> _productCollections;
       // private readonly IMongoCollection<Product> _menProducts;
       //private readonly IMongoCollection<Product> _womenProducts;
        public ProductRepository(IMongoDatabase mongoDatabase) {
            _productCollections = mongoDatabase.GetCollection<Object>("products");
        }

        public async Task<IEnumerable<Object>> GetAllAsync() => await _productCollections.Find( _ => true).ToListAsync(); 
    }
}
