using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(string productId);
        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(string productId, Product updatedProduct);
        Task<bool> DeleteProductAsync(string productId);
    }
}
