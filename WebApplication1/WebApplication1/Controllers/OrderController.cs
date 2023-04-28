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

        IRepositry<OrderProducts> _OrderProducts;
        IRepositry<Order> _order;
        IRepositry<Product> _Product;
        IRepositry<Cart> _cart;
        IRepositry<Customer> _Customer;

        public OrderController(IRepositry<Cart> cart, IRepositry<Order> order, IRepositry<OrderProducts> OrderProducts, IRepositry<Product> Product ,IRepositry<Customer> customer)
        {
            _OrderProducts = OrderProducts;
            _Product = Product;
            _Customer = customer;
            _order= order;
            _cart= cart;
        }

        [HttpGet("id")]
        public ActionResult<OrderDTO> GetById(int id)
        {
            OrderProducts orderProduct = _OrderProducts.getall("Product", "Product.Resturant.ResturantCities", "Order").FirstOrDefault(x => x.ID == id);
            List<OrderProducts> orderProducts = _OrderProducts.getall("Product", "Product.Resturant").ToList();
            float x = 0;
            OrderDTO orderDTO = new OrderDTO();
            orderDTO.ProductName = orderProduct.Product.Name;
            orderDTO.CustomerID = orderProduct.Order.CustomerID;
            orderDTO.ProductID = orderProduct.ProductID;
            orderDTO.Quantity = orderProduct.Quantity;
            orderDTO.Price = orderProduct.Product.Price;
            orderDTO.TotalPrice = orderProduct.Quantity * orderProduct.Product.Price;
            foreach (var item in orderProducts)
            {
                x += item.TotalPrice;

            }
            orderDTO.SubTotal = x;
            orderDTO.DelivaryFee = orderProduct.Product.Resturant.ResturantCities.FirstOrDefault(c=>c.ResturantID==orderDTO.ResturantId).DelivaryFee;
            orderDTO.TotalAmount = (orderDTO.DelivaryFee + orderDTO.SubTotal);
            return Ok(orderDTO);
        }

        [HttpGet]
        public ActionResult<OrderDTO> GetAll()
        {
            List<OrderDTO> orderDTO = new List<OrderDTO>();
            List<OrderProducts> orderProducts = _OrderProducts.getall("Product", "Product.Resturant.ResturantCities").ToList();
            float SubbTotal = 0;
            foreach (OrderProducts order in orderProducts)
            {
                orderDTO.Add(new OrderDTO()
                {
                    ProductName = order.Product.Name,
                    Quantity = order.Quantity,
                    Price = order.Product.Price,
                   //  DelivaryFee = order.Product.Resturant.ResturantCities.FirstOrDefault(c=>c.ResturantID==orderDTO).DelivaryFee,
                    //TotalAmount = order.Product.Resturant.DelivaryFee + SubTotal,
                    TotalPrice = (order.Product.Price * order.Quantity),
                    SubTotal = order.Product.Price,

                }); ;

            }
            return Ok(orderDTO);
        }

        [HttpDelete("id:int")]
        public IActionResult Delete(int id)
        {
            OrderProducts orderProducts = _OrderProducts.getall("Product", "Product.Resturant").FirstOrDefault(o => o.ID == id);
            if (orderProducts != null)
            {
                _OrderProducts.delete(orderProducts);
                
                return Ok("Delete");
            }
            return BadRequest();
        }


        [HttpPost]
        public IActionResult New(OrderProducts OrderProducts)
        {
            OrderDTO OrderDTO = new OrderDTO();
            // Cart cart = new Cart();
            if (ModelState.IsValid)
            {
               //OrderProducts.Product.Name = OrderDTO.ProductName;
               //OrderProducts.ProductID = OrderDTO.ProductID;
                //OrderProducts.Quantity = OrderDTO.Quantity;
               ///OrderProducts.TotalPrice = OrderDTO.TotalPrice;
              /// OrderProducts.Product.Resturant.DelivaryFee = OrderDTO.DelivaryFee;
                _OrderProducts.create(OrderProducts);
                return Ok("Created");
            }
            return Ok();
        }



        [HttpPost]
        public  IActionResult NewOrder(string ApplicationUserId)
        {
            List<OrderProducts> selectedItems = _OrderProducts.getall().FirstOrDefault(c => c.Order.Customer.ApplicationUserId == ApplicationUserId).ToList();
            Order order = new Order()
            {
                Date = DateTime.Now,
                TotalPrice = _cart.getall().Sum(o => o.TotalPrice),
                OrderState = "bending",
                CustomerID= int.Parse(ApplicationUserId),
                IsDeleted= false,
            };
            _order.create(order);

            OrderProducts orderProductsorg;
            foreach(OrderProducts orderProduct in selectedItems)
            {
                orderProductsorg = new OrderProducts();
                orderProductsorg.Product.Name = orderProduct.Product.Name;
                orderProductsorg.OrderID= order.ID;
                orderProductsorg.TotalPrice = orderProduct.TotalPrice;
                orderProductsorg.ProductID=orderProduct.ProductID;
                orderProductsorg.Quantity = orderProduct.Quantity;
                _OrderProducts.delete(orderProduct);
                _OrderProducts.create(orderProductsorg);
            }


            return Ok();
        }



       
    }
}



