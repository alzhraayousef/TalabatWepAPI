namespace WebApplication1.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public List<ResturantCategories> ResturantCategories { get; set; }
        public List<Product> Products { get; set; }

    }
}
