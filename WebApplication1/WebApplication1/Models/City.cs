namespace WebApplication1.Models
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ResturantCities> ResturantCities { get; set; }
        public List<District> Districts { get; set; }
    }
}