//IRepositry<Order> orderRepo;
//public OrderController(IRepositry<Order> _orderRepo)
//{
//    orderRepo = _orderRepo;
//}

//[HttpGet]
//public ActionResult<OrderDTO> getAll()
//{
//    List<Order> orders = orderRepo.getall("Customer");


//    List<Order> orders = context.Order.Include(c => c.Customer).Include(p => p.OrderProducts).ToList();

//    List<OrderDTO> orderDTO = new List<OrderDTO>();

//    foreach (var order in orders)
//    {
//        orderDTO.Add(new OrderDTO()
//        {
//            Id = order.ID,
//            CustomerId = order.CustomerID,
//            CustomerName = order.Customer.FirstName,
//            OrderProducts = order.OrderProducts,
//            TotalPrice = order.TotalPrice,
//            OrderState = order.OrderState,
//            isDeleted = order.IsDeleted,
//            date = order.Date,
//        });

//    }
//    return Ok(orderDTO);
//}


//[HttpGet("{id}")]
//public ActionResult<OrderDTO> getById(int id)
//{

//    Order orderModel = orderRepo.getall("Customer").FirstOrDefault(d => d.ID == id);
//    OrderDTO orderDTO = new OrderDTO();
//    orderDTO.Id = orderModel.ID;
//    orderDTO.CustomerId = orderModel.CustomerID;
//    orderDTO.CustomerName = orderModel.Customer.FirstName;
//    orderDTO.OrderProducts = orderModel.OrderProducts;
//    orderDTO.TotalPrice = orderModel.TotalPrice;
//    orderDTO.OrderState = orderModel.OrderState;
//    orderDTO.isDeleted = orderModel.IsDeleted;
//    orderDTO.date = orderModel.Date;


//    return Ok(orderDTO);
//}

//[HttpPost]
//public ActionResult<OrderDTO> New(OrderDTO orderDTO)
//{
//    if (ModelState.IsValid)
//    {
//        Order order = new Order();
//        order.ID = orderDTO.Id;
//        order.CustomerID = orderDTO.CustomerId;
//        order.Date = orderDTO.date;
//        order.TotalPrice = orderDTO.TotalPrice;
//        order.OrderState = orderDTO.OrderState;
//        order.IsDeleted = orderDTO.isDeleted;
//        order.OrderProducts = orderDTO.OrderProducts;

//        orderRepo.create(order);

//        return Ok("Created");
//    }
//    return Ok(orderDTO);

//}
//[HttpPut("{id}")]
//public ActionResult<OrderDTO> edit(int id, OrderDTO orderDTO)
//{
//    if (ModelState.IsValid)
//    {
//        Order order = orderRepo.getall("Customer").FirstOrDefault(d => d.ID == id);
//        order.ID = orderDTO.Id;
//        order.CustomerID = orderDTO.CustomerId;
//        order.Customer.FirstName = orderDTO.CustomerName;
//        order.OrderProducts = orderDTO.OrderProducts;
//        order.TotalPrice = orderDTO.TotalPrice;
//        order.OrderState = orderDTO.OrderState;
//        order.IsDeleted = orderDTO.isDeleted;
//        order.Date = orderDTO.date;

//        return Ok("updated");
//    }
//    return BadRequest(ModelState);
//}
//[HttpDelete("{id}")]
//public ActionResult<OrderDTO> del(OrderDTO orderDTO)
//{
//    Order order = new Order();
//    order.ID = orderDTO.Id;
//    order.CustomerID = orderDTO.CustomerId;
//    order.Date = orderDTO.date;
//    order.TotalPrice = orderDTO.TotalPrice;
//    order.OrderState = orderDTO.OrderState;
//    order.IsDeleted = orderDTO.isDeleted;
//    order.OrderProducts = orderDTO.OrderProducts;
//    orderRepo.delete(order);

//    return Ok("deleted");
//}
