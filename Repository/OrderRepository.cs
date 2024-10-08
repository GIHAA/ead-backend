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

        //create order
        public async Task CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        //get all orders
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

        //get all cancellation requests
        public async Task<(List<Order> orders, long totalOrders)> GetAllCancelReqOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Cancellation.Requested, true); // Filter orders where cancellation is requested

            if (!string.IsNullOrEmpty(customerId))
            {
                var customerFilter = Builders<Order>.Filter.Eq(o => o.CustomerId, customerId);
                filter = Builders<Order>.Filter.And(filter, customerFilter); // Add customer filter if provided
            }

            var totalOrders = await _orders.CountDocumentsAsync(filter);
            var orders = await _orders.Find(filter)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();

            return (orders, totalOrders);
        }

        //get order details by id
        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        //update order
        public async Task UpdateOrderAsync(Order order)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
        }

        //get vendor specid orders
        public async Task<List<Order>> GetOrdersByVendorIdAsync(string vendorId)
        {
            var filter = Builders<Order>.Filter.Eq("Items.VendorId", vendorId);
            var orders = await _orders.Find(filter).ToListAsync();
            return orders;
        }
    }
}
