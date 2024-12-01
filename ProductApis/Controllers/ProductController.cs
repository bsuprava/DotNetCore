using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductApis.Repository;
using ProductApis.Services;
using System.Text.Json;

namespace ProductApis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;
        public ProductController(ProductRepository productRepository,
            ProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }
        
        [HttpGet]
        [Route("GetAll")]
        //public async Task<IActionResult> GetAllAsysnc() => Ok(  await _productRepository.GetAllAsync() );
        public async Task<IActionResult> GetAllAsysnc()
        {
            var docCollections = await _productRepository.GetAllAsync();
            var docs = docCollections.AsQueryable();
            if (docCollections != null)
                return Ok(docCollections);
            else return BadRequest();
        }

        [HttpGet]
        [Route("GetAllBsonDoc")]
        public async Task<IActionResult> GetAllDocumentsAsync()
        {
            var docCollections =  _productService.GetAllDocuments();
            if (docCollections != null)
            {
                //var json = JsonSerializer.Serialize(documents); return Ok(json);

                var jsonList = docCollections.Select(doc => doc.ToJson()).ToList();
                return Ok(jsonList);
               
            }
            else return BadRequest();
        }


        [HttpGet]
        [Route("GetAllBsonDoc/{Key}")]
        public async Task<IActionResult> GetAllDocumentsByKeyAsync(string Key)
        {
            var docCollections = _productService.GetDocumentsByKey(Key);
            if (docCollections != null)
            {
                /* line no 61 JsonSerializer is unable to serialize Bson data
                 System.InvalidCastException: Unable to cast object of type 'MongoDB.Bson.BsonObjectId' to type 'MongoDB.Bson.BsonBoolean'. at AsBooleanGetter(Object) 
                at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)*/
                //var json = JsonSerializer.Serialize(docCollections); return Ok(json);

                /*below line of code gives success result in serialized text format*/
                //var jsonList = docCollections.Select(doc => doc.ToJson()).ToList();

                /*below line of code gives success result in proper json format*/
                var jsonList = docCollections.Select(doc => doc.ToDictionary()).ToList();
                return Ok(jsonList);

            }
            else return BadRequest();
        }

        //[HttpGet]
        //[Route("GetAlDocuments")]
        //public async Task<IActionResult> GetAllDocumentsAsysnc()
        //{
        //    try
        //    {
        //        var docCollections = await _productRepository.GetAllDocAsync();

        //        foreach (var collection in docCollections)
        //        {
        //            Console.WriteLine($"Collection ID: {collection.ObjId}");
        //            Console.WriteLine("Men Products:");
        //            foreach (var product in collection.MenProducts)
        //            {
        //                Console.WriteLine($" - {product.Name}: {product.Price}");
        //            }

        //            Console.WriteLine("Women Products:");
        //            foreach (var product in collection.WomenProducts)
        //            {
        //                Console.WriteLine($" - {product.Name}: {product.Price}");
        //            }
        //        }

        //        //if(docCollections!=null)
        //        //    return Ok( docCollections );
        //        //else return BadRequest();
        //        return docCollections == null ? NotFound() : Ok(docCollections);
        //        /// Note:
        //        //The NotFound convenience method is invoked as shorthand for return new NotFoundResult();
        //        //The Ok convenience method is invoked as shorthand for return new OkObjectResult(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
