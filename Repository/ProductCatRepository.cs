using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public class ProductCatRepository : IProductCatRepository
    {
        private readonly IMongoCollection<ProductCat> _productCats;

        public ProductCatRepository(MongoDBContext context)
        {
            _productCats = context.ProductCats;
        }

        public async Task<List<ProductCat>> GetProductCatsAsync(int pageNumber, int pageSize)
        {
            return await _productCats.Find(p => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<ProductCat> GetProductCatByIdAsync(string productCatId)
        {
            return await _productCats.Find(p => p.Id == productCatId).FirstOrDefaultAsync();
        }

        public async Task CreateProductCatAsync(ProductCat productCat)
        {
            await _productCats.InsertOneAsync(productCat);
        }

        public async Task<long> GetTotalProductCatsAsync()
        {
            return await _productCats.CountDocumentsAsync(u => true);
        }

        public async Task<bool> UpdateProductCatAsync(string productCatId, ProductCat updatedProductCat)
        {
            var result = await _productCats.ReplaceOneAsync(p => p.Id == productCatId, updatedProductCat);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductCatAsync(string productCatId)
        {
            var result = await _productCats.DeleteOneAsync(p => p.Id == productCatId);
            return result.DeletedCount > 0;
        }

        public Task<List<ProductCat>> GetAllProductCatAsync()
        {
            return _productCats.Find(p => true).ToListAsync();
        }
    }
}