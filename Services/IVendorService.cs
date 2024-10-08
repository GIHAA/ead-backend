
using HealthyBites._Models;
using HealthyBites.Dtos;


namespace HealthyBites.Services
{
    public interface IVendorService
    {
        
        Task<(List<User> vendors, long totalVendors)> GetAllVendorsAsync(int pageNumber, int pageSize);
        Task<User> GetVendorByIdAsync(string vendorId);
        Task<User> CreateVendorAsync(VendorCreateDto vendorDto, string userId);
        Task<bool> UpdateVendorAsync(string vendorId, VendorUpdateDto vendorDto);
        Task<bool> DeleteVendorAsync(string vendorId);
       // Task<bool> AssignVendorToUser(string vendorId, string userId);
        Task<List<User>> GetVendorsByUserAsync(string userId);
    }
}
