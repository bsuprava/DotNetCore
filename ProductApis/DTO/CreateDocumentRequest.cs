using ProductApis.Models;

namespace ProductApis.DTO
{
    public class CreateDocumentRequest
    {
        public string Key { get; set; }
        public List<Product> Products { get; set; }
    }
}
