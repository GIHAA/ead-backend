
using TechFixBackend._Models;
using TechFixBackend.Dtos;


namespace TechFixBackend.Services
{
    public interface IVendorService
    {
        
        Task<(List<Vendor> vendors, long totalVendors)> GetAllVendorsAsync(int pageNumber, int pageSize);
        Task<Vendor> GetVendorByIdAsync(string vendorId);
        Task<Vendor> CreateVendorAsync(VendorCreateDto vendorDto, string userId);
        Task<bool> UpdateVendorAsync(string vendorId, VendorUpdateDto vendorDto);
        Task<bool> DeleteVendorAsync(string vendorId);
        Task<bool> AssignVendorToUser(string vendorId, string userId);
        Task<List<Vendor>> GetVendorsByUserAsync(string userId);
    }
}
