using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class CartDTO
    {
        public string ResturantName { get; set; }
        public string ProductName { get; set; }
        public int CityID { get; set; }
        public int ResturantId { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public float ProductPrice { get; set; }
        public float TotalPrice { get; set; }
        public int Quantity { get; set; }
        public float delivaryFee { get; set; }
    }
}
