using ProductMinimalApis.Models;

namespace ProductMinimalApis.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(); 
        T? GetById(int id); 
        void Add(T entity); 
        void Update(T entity); 
        void Delete(int id); 
        void Save();
        IList<T>? GetByPage(decimal pagesize = 5, int page = 1);
    }

    //public interface IProductRepository : IRepository<Product> { }
    //public interface ICategoryRepository : IRepository<Category> { }
    //public interface IOrderRepository : IRepository<Order> { }
}
