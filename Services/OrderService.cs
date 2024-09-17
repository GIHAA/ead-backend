using MongoDB.Driver;
using System.Collections.Generic;

public class OrderService
{
    private readonly IMongoCollection<Order> _orders;

    public OrderService(MongoDBContext context)
    {
        _orders = context.Orders;
    }

    // Create a new order
    public Order CreateOrder(Order order)
    {
        _orders.InsertOne(order);
        return order;
    }

    // Get all orders
    public List<Order> GetOrders() => _orders.Find(order => true).ToList();

    // Get a single order by ID
    public Order GetOrderById(string orderId) =>
        _orders.Find(order => order.Id == orderId).FirstOrDefault();

    // Update an order
    public void UpdateOrder(string orderId, Order updatedOrder) =>
        _orders.ReplaceOne(order => order.Id == orderId, updatedOrder);

    // Delete an order
    public void DeleteOrder(string orderId) =>
        _orders.DeleteOne(order => order.Id == orderId);
}
