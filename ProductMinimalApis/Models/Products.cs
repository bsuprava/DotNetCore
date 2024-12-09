namespace ProductMinimalApis.Models
{
    // Dependent (child)
    public class Products
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }// Required foreign key property
        public Categoryies Category { get; set; } = null;// Required reference navigation to principal
    }
}
