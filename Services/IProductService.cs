using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;

namespace TechFixBackend.Services
{
    public interface IProductService
    {
        Task<(List<ProductWithVendorDto> products, long totalProducts)> GetAllProductsAsync(int pageNumber, int pageSize , string search = "");
        Task<ProductWithVendorDto> GetProductByIdAsync(string productId);
        Task<Product> CreateProductAsync(ProductCreateDto productDto);
        Task<bool> UpdateProductAsync(string productId, ProductUpdateDto productDto);
        Task<bool> DeleteProductAsync(string productId);
    }
}
