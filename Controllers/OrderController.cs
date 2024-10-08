/*
 * File: OrderController.cs
 * Project: Healthy Bites.Controllers
 * Description: Controller responsible for handling all order-related operations such as creating orders, 
 *              updating order status, requesting cancellations, and retrieving orders for customers and vendors.
 *              It handles operations with proper JWT token authentication and calls the IOrderService for the core logic.
 * Authors: Kandambige S.T. it21181856 | Perera W.H.T.H. it21165498
 */


using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HealthyBites.Dtos;
using HealthyBites.Services;
using System.IdentityModel.Tokens.Jwt;

namespace HealthyBites.Controllers
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

        //customer create order
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                // Get the JWT token from the Authorization header
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                Console.WriteLine(jwtToken);

                // Extract the user ID from the token
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in the token." });
                }

                var userId = userIdClaim.Value;

                // Pass the user ID to the service method
                await _orderService.CreateOrderAsync(createOrderDto, userId);

                return Ok(new { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        //admin get all orders
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

        //customer send cancel request
        [HttpGet("cancel-requsted")]
        public async Task<IActionResult> GetAllCancelReqOrders(int pageNumber = 1, int pageSize = 10, string customerId = null)
        {
            try
            {
                var (orders, totalOrders) = await _orderService.GetAllCancelReqOrdersAsync(pageNumber, pageSize, customerId);
                if (orders == null || !orders.Any()) return Ok(new { Message = "No cancelled orders found." });

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

        [HttpPut("request-cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId, [FromBody] RequestCancelOrderDto cancelOrderDto)
        {
            try
            {
                await _orderService.CancelRequestOrderAsync(orderId, cancelOrderDto);
                return Ok(new { Message = "Order cancelletion requested successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //update cancllation response 
        [HttpPut("cancel-response/{orderId}")]
        public async Task<IActionResult> CancelResponse(string orderId, [FromBody] CancellationResponseDto cancellationResponseDto)
        {
            try
            {
                await _orderService.UpdateOrderCancelAsync(orderId, cancellationResponseDto);
                return Ok(new { Message = "Order cancellation status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //update entire order status
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

        //get all orders for currently logged in vendor
        [HttpGet("vendor-orders")]
        public async Task<IActionResult> GetOrdersByVendor()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                // Console.WriteLine(token);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var vendorIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (vendorIdClaim == null)
                {
                    return Unauthorized(new { Message = "Vendor ID not found in the token." });
                }

                var vendorId = vendorIdClaim.Value;

                var orders = await _orderService.GetOrdersByVendorIdAsync(vendorId);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "No orders found for the vendor." });
                }

                return Ok(orders);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("customer-orders")]
        public async Task<IActionResult> GetOrdersByCustomer(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                // Console.WriteLine(token);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var customerIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (customerIdClaim == null)
                {
                    return Unauthorized(new { Message = "Customer ID not found in the token." });
                }

                var customerId = customerIdClaim.Value;

                var (orders, totalOrders) = await _orderService.GetOrdersByCustomerIdAsync(customerId, pageNumber, pageSize);
                if (orders == null || !orders.Any()) return Ok(new { Message = "No orders found." });

                return Ok(new { totalOrders, orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }




        [HttpPut("{orderId}/item/{productId}/status")]
        public async Task<IActionResult> UpdateOrderItemStatus(string orderId, string productId, [FromBody] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    return BadRequest(new { message = "Status cannot be empty." });
                }

                await _orderService.UpdateOrderItemStatusAsync(orderId, productId, status);

                return Ok(new { message = "Order item status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
