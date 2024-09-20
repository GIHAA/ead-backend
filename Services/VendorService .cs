using TechFixBackend._Models;
using TechFixBackend.Dtos;
using TechFixBackend.Repository;

namespace TechFixBackend.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly AuthService _authService;

        public VendorService(IVendorRepository vendorRepository, AuthService authService)
        {
            _vendorRepository = vendorRepository;
            _authService = authService;
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
                VendorName = vendorDto.VendorName,
                IsActive = vendorDto.IsActive,
                VendorId = userId
            };

            // Insert the vendor into the database
            await _vendorRepository.CreateVendorAsync(vendor);

            // Update user's vendor list if needed
            //user.VendorIds.Add(vendor.Id);
            //await _authService.UpdateUserAsync(userId, user);

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

            return await _vendorRepository.UpdateVendorAsync(vendorId, vendor);
        }

        // Deletes a vendor by its ID
        public async Task<bool> DeleteVendorAsync(string vendorId)
        {
            return await _vendorRepository.DeleteVendorAsync(vendorId);
        }

        // Assigns a vendor to a specific user
        //public async Task<bool> AssignVendorToUser(string vendorId, string userId)
        //{
        //    var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
        //    var user = await _authService.GetUserByIdAsync(userId);

        //    if (vendor == null || user == null) return false;

        //    // Update the vendor's UserId
        //    vendor.UserId = userId;
        //    var updated = await _vendorRepository.UpdateVendorAsync(vendorId, vendor);
        //    if (!updated) return false;

        //    // Update the user's vendor list
        //    user.VendorIds.Add(vendorId);
        //    var userUpdated = await _authService.UpdateUserAsync(userId, user);

        //    return userUpdated;
        //}

        // Retrieves vendors associated with a specific user
        public async Task<List<Vendor>> GetVendorsByUserAsync(string userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null) return new List<Vendor>();

            return await _vendorRepository.GetVendorsByUserIdAsync(userId);
        }

      
    }
}
