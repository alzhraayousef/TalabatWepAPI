using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class RestaurantWithCityDTO
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public List<ResturantsDTO> RestList { get; set; } = new List<ResturantsDTO>();
    }
    public class ResturantsDTO
    {
        public int Id { get; set;}
        public string RestaurantName { get; set; }
        
    }
}
