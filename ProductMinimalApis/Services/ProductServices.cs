using ProductMinimalApis.Data.Repositories;
using ProductMinimalApis.Models;

namespace ProductMinimalApis.Services
{
    public class ProductServices
    {
        private readonly IRepository<Products> _productRepository;
        public ProductServices(IRepository<Products> productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Products> GetAllProducts() => _productRepository.GetAll().ToList();

        public Products GetProduct(int id) => _productRepository.GetById(id);

        
        public bool AddProduct(Products newproduct)
        {
            try
            {
                _productRepository.Add(newproduct);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public bool DeleteProductById(int id)
        {
            try
            {
                _productRepository.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public bool UpdateProductById(int id, Products existproduct)
        {
            try
            {
                var product = GetProduct(id);
                product.Name = existproduct.Name;
                product.Price = existproduct.Price;
                product.CategoryId = existproduct.CategoryId;
                _productRepository.Update(product);
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
