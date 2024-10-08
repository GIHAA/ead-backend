namespace HealthyBites.Dtos
{
    public class VendorUpdateDto
    {
        public string VendorName { get; set; }
        public bool IsActive { get; set; }
        public float AverageRating { get; set; } 
        public string Comments { get; set; }
    }
}
