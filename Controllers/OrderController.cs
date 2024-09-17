using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderModel model)
        {
            var order = new Order
            {
                CustomerId = model.CustomerId,
                OrderDate = model.OrderDate,
                TotalAmount = model.TotalAmount,
                DeliveryAddress = model.DeliveryAddress,
                DeliveryStatus = model.DeliveryStatus,
            };

            var newOrder = _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }

        // GET: api/order
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();
            return Ok(orders);
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrder(string id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(string id, [FromBody] Order updatedOrder)
        {
            var existingOrder = _orderService.GetOrderById(id);
            if (existingOrder == null)
                return NotFound(new { Message = "Order not found" });

            _orderService.UpdateOrder(id, updatedOrder);
            return Ok(new { Message = "Order updated successfully" });
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound(new { Message = "Order not found" });

            _orderService.DeleteOrder(id);
            return Ok(new { Message = "Order deleted successfully" });
        }
    }
}
