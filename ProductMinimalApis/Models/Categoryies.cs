namespace ProductMinimalApis.Models
{
    // Principal (parent)
    public class Categoryies
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Products> Products { get; set; } = new List<Products>();// Collection navigation containing dependents
    }
}
