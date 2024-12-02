using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductApis.DTO;
using ProductApis.Models;
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
            //step1: Input validation
            if (string.IsNullOrEmpty(Key)) return BadRequest();

            //step2: host filtering  or allowlist implemented in appsettings.json

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

        /// <summary>
        /// The HEAD method retrieves metadata (headers) for a resource without the body.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpHead("{key}")]
        public IActionResult DocumentExistsAsync(string key)
        {
            try
            {
                var exists = _productService.GetDocumentsByKey(key);
                if (exists!=null)
                {
                    Response.Headers.Add("Resource-Exists", "true");
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// The OPTIONS method returns the supported HTTP methods for the resource.
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, TRACE");
            return Ok();
        }

        ///// <summary>
        ///// The TRACE method is used for debugging purposes. It echoes the request back to the client.
        ///// </summary>
        ///// <returns></returns>
        //[HttpTrace]
        //public IActionResult Trace()
        //{
        //    var traceDetails = new
        //    {
        //        Method = HttpContext.Request.Method,
        //        Path = HttpContext.Request.Path,
        //        Headers = HttpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
        //    };

        //    return Ok(traceDetails);
        //}


        /// <summary>
        /// Create Document under specified collection
        /// </summary>
        /// <param name="newDocument"></param>
        /// <returns></returns>
                 /* Enter below payload format for the endpoint
                 {
                  "key": "kids_products",
                  "products":[
                           { "id": 11, "name": "black casual Tshirt", "img": "KidCloth1", "price": 500 },
                           { "id": 12, "name": "Black winter jacket", "img": "KidCloth2", "price": 1500 },
                           { "id": 13, "name": "black casual Tshirt", "img": "KidCloth3", "price": 500 },
                           { "id": 14, "name": "Black winter jacket", "img": "KidCloth4", "price": 1500 },
	                   { "id": 15, "name": "black casual Tshirt", "img": "KidCloth5", "price": 500 },
                           { "id": 16, "name": "Black winter jacket", "img": "KidCloth6", "price": 1500 },
	                   { "id": 17, "name": "black casual Tshirt", "img": "KidCloth7", "price": 500 },
                           { "id": 18, "name": "Black winter jacket", "img": "KidCloth8", "price": 1500 },
	                   { "id": 19, "name": "black casual Tshirt", "img": "KidCloth9", "price": 500 },
                           { "id": 10, "name": "Black winter jacket", "img": "KidCloth10", "price": 1500 }
                  ]
                }

        After dropping men_products.id index then it creates new document 
                */
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentRequest createDocumentRequest)
        {
            try
            {
                Dictionary<string, List<Product>> keyValuePairs = new Dictionary<string, List<Product>>();
                keyValuePairs.Add(createDocumentRequest.Key, createDocumentRequest.Products);
                await _productService.CreateDocumentAsync(keyValuePairs.ToBsonDocument());
                //return Ok("Document created successfully.");
                return StatusCode(201, "Document created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// An optimal way to separate data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateNew")]
        //public async Task<IActionResult> CreateDocumentByQrerystring([FromBody] CreateDocumentRequest createDocumentRequest)
        public async Task<IActionResult> CreateDocument([FromQuery] string key, [FromBody] List<Product> products)
        {
            try
            {
                Dictionary<string, List<Product>> keyValuePairs = new Dictionary<string, List<Product>>();
                keyValuePairs.Add(key, products);
                await _productService.CreateDocumentAsync(keyValuePairs.ToBsonDocument());
                return Ok("Document created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// update document
        /// </summary>
        /// <param name="key"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        /* payload
         {
  "key": "kids_products",
  "products": [
           { "id": 11, "name": "White casual Tshirt", "img": "KidCloth1", "price": 500 },
           { "id": 12, "name": "Black winter jacket", "img": "KidCloth2", "price": 1500 },
           { "id": 13, "name": "Blue casual Tshirt", "img": "KidCloth3", "price": 500 },
           { "id": 14, "name": "Black winter jacket", "img": "KidCloth4", "price": 1500 },
	   { "id": 15, "name": "Dark casual Tshirt", "img": "KidCloth5", "price": 500 },
           { "id": 16, "name": "Black winter jacket", "img": "KidCloth6", "price": 1500 },
	   { "id": 17, "name": "Red casual Tshirt", "img": "KidCloth7", "price": 500 },
           { "id": 18, "name": "Black winter jacket", "img": "KidCloth8", "price": 1500 },
	   { "id": 19, "name": "Green casual Tshirt", "img": "KidCloth9", "price": 500 },
           { "id": 10, "name": "Black winter jacket", "img": "KidCloth10", "price": 1500 }
  ]
}
         */
        [HttpPut("update/{key}")]
        public async Task<IActionResult> UpdateDocument( string key, [FromBody] List<Product> products)
        {
            try
            {
                var success = await _productService.UpdateDocumentAsync(key, products);
                if (success)
                    return Ok("Document updated successfully.");
                else
                    return NotFound($"Document with ID {key} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("delete/{key}")]
        public async Task<IActionResult> DeleteDocument(string key)
        {
            try
            {
                var result = await _productService.DeleteDocumentsByKey(key);
                if (result != null && result.IsAcknowledged == true)
                {
                    return Ok("Document deleted successfully. Deleted count:" + result.DeletedCount);
                }                   
                else
                    return NotFound($"Document with ID {key} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PATCH: Partial update
        [HttpPatch("patch/{key}")]
        public async Task<IActionResult> PatchDocument(string key, [FromBody] List<Product> products)
        {
            try
            {
               
                var result = await _productService.PatchDocumentAsync(key,products);

                if (result > 0)
                    return Ok("Document patched successfully.Patch count:" + result);
                else
                    return NotFound("No matching document found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Patch failed for key "+ key + ", Detail : " + ex.Message);
                return StatusCode(500, "Patch failed for key " + key);
            }
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
