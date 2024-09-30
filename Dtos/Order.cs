namespace TechFixBackend.Dtos
{
    public class CreateOrderDto
    {
        public string CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string DeliveryAddress { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Status { get; set; } = "Processing";
    }

    public class OrderUpdateDto
    {
        public string DeliveryAddress { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderStatusUpdateDto
    {
        public string Status { get; set; }
    }

    public class OrderItemStatusUpdateDto
    {
        public string Status { get; set; }
    }

    public class RequestCancelOrderDto
    {
        public string Reason { get; set; } // Optional cancellation reason provided by the customer

    }
}
