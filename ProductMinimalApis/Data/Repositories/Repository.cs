using Microsoft.EntityFrameworkCore;
using ProductMinimalApis.Models;

namespace ProductMinimalApis.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context; 
        protected readonly DbSet<T> _entities; 

        public Repository(AppDbContext context) 
        { 
            _context = context; 
            _entities = context.Set<T>(); 
        }
        public IEnumerable<T> GetAll() { return _entities.ToList(); }
        public T GetById(int id) { return _entities.Find(id); }
        public void Add(T entity) { _entities.Add(entity); Save(); }
        public void Update(T entity) { _entities.Update(entity); Save(); }
        public void Delete(int id) { var entity = _entities.Find(id); _entities.Remove(entity); Save(); }
        public void Save() { _context.SaveChanges(); }
    }

    //public class ProductRepository : Repository<Product>, IProductRepository { public ProductRepository(AppDbContext context) : base(context) { } }
    //public class CategoryRepository : Repository<Category>, ICategoryRepository { public CategoryRepository(AppDbContext context) : base(context) { } }
    //public class OrderRepository : Repository<Order>, IOrderRepository { public OrderRepository(AppDbContext context) : base(context) { } }
}
