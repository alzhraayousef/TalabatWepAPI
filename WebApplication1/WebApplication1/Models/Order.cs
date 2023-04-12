namespace WebApplication1.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public string OrderState { get; set; }
        public bool IsDeleted { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
