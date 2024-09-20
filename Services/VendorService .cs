using TechFixBackend._Models;
using TechFixBackend.Dtos;
using TechFixBackend.Repository;

namespace TechFixBackend.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;

        public VendorService(IVendorRepository vendorRepository, AuthService authService, NotificationService notificationService)
        {
            _vendorRepository = vendorRepository;
            _authService = authService;
            _notificationService = notificationService;
        }

        // Retrieves all vendors with pagination
        public async Task<(List<Vendor> vendors, long totalVendors)> GetAllVendorsAsync(int pageNumber, int pageSize)
        {
            var (vendors, totalVendors) = await _vendorRepository.GetVendorsAsync(pageNumber, pageSize);
            return (vendors, totalVendors);
        }

        // Retrieves a specific vendor by its ID
        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            return await _vendorRepository.GetVendorByIdAsync(vendorId);
        }

        // Creates a new vendor and associates it with the specified user
        public async Task<Vendor> CreateVendorAsync(VendorCreateDto vendorDto, string userId)
        {
            // Check if the user exists
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Create a new vendor object
            var vendor = new Vendor
            {
                VendorName = user.Name,
                Comments   = vendorDto.Comments,
                AverageRating = vendorDto.AverageRating,
                IsActive = vendorDto.IsActive,
                VendorId = userId 
            };

            // Insert the vendor into the database
            await _vendorRepository.CreateVendorAsync(vendor);

            //// Update user's vendor list if needed
            //user.VendorIds.Add(vendor.Id);
            //var updateModel = new UserUpdateModel
            //{
            //    Role = user.Role,
            //    VendorIds = user.VendorIds
            //};
            //await _authService.UpdateUserAsync(userId, updateModel);

            // Send a notification to the user about vendor creation
            await _notificationService.SendNotificationToUserAsync(userId, $"Vendor '{vendor.VendorName}' created successfully.");

            return vendor;
        }

        // Updates an existing vendor's details
        public async Task<bool> UpdateVendorAsync(string vendorId, VendorUpdateDto vendorDto)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor == null) return false;

            // Update vendor fields with new data
            vendor.VendorName = vendorDto.VendorName;
            vendor.IsActive = vendorDto.IsActive;
            vendor.AverageRating = vendorDto.AverageRating;
            vendor.Comments = vendorDto.Comments;

            var updated = await _vendorRepository.UpdateVendorAsync(vendorId, vendor);

            if (updated)
            {
                // Send a notification to the user about vendor update
                await _notificationService.SendNotificationToUserAsync(vendor.VendorId, $"Vendor '{vendor.VendorName}' updated successfully.");
            }

            return updated;
        }

        // Deletes a vendor by its ID
        public async Task<bool> DeleteVendorAsync(string vendorId)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor == null) return false;

            var deleted = await _vendorRepository.DeleteVendorAsync(vendorId);

            if (deleted)
            {
                // Send a notification to the user about vendor deletion
                await _notificationService.SendNotificationToUserAsync(vendor.VendorId, $"Vendor '{vendor.VendorName}' deleted successfully.");
            }

            return deleted;
        }

        // Retrieves vendors associated with a specific user
        public async Task<List<Vendor>> GetVendorsByUserAsync(string userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null) return new List<Vendor>();

            return await _vendorRepository.GetVendorsByUserIdAsync(userId);
        }
    }
}
