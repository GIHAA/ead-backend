using Microsoft.AspNetCore.Mvc;

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

        // POST: api/order/create
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] OrderModel orderModel)
        {
            try
            {
                _orderService.CreateOrder(orderModel);
                return Ok(new { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/order/all
        [HttpGet]
        public IActionResult GetAllOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (orders, totalOrders) = _orderService.GetAllOrders(pageNumber, pageSize);
                if (orders == null || !orders.Any())
                {
                    return Ok(new { Message = "No orders found." });
                }
                return Ok(new { totalOrders, orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



        // GET: api/order/view/{orderId}
        [HttpGet("view/{orderId}")]
        public IActionResult ViewOrder(string orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return NotFound(new { Message = "Order not found" });

            return Ok(order); 
        }

        // PUT: api/order/update/{orderId}
        [HttpPut("update/{orderId}")]
        public IActionResult UpdateOrder(string orderId, [FromBody] OrderUpdateModel updateModel)
        {
            try
            {
                var existingOrder = _orderService.GetOrderById(orderId);
                if (existingOrder == null)
                    return NotFound(new { Message = "Order not found" });

                // Ensure the order can be updated before dispatch
                if (existingOrder.Status == "Shipped" || existingOrder.Status == "Delivered")
                {
                    return BadRequest(new { Message = "Cannot update order after dispatch" });
                }

                _orderService.UpdateOrder(existingOrder, updateModel);
                return Ok(new { Message = "Order updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/order/cancel/{orderId}
        [HttpPut("cancel/{orderId}")]
        public IActionResult CancelOrder(string orderId)
        {
            try
            {
                var existingOrder = _orderService.GetOrderById(orderId);
                if (existingOrder == null)
                    return NotFound(new { Message = "Order not found" });

                // Ensure the order can only be canceled before dispatch
                if (existingOrder.Status == "Shipped" || existingOrder.Status == "Delivered")
                {
                    return BadRequest(new { Message = "Cannot cancel order after dispatch" });
                }

                _orderService.CancelOrder(existingOrder);
                return Ok(new { Message = "Order canceled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // PUT: api/order/update-status/{orderId}
        [HttpPut("update-status/{orderId}")]
        public IActionResult UpdateOrderStatus(string orderId, [FromBody] OrderStatusUpdateModel statusUpdateModel)
        {
            try
            {
                _orderService.UpdateOrderStatus(orderId, statusUpdateModel.Status);
                return Ok(new { Message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // PUT: api/order/update-item-status/{orderId}/{productId}
        [HttpPut("update-item-status/{orderId}/{productId}")]
        public IActionResult UpdateOrderItemStatus(string orderId, string productId, [FromBody] OrderItemStatusUpdateModel statusUpdateModel)
        {
            try
            {
                _orderService.UpdateOrderItemStatus(orderId, productId, statusUpdateModel.Status);
                return Ok(new { Message = "Order item status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }




    }





}
