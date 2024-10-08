/*
 * File: OrderRepository.cs
 * Project: TechFixBackend
 * Description: This file contains the implementation of the OrderRepository class, responsible for interacting with the MongoDB database
 *              to perform CRUD operations related to orders. It handles retrieving, creating, updating orders, and fetching orders
 *              based on customer or vendor filters. The repository pattern ensures clean separation of concerns and allows
 *              easy modification or replacement of data sources in the future.
 * 
 * Authors: Kandambige S.T. it21181856 | Perera W.H.T.H. it21165498
 * 
 * Dependencies:
 * - IMongoCollection<Order>: MongoDB collection for Order documents.
 * - MongoDBContext: The database context for accessing MongoDB collections.
 * 
 * Methods:
 * - CreateOrderAsync(Order): Inserts a new order into the database.
 * - GetAllOrdersAsync(int, int, string): Retrieves a paginated list of all orders with optional customer filtering.
 * - GetAllCancelReqOrdersAsync(int, int, string): Retrieves a paginated list of all orders with cancellation requests, with optional customer filtering.
 * - GetOrderByIdAsync(string): Retrieves an order by its unique ID.
 * - UpdateOrderAsync(Order): Updates an existing order document in the database.
 * - GetOrdersByVendorIdAsync(string): Retrieves all orders related to a specific vendor by filtering items within orders.
 * 
 * Notes:
 * - Pagination is applied for both order retrieval and cancellation requests.
 * - Filters are used to conditionally apply customer or vendor-specific retrieval logic.
 * - The repository pattern helps with the separation of data access logic from business logic.
 * 
 */

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
