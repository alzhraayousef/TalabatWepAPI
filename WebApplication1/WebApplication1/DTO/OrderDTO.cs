
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class OrderDTO
    {
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int ResturantId { get; set; }

        public int Quantity { get; set; }
        public float TotalPrice { get; set; }

        public float SubTotal { get; set; }

        public float DelivaryFee { get; set; }
        public float TotalAmount { get; set; }
        public int ProductID { get; internal set; }
        public object CustomerID { get; internal set; }

        //public int ProductId { get; set; }
        //public string ProductName { get; set; }

    }
}
