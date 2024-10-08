using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public interface IVendorRepository
    {
        // Get all vendors with pagination
        Task<(List<User> vendors, long totalVendors)> GetVendorsAsync(int pageNumber, int pageSize);

        // Get vendor by ObjectId
        Task<User> GetVendorByIdAsync(string vendorId);

        // Create a new vendor
        Task CreateVendorAsync(User vendor);

        // Update an existing vendor
        Task<bool> UpdateVendorAsync(string vendorId, User updatedVendor);

        // Delete a vendor by ObjectId
        Task<bool> DeleteVendorAsync(string vendorId);
        Task<List<User>> GetVendorsByUserIdAsync(string userId);

       
    }
}
