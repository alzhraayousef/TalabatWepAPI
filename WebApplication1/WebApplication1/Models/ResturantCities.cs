namespace WebApplication1.Models
{
    public class ResturantCities
    {
        public int ID { get; set; }
        public int CityID { get; set; }
        public City City { get; set; }
        public int ResturantID { get; set; }
        public Resturant Resturant { get; set; }
    }
}
