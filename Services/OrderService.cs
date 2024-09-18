using MongoDB.Driver;

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

        order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

        _orders.InsertOne(order);
    }

    // Get all orders
    public (List<Order> orders, long totalOrders) GetAllOrders(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        long totalOrders = _orders.CountDocuments(o => true);

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
    public void UpdateOrder(Order existingOrder, OrderUpdateModel updateModel)
    {
        if (!string.IsNullOrEmpty(updateModel.DeliveryAddress))
        {
            existingOrder.DeliveryAddress = updateModel.DeliveryAddress;
        }
        if (updateModel.Items != null && updateModel.Items.Any())
        {
            foreach (var item in updateModel.Items)
            {
                var existingItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (existingItem != null)
                {
                    if (existingItem.Status == "Processing")
                    {
                        if (item.Quantity <= 0)
                        {
                            existingOrder.Items.Remove(existingItem);
                        }
                        else
                        {
                            existingItem.Quantity = item.Quantity;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot update item '{existingItem.ProductId}' as it is not in 'Processing' status.");
                    }
                }
                else
                {
                    existingOrder.Items.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }
        }
        existingOrder.TotalAmount = existingOrder.Items.Sum(i => i.TotalPrice);
        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }


    // Cancel order (before dispatch)
    public void CancelOrder(Order existingOrder)
    {
        existingOrder.Status = "Canceled";
        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }
    public void UpdateOrderStatus(Order existingOrder, OrderStatusUpdateModel statusUpdateModel)
    {
        existingOrder.Status = statusUpdateModel.Status;

        if (statusUpdateModel.Status == "Shipped")
        {
            existingOrder.DispatchedDate = DateTime.UtcNow;
        }

        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }



    //updating
    public void UpdateOrderStatus(string orderId, string newStatus)
    {
        var existingOrder = _orders.Find(o => o.Id == orderId).FirstOrDefault();

        if (existingOrder == null)
            throw new Exception("Order not found.");

        existingOrder.Status = newStatus;

        if (newStatus == "Shipped")
        {
            existingOrder.DispatchedDate = DateTime.UtcNow;
        }

        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }



    public void UpdateOrderItemStatus(string orderId, string productId, string newStatus)
    {
        var existingOrder = _orders.Find(o => o.Id == orderId).FirstOrDefault();

        if (existingOrder == null)
            throw new Exception("Order not found.");

        var orderItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == productId);

        if (orderItem == null)
            throw new Exception("Order item not found.");


        orderItem.Status = newStatus;

        _orders.ReplaceOne(o => o.Id == existingOrder.Id, existingOrder);
    }














}





