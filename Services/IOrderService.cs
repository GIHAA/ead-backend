using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend._Models;

namespace TechFixBackend.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto createOrderDto , string customerId);
        Task<(List<GetOrderDetailsDto> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task<GetOrderDetailsDto> GetOrderByIdAsync(string orderId); 
        Task UpdateOrderAsync(string orderId, OrderUpdateDto updateDto);
        Task CancelRequestOrderAsync(string orderId, RequestCancelOrderDto cancelOrderDto);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task UpdateOrderItemStatusAsync(string orderId, string productId, string status);
         Task<List<VendorOrderDto>> GetOrdersByVendorIdAsync(string vendorId);
    }
}