using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IVendorRepository
    {
        // Get all vendors with pagination
        Task<(List<Vendor> vendors, long totalVendors)> GetVendorsAsync(int pageNumber, int pageSize);

        // Get vendor by ObjectId
        Task<Vendor> GetVendorByIdAsync(string vendorId);

        // Create a new vendor
        Task CreateVendorAsync(Vendor vendor);

        // Update an existing vendor
        Task<bool> UpdateVendorAsync(string vendorId, Vendor updatedVendor);

        // Delete a vendor by ObjectId
        Task<bool> DeleteVendorAsync(string vendorId);
        Task<List<Vendor>> GetVendorsByUserIdAsync(string userId);

       
    }
}
