using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
       
        IRepositry<Order> orderRepo;
        public OrderController(IRepositry<Order> _orderRepo)
        {
            orderRepo = _orderRepo;
        }
        
        [HttpGet]
        public ActionResult<OrderDTO> getAll()
        {
            List<Order> orders = orderRepo.getall("Customer");
            

            //List<Order> orders = context.Order.Include(c => c.Customer).Include(p => p.OrderProducts).ToList();

            List<OrderDTO> orderDTO = new List<OrderDTO>();

            foreach (var order in orders)
            {
                orderDTO.Add(new OrderDTO()
                {
                    Id = order.ID,
                    CustomerId = order.CustomerID,
                    CustomerName = order.Customer.FirstName,
                    OrderProducts=order.OrderProducts,
                    TotalPrice = order.TotalPrice,
                    OrderState = order.OrderState,
                    isDeleted = order.IsDeleted,
                    date = order.Date,
                }) ;

            }
            return Ok(orderDTO);
        }


        [HttpGet("{id}")]
        public ActionResult<OrderDTO> getById(int id)
        {
            
            Order orderModel = orderRepo.getall("Customer").FirstOrDefault(d => d.ID == id);
            OrderDTO orderDTO = new OrderDTO();
            orderDTO.Id = orderModel.ID;
            orderDTO.CustomerId = orderModel.CustomerID;
            orderDTO.CustomerName = orderModel.Customer.FirstName;
            orderDTO.OrderProducts = orderModel.OrderProducts;
            orderDTO.TotalPrice = orderModel.TotalPrice;
            orderDTO.OrderState = orderModel.OrderState;
            orderDTO.isDeleted = orderModel.IsDeleted;
            orderDTO.date = orderModel.Date;


            return Ok(orderDTO);
        }
     
        [HttpPost]
        public ActionResult<OrderDTO> New(OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.ID = orderDTO.Id;
                order.CustomerID = orderDTO.CustomerId;
                order.Date = orderDTO.date;
                order.TotalPrice= orderDTO.TotalPrice;
                order.OrderState = orderDTO.OrderState;
                order.IsDeleted = orderDTO.isDeleted;
                order.OrderProducts=orderDTO.OrderProducts;

                orderRepo.create(order);

                return Ok("Created");
            }
            return Ok(orderDTO);

        }
        [HttpPut("{id}")]
        public ActionResult<OrderDTO> edit(int id, OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                Order order = orderRepo.getall("Customer").FirstOrDefault(d => d.ID == id);
                order.ID = orderDTO.Id;
                order.CustomerID = orderDTO.CustomerId;
                order.Customer.FirstName = orderDTO.CustomerName;
                order.OrderProducts = orderDTO.OrderProducts;
                order.TotalPrice= orderDTO.TotalPrice;
                order.OrderState = orderDTO.OrderState;
                order.IsDeleted = orderDTO.isDeleted;
                order.Date= orderDTO.date;

                return Ok("updated");
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public ActionResult<OrderDTO> del(OrderDTO orderDTO)
        {
            Order order = new Order();
            order.ID = orderDTO.Id;
            order.CustomerID = orderDTO.CustomerId;
            order.Date = orderDTO.date;
            order.TotalPrice = orderDTO.TotalPrice;
            order.OrderState = orderDTO.OrderState;
            order.IsDeleted = orderDTO.isDeleted;
            order.OrderProducts = orderDTO.OrderProducts;
            orderRepo.delete(order);

            return Ok("deleted");
        }

    }
}
