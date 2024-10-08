using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAdminAsync(int pageNumber, int pageSize , string search = "");
        Task<bool> DecreaseProductQuantityAsync(string productId, int quantity);
        Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize ,string userId  , string search = "");
        Task<Product> GetProductByIdAsync(string productId);
        Task<List<Product>> GetProductsByCategoryAsync(string categoryId);
        Task<long> GetTotalProductsAdminAsync(string search = "");
        Task<long> GetTotalProductsAsync(string search = "" , string userId = "");
        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(string productId, Product updatedProduct);
        Task<bool> DeleteProductAsync(string productId);
    }
}
