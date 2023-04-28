
using Microsoft.AspNetCore.Http;

﻿using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public IRepositry<Resturant> restaurantRepo;
        public IRepositry<Category> categoryRepo;
        Context context;
        public CategoryController(IRepositry<Resturant> restaurantRepo, IRepositry<Category> categoryRepo)
        {
            this.restaurantRepo = restaurantRepo;
            this.categoryRepo = categoryRepo;
        }

        [HttpPost]
        public IActionResult New(Category newCat)
        {
            if (ModelState.IsValid)
            {
                categoryRepo.create(newCat);
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status201Created);

            }
            return BadRequest(ModelState);

        }

        [HttpGet("GetAllCtegory")]
        public IActionResult getAll()
        {
            List<Category> categoryList = categoryRepo.getall();
            return Ok(categoryList);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetCategoryByID(int id)
        {
           
            Category category = categoryRepo.getbyid(id);
            return Ok(category.Name);

        }

        [HttpGet("{ResturantId:int}")]
        public IActionResult getCategorybyResturantId(int ResturantId)
        {
            Resturant RestModel = restaurantRepo.getall("ResturantCategories","ApplicationUser").FirstOrDefault(c => c.ID == ResturantId);

            CategoryWithResturantDTO RestDTO = new CategoryWithResturantDTO();
            RestDTO.Id = RestModel.ID;
            RestDTO.ResturantName = RestModel.ApplicationUser.UserName;

            foreach (var item in RestModel.ResturantCategories)
            {
                item.Category = categoryRepo.getall().FirstOrDefault(t => t.ID == item.CategoryID);

                CategoriesDTO cat = new CategoriesDTO();

                cat.Id = item.CategoryID;
                cat.CatergoryName = item.Category.Name;

                RestDTO.CategoryList.Add(cat);
            }
            return Ok(RestDTO);
        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, Category newCat)
        {
            if (ModelState.IsValid)
            {
                Category orgCat = categoryRepo.getbyid(id);

                orgCat.Name = newCat.Name;

                orgCat.Name = newCat.Name; 

                orgCat.IsDeleted = newCat.IsDeleted;
                context.SaveChanges();
                return Ok("Updated");
            }
            return BadRequest(ModelState);

        }


    }
}
