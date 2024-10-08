using HealthyBites._Models;

namespace HealthyBites.Dtos
{
    public class NotificationDto
    {
        public Notification Notification { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
    }
}

