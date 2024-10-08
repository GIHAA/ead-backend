/*
 * File: IProductService.cs
 * Project: TechFixBackend.Services
 * Description: Interface defining the service layer for managing products. 
 *              Provides method signatures for retrieving products with vendor details, 
 *              creating, updating, deleting products, and handling operations based on category or search queries.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;

namespace TechFixBackend.Services
{
    public interface IProductService
    {
        Task<(List<ProductWithVendorDto> products, long totalProducts)> GetAllProductsAsync(int pageNumber, int pageSize ,string userId  , string search = "");
        Task<List<ProductWithVendorDto>> GetProductsByCategoryAsync(string categoryId);
        Task<ProductWithVendorDto> GetProductByIdAsync(string productId);
        Task<Product> CreateProductAsync(ProductCreateDto productDto);
        Task<bool> UpdateProductAsync(string productId, ProductUpdateDto productDto);
        Task<bool> DeleteProductAsync(string productId);
    }
}
