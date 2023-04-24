using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<City> cityRepo;
        Context context;
       
        public CityController(IRepositry<Resturant> restaurantRepo, IRepositry<City> cityRepo)
        {
            this.restaurantRepo = restaurantRepo;
            this.cityRepo = cityRepo;
        }
        [HttpPost]
        public IActionResult New(City newCity)
        {
            if (ModelState.IsValid) 
            {
                cityRepo.create(newCity);
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status201Created);

            }
            return BadRequest(ModelState);

        }

        [HttpGet("GetAllCities")]
        public IActionResult getAll()
        {
            List<City> cityList = cityRepo.getall();
            return Ok(cityList);
        }


        [HttpGet("{id:int}")]
        public IActionResult getRestaurantbyCityId(int id)
        {
            City CityModel = cityRepo.getall("ResturantCities").FirstOrDefault(c => c.ID == id);
            RestaurantWithCityDTO CityDTO = new RestaurantWithCityDTO();
            CityDTO.Id = CityModel.ID;
            CityDTO.CityName = CityModel.Name;

            foreach (var item in CityModel.ResturantCities)
            {
                item.Resturant = restaurantRepo.getall("ApplicationUser").FirstOrDefault(t => t.ID == item.ResturantID);
                ResturantsDTO res = new ResturantsDTO();
                res.Id = item.ResturantID;
                res.RestaurantName = item.Resturant.ApplicationUser.UserName;

                CityDTO.RestList.Add(res);
            }
            return Ok(CityDTO);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, City newCity)
        {
            if (ModelState.IsValid)
            {
                City orgCity = cityRepo.getbyid(id);
                orgCity.Name =newCity.Name;

                context.SaveChanges();
                return Ok("Updated");
            }
            return BadRequest(ModelState);

        }



    }
}