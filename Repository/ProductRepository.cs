using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDBContext context)
        {
            _products = context.Products;
        }

       public async Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize, string search = "")
        {
            // Apply search filter
            var filter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                             p.ProductDescription.ToLower().Contains(search.ToLower()));

            return await _products.Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> GetTotalProductsAsync(string search = "")
        {
            // Apply search filter
            var filter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                             p.ProductDescription.ToLower().Contains(search.ToLower()));

            return await _products.CountDocumentsAsync(filter);
        }


        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<long> GetTotalProductsAsync()
        {
            return await _products.CountDocumentsAsync(u => true);
        }

        public async Task<bool> UpdateProductAsync(string productId, Product updatedProduct)
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == productId, updatedProduct);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == productId);
            return result.DeletedCount > 0;
        }
        Task<List<Product>> IProductRepository.GetProductsByCategoryAsync(string categoryId)
        {
            var filter = Builders<Product>.Filter.Where(p => p.CategoryId == categoryId);
            return _products.Find(filter).ToListAsync();
        }
    }
}
