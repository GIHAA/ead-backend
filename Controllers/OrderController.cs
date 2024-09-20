using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                await _orderService.CreateOrderAsync(createOrderDto);
                return Ok(new { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 10, string customerId = null)
        {
            try
            {
                var (orders, totalOrders) = await _orderService.GetAllOrdersAsync(pageNumber, pageSize, customerId);
                if (orders == null || !orders.Any()) return Ok(new { Message = "No orders found." });

                return Ok(new { totalOrders, orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("view/{orderId}")]
        public async Task<IActionResult> ViewOrder(string orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound(new { Message = "Order not found" });

            return Ok(order);
        }

        [HttpPut("update/{orderId}")]
        public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderUpdateDto updateDto)
        {
            try
            {
                await _orderService.UpdateOrderAsync(orderId, updateDto);
                return Ok(new { Message = "Order updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId);
                return Ok(new { Message = "Order canceled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] OrderStatusUpdateDto statusUpdateDto)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, statusUpdateDto.Status);
                return Ok(new { Message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update-item-status/{orderId}/{productId}")]
        public async Task<IActionResult> UpdateOrderItemStatus(string orderId, string productId, [FromBody] OrderItemStatusUpdateDto statusUpdateDto)
        {
            try
            {
                await _orderService.UpdateOrderItemStatusAsync(orderId, productId, statusUpdateDto.Status);
                return Ok(new { Message = "Order item status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
