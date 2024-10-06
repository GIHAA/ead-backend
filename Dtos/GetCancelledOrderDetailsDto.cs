using TechFixBackend.Dtos;

public class GetCancelledOrderDetailsDto
{
    public string OrderId { get; set; }
    public User Customer { get; set; }
    public string DeliveryAddress { get; set; }
    public float TotalAmount { get; set; }
    public string Status { get; set; }
    public List<GetOrderItemDto> Items { get; set; }
    public DateTime OrderDate { get; set; }
    public string DeliveryStatus { get; set; }
    public DateTime? DispatchedDate { get; set; }
    public CancellationDetailsDto Cancellation { get; set; }
}

public class CancellationDetailsDto
{
    public bool Requested { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
