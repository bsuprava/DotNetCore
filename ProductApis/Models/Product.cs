namespace ProductApis.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string Img { get; set; } = string.Empty;
    }

    public class men_products: Product { }
    public class women_products : Product { }
}
