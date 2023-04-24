namespace WebApplication1.DTO
{
    public class ProductsWithAllDetailsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int ResturantID { get; set; }
    }
}
