
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        public float TotalPrice { get; set; } 
        public string OrderState { get; set; }
        public bool isDeleted { get; set; }
        public DateTime date { get; set; }

        //public int ProductId { get; set; }
        //public string ProductName { get; set; }

    }
}
