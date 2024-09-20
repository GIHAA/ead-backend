using MongoDB.Driver;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public class VendorRepository(MongoDBContext context) : IVendorRepository
    {
        private readonly IMongoCollection<Vendor> _vendors = context.Vendors;

        public async Task<(List<Vendor> vendors, long totalVendors)> GetVendorsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            long totalVendors = await _vendors.CountDocumentsAsync(v => true);
            var vendors = await _vendors
                .Find(v => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (vendors, totalVendors);
        }

        // Get vendor by ObjectId as string
        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            return await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
        }

        // Create a new vendor
        public async Task CreateVendorAsync(Vendor vendor)
        {
            await _vendors.InsertOneAsync(vendor);
        }

        // Update an existing vendor using ObjectId
        public async Task<bool> UpdateVendorAsync(string vendorId, Vendor updatedVendor)
        {
            var result = await _vendors.ReplaceOneAsync(v => v.Id == vendorId, updatedVendor);
            return result.ModifiedCount > 0;
        }

        // Delete a vendor by ObjectId
        public async Task<bool> DeleteVendorAsync(string vendorId)
        {
            var result = await _vendors.DeleteOneAsync(v => v.Id == vendorId);
            return result.DeletedCount > 0;
        }

        // Get vendors by UserId
        public async Task<List<Vendor>> GetVendorsByUserIdAsync(string userId)
        {
            return await _vendors.Find(v => v.UserId == userId).ToListAsync();
        }


    }
}
