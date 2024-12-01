using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductApis.Models
{
    //Not Alligned with Mongodb collection
    //public class Collection
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }

    //    //[BsonElement("men_products")]
    //    //[BsonRepresentation(BsonType.Array)]
    //    //public men_products MensProduct { get; set; }

    //    //[BsonElement("women_products")]
    //    //[BsonRepresentation(BsonType.Array)]
    //    //public women_products WomensProduct { get; set; }

    //    [BsonRepresentation(BsonType.Array)]
    //    public Product Products { get; set; }
    //}

    public class Collection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("men_products")]
        [BsonRepresentation(BsonType.Array)]
        public List<Product> MenProducts { get; set; } = new();

        [BsonElement("women_products")]
        [BsonRepresentation(BsonType.Array)]
        public List<Product> WomenProducts { get; set; } = new();
    }

    public class Product
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

       [BsonElement("price")]
        public decimal Price { get; set; } = decimal.Zero;
        
        [BsonElement("img")]
        public string Img { get; set; } = string.Empty;
    }

    public class men_products: Product { }
    public class women_products : Product { }
}
