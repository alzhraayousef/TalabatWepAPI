using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        Context context;
        public ProductController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        
        public ActionResult GetAllProduct()
        {
            List<Product> Products = context.Product.Include(c=>c.Resturant).Include(c=>c.Category).ToList();
 
            return Ok(Products);


        }
        [HttpPost]//api/Department Post
        public IActionResult New(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                context.Product.Add(newProduct);
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status201Created);

            }
            return BadRequest(ModelState);
        }
        //[HttpGet]
        //public ActionResult GetAllProduct()
        //{
        //    List<Product> Products = context.Product.Include(c=>c.Category).ToList();
        //    List<ProductwithCategoryDTO> productDTO = new List<ProductwithCategoryDTO>();
        //    productDTO.ID = Product.ID;
        //    productDTO.Name = Product.Name;
        //    productDTO.ResturantID = Product.ResturantID;

        //    productDTO.CategoryID = Product.CategoryID;
        //    productDTO.Category = Product.Category.Name;
        //    productDTO.Description = Product.Description;
        //    productDTO.Image = Product.Image;
        //    productDTO.Price = Product.Price;
        //    productDTO.IsDeleted = Product.IsDeleted;
        //    return Ok(productDTO);

        //}
        [HttpGet("{id:int}")]
        public ActionResult ShowAllByPrdID(int id)
        {
            Product Productmodel = context.Product.Include(c => c.Category).Include(r => r.Resturant).FirstOrDefault(e=>e.ID==id);
            ProductwithCategoryDTO productDTO = new ProductwithCategoryDTO();
            productDTO.ID = Productmodel.ID;
            productDTO.Name = Productmodel.Name;
            productDTO.ResturantID = Productmodel.ResturantID;
            //productDTO.Resturant = Productmodel.Resturant.ApplicationUser.UserName;
            productDTO.CategoryID = Productmodel.CategoryID;
            productDTO.Category = Productmodel.Category.Name;
            productDTO.Description = Productmodel.Description;
            productDTO.Image = Productmodel.Image;
            productDTO.Price = Productmodel.Price;
            productDTO.IsDeleted = Productmodel.IsDeleted;
            return Ok(productDTO);
            //Include(c => c.Category).Include(r => r.Resturant).

        }
        //[HttpGet("{id:int}")]
        //public ActionResult GetProductsByCatId(int id)
        //{
        //    List<Product> Products = context.Product.Where(p=>p.CategoryID==id).ToList();

        //    return Ok(Products);

        //}
        

        
    }
}
