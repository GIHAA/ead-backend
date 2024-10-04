using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoDBContext context)
        {
            _orders = context.Orders;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task<(List<Order> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            var filter = string.IsNullOrEmpty(customerId)
                ? Builders<Order>.Filter.Empty
                : Builders<Order>.Filter.Eq(o => o.CustomerId, customerId);

            var totalOrders = await _orders.CountDocumentsAsync(filter);
            var orders = await _orders.Find(filter)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Limit(pageSize)
                                      .ToListAsync();

            return (orders, totalOrders);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

public async Task<List<Order>> GetOrdersByVendorIdAsync(string vendorId)
{
    var filter = Builders<Order>.Filter.Eq("Items.VendorId", vendorId);

    var orders = await _orders.Find(filter).ToListAsync();

    // Log the number of orders found
    Console.WriteLine($"Orders found for VendorId {vendorId}: {orders.Count}");

    return orders;
}
    }
}
