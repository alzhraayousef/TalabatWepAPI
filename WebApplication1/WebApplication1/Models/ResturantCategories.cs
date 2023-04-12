namespace WebApplication1.Models
{
    public class ResturantCategories
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int ResturantID { get; set; }
        public Resturant Resturant { get; set; }

    }
}
