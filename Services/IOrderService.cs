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
        Task<(List<GetCancelledOrderDetailsDto> orders, long totalOrders)> GetAllCancelReqOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task CancelRequestOrderAsync(string orderId, RequestCancelOrderDto cancelOrderDto);
        Task UpdateOrderCancelAsync(string orderId, CancellationResponseDto cancellationResponseDto);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task UpdateOrderItemStatusAsync(string orderId, string productId, string status);
        Task<List<VendorOrderDto>> GetOrdersByVendorIdAsync(string vendorId);
    }
}