using ProductMinimalApis.Data.Repositories;
using ProductMinimalApis.Models;

namespace ProductMinimalApis.Services
{
    public class CategoryServices
    {
        private readonly IRepository<Categoryies> _categoryRepository;
        public CategoryServices(IRepository<Categoryies> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Categoryies> GetAllProducts() => _categoryRepository.GetAll().ToList();

        public bool AddCategory(Categoryies newCategory)
        {
            try
            {
                _categoryRepository.Add(newCategory);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
    }
}
