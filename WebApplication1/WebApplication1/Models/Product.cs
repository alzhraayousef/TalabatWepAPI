namespace WebApplication1.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int ResturantID { get; set; }
        public Resturant Resturant { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }
        public List<Cart> Cart { get; set; }



    }
}
