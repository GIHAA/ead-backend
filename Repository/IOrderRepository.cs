using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<(List<Order> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByVendorIdAsync(string vendorId);
    }
}
