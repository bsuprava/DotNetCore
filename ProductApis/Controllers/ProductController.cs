using Microsoft.AspNetCore.Mvc;
using ProductApis.Repository;

namespace ProductApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly ProductRepository _productRepository;
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsysnc() => Ok(  await _productRepository.GetAllAsync() );

    }
}
