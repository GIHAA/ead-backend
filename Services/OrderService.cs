using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class OrderService
{
    private readonly IMongoCollection<Order> _orders;

    public OrderService(MongoDBContext context)
    {
        _orders = context.Orders;
    }

    // Create a new order
    public void CreateOrder(OrderModel orderModel)
    {
        var order = new Order
        {
            CustomerId = orderModel.CustomerId,
            Items = orderModel.Items.ConvertAll(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }),
            DeliveryAddress = orderModel.DeliveryAddress
        };

        // Calculate total amount for the order
        order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

        _orders.InsertOne(order);
    }

    // Get all orders
    public (List<Order> orders, long totalOrders) GetAllOrders(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        // Get total count of orders
        long totalOrders = _orders.CountDocuments(o => true);

        // Fetch paginated orders
        var pagedOrders = _orders
            .Find(o => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToList();

        return (pagedOrders, totalOrders);
    }


    // Get order by ID
    public Order GetOrderById(string orderId)
    {
        return _orders.Find(o => o.Id == orderId).FirstOrDefault();
    }

    // Update order details (before dispatch)
    public void UpdateOrder(Order existingOrder, OrderUpdateModel updateModel)
    {
        if (!string.IsNullOrEmpty(updateModel.DeliveryAddress))
        {
            existingOrder.DeliveryAddress = updateModel.DeliveryAddress;
        }

        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }

    // Cancel order (before dispatch)
    public void CancelOrder(Order existingOrder)
    {
        existingOrder.Status = "Canceled";
        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }

    // Update order status (e.g., Shipped, Delivered)
    public void UpdateOrderStatus(Order existingOrder, OrderStatusUpdateModel statusUpdateModel)
    {
        existingOrder.Status = statusUpdateModel.Status;

        if (statusUpdateModel.Status == "Shipped")
        {
            existingOrder.DispatchedDate = DateTime.UtcNow;
        }

        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }
}
