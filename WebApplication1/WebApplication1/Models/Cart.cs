namespace WebApplication1.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
