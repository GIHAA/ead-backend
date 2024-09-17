public class CreateOrderModel
{
    public string CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; }
    public float TotalAmount { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryStatus { get; set; }
}