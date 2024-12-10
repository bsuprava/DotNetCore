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


        public ProductResponse? GetProductsByPage(int page = 1, int pagesize = 5 )
        {
            var products = _productRepository.GetByPage( pagesize, page);
            
            if (products == null)
                return null;

            return new ProductResponse()
            {
                Currentpage= page,
                PageSize =(int) pagesize,
                Products= products
            };
        }

        public class ProductResponse
        {
            public int Currentpage { get; set; }
            public int PageSize { get; set; }
            public IList<Products>? Products { get; set; }
        }
    }
}
