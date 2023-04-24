﻿//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Models;

//namespace WebApplication1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ResturantController : ControllerBase
//    {
//        Context context;
//        public ResturantController(Context context)
//        {
//            this.context = context;
//        }
//        [HttpGet]
//        //[Authorize]//jwt
//        public ActionResult GetAllResturant()
//        {
//            List<Resturant> resturants = context.Resturant.Include(c=>c.Products).Include(c=>c.ResturantCategories).ToList();

//            return Ok(resturants);

//        }
//        [HttpGet("{id:int}")]
//        //[Authorize]//jwt
//        public ActionResult GetResturantDetails(int id)
//        {
//            Resturant resturant = context.Resturant.FirstOrDefault(R => R.ID == id);

//            return Ok(resturant);

//        }

//    }
//}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResturantController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<City> cityRepo;
        Context context;
        public ResturantController(IRepositry<Resturant> restaurantRepo, IRepositry<City> cityRepo)
        {
            this.restaurantRepo = restaurantRepo;
            this.cityRepo = cityRepo;
        }

        [HttpGet("GetAllRestaurant")]
        public IActionResult getAll()
        {
            List<Resturant> restaurantList = restaurantRepo.getall("ApplicationUser", "ResturantCategories");
            return Ok(restaurantList);
        }
      

            [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, Resturant newRest)
        {
            if (ModelState.IsValid)
            {
                Resturant orgRest = restaurantRepo.getbyid(id);
                orgRest.ApplicationUser.UserName = newRest.ApplicationUser.UserName;
                orgRest.Image = newRest.Image;
                orgRest.MinOrderAmmount = newRest.MinOrderAmmount;

                context.SaveChanges();
                return Ok("Updated");
            }
            return BadRequest(ModelState);

        }



    }
}
