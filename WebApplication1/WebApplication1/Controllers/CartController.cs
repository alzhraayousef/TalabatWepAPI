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
        IRepositry<Customer> customerRepo;
        public CartController(IRepositry<Cart> _cartRepo, IRepositry<Product> _productRepo,
            IRepositry<Customer> _customerRepo)
        {
            productRepo = _productRepo;
            cartRepo = _cartRepo;
            customerRepo= _customerRepo;

        }



    [HttpPost]
        public ActionResult<CartDTO> AddToCart(int ProductID, string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Product product = productRepo.getall().FirstOrDefault(p => p.ID == ProductID);

            List<Cart> cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customer.ID).ToList();
            Cart cart;
            bool flagResturentDound = false;
            bool flagProductFound = false;
            if (cartList.Count>0)
            {
                foreach(Cart item in cartList)
                {
                    if(item.Product.ResturantID==product.ResturantID)
                    {
                        flagResturentDound = true;
                    }
                    if(item.ProductID==ProductID)
                    {
                        flagProductFound = true;
                    }
                }
            }
            if (cartList.Count <= 0 || flagResturentDound)
            {
                if (flagProductFound)
                {
                    cart = cartRepo.getall().FirstOrDefault(c => c.ProductID == ProductID&&c.CustomerID==customer.ID);
                    cart.Quantity += 1;
                    cart.TotalPrice = cart.Quantity * product.Price;
                    cartRepo.update(cart);
                }
                else
                {
                    cart = new Cart
                    {
                        CustomerID = customer.ID,
                        ProductID = product.ID,
                        Quantity = 1,
                        TotalPrice = product.Price
                    };
                    cartRepo.create(cart);
                }
            }
            else
            {
                return BadRequest("Resturant Not Exist");
            }

            return Ok("Successfully Added To Cart");
        }

        [HttpGet("customerID")]
        public IActionResult GetCartByCustomerId(int customerID)
        {
            List<Cart> cartList = cartRepo.getall("Product").Where(c => c.CustomerID == customerID).ToList();
            return Ok(cartList);
        }

        [HttpPut]
        public IActionResult EditCart(int ProductID, int quantity, string ApplicationUserId)
        {
            Customer customer = customerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == ApplicationUserId);
            Cart cart = cartRepo.getall().FirstOrDefault(c => c.ProductID == ProductID && c.CustomerID == customer.ID);
            Product product = productRepo.getall().FirstOrDefault(p=>p.ID == ProductID);
            cart.Quantity=quantity;
            cart.TotalPrice = product.Price * cart.Quantity;
            cartRepo.update(cart);
            return Ok(cart.TotalPrice);
        }

        [HttpDelete("{id}")]
        public ActionResult<CartDTO> deleteFromCart(int id)
        {
            Cart cart = cartRepo.getbyid(id);
            cartRepo.delete(cart);

            return Ok("deleted");
        }

    }
}
