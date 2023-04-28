namespace WebApplication1.Models
{
    public class Resturant
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public float MinOrderAmmount { get; set; }
        public string WorkingHours { get;}
        public bool IsDeleted { get; set; }
        public List<ResturantCategories> ResturantCategories { get; set; }
        public List<Product> Products { get; set; }
        public List<ResturantCities> ResturantCities { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
