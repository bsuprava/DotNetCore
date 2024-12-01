using MongoDB.Driver;
using ProductApis.Models;

namespace ProductApis.Repository
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Object> _allCollections;
        
        public ProductRepository(IMongoDatabase mongoDatabase) {
            _allCollections = mongoDatabase.GetCollection<Object>("products");
        }

        public async Task<IEnumerable<Object>> GetAllAsync() => await _allCollections.Find( _ => true).ToListAsync();
        
    }
}
