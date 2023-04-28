using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        IRepositry<Cart> cartRepo;
        IRepositry<Product> productRepo;
        
        public CartController(IRepositry<Cart> _cartRepo, IRepositry<Product> _productRepo)
        {
            productRepo = _productRepo;
            cartRepo = _cartRepo;
            
        }
        
        [HttpGet]
        public ActionResult<CartDTO> getCartByProduct(Product product)
        {
            CartDTO cartDTO = new CartDTO();
            product = productRepo.getall("Resturant.ResturantCities", "Resturant.ApplicationUser").FirstOrDefault();
            cartDTO.ResturantId = product.ResturantID;
            cartDTO.ResturantName = product.Resturant.ApplicationUser.UserName;
            cartDTO.ProductName = product.Name;
            cartDTO.ProductPrice = product.Price;
            cartDTO.delivaryFee = product.Resturant.ResturantCities
                .FirstOrDefault(c => c.ResturantID == cartDTO.ResturantId).DelivaryFee;

            return Ok(cartDTO);
        }

        //[HttpGet]
        //[Route("productId")]
        //public ActionResult<CartDTO> getProductWithQuantity(int productId)
        //{
        //    Cart cart = cartRepo.getall("Product.Resturant.ApplicationUser", "Product.Resturant.ResturantCities")
        //        .FirstOrDefault(c=>c.ProductID== productId);
        //    int ResturantId = cart.Product.ResturantID;
        //    CartDTO cartDTO = new CartDTO();
        //    cartDTO.ResturantName = cart.Product.Resturant.ApplicationUser.UserName;
        //    cartDTO.ProductName = cart.Product.Name;
        //    cartDTO.Quantity = cart.Quantity;
        //    cartDTO.ProductPrice = cart.TotalPrice* cart.Quantity;
        //    cartDTO.delivaryFee = cart.Product.Resturant.ResturantCities
        //        .FirstOrDefault(c=>c.ResturantID == ResturantId).DelivaryFee;
        //    return Ok(cartDTO);
        //}

        [HttpPost]
        public ActionResult<CartDTO> New(CartDTO cartDTO)
        {
            if (ModelState.IsValid)
            {
                Cart cart = new Cart();
                cart.ProductID = cartDTO.ProductID;
                cart.CustomerID = cartDTO.CustomerID;
                cart.TotalPrice = cartDTO.ProductPrice;
                cart.Quantity = cartDTO.Quantity;
                cartRepo.create(cart);
                return Ok("Created");
            }
            return Ok(cartDTO);
        }
        [HttpPut("{id}")]
        public IActionResult Edit(int id, CartDTO cartDTO)
        {
            if (ModelState.IsValid)
            {
                Cart cart = cartRepo.getbyid(id);
                cart.CustomerID = cartDTO.CustomerID;
                cart.ProductID = cartDTO.ProductID;
                cart.TotalPrice = cartDTO.ProductPrice;
                cart.Quantity = cartDTO.Quantity;
                cartRepo.update(cart);
                return Ok("updated");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public ActionResult<CartDTO> del(int id)
        {
            Cart cart = cartRepo.getbyid(id);
            cartRepo.delete(cart);

            return Ok("deleted");
        }

    }
}
