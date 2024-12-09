namespace ProductMinimalApis.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<Products>? Products { get; set; }
    }
}
