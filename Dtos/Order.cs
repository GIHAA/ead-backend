public class CreateOrderModel
{
    public string CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public float TotalAmount { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryStatus { get; set; }
}

public class OrderModel
{
    public string CustomerId { get; set; }
    public List<OrderItemModel> Items { get; set; }
    public string DeliveryAddress { get; set; }
}

public class OrderItemModel
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }
    public string Status { get; set; } // Status of the product (e.g., "Processing", "Shipped", "Delivered")
}


public class OrderUpdateModel
{
    public string DeliveryAddress { get; set; }
    public List<OrderItemModel> Items { get; set; } // New or updated items list
}

public class OrderStatusUpdateModel
{
    public string Status { get; set; } // Shipped, Delivered, Canceled
}



public class OrderItemStatusUpdateModel
{
    public string Status { get; set; } // e.g., "Processing", "Shipped", "Delivered"
}


