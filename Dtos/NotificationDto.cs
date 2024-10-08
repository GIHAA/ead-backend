using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class NotificationDto
    {
        public Notification Notification { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
    }
}

