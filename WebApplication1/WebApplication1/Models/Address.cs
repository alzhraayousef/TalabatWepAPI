namespace WebApplication1.Models
{
    public class Address
    {

        public int ID { get; set; }
        public string? Longitude  { get; set; }
        public string? Latitude { get; set; }
        public string StreetName { get; set; }
        public int ApartmentNo { get; set; }
        public int FloorNo { get; set; }
        public int CityID { get; set; }
        public City City { get; set; }
        public int DistrictID { get; set; }
        public District District { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
