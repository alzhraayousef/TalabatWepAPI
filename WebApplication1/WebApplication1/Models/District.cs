namespace WebApplication1.Models
{
    public class District
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }
        public City City { get; set; }
    }
}
