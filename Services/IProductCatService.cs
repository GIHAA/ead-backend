/*
 * File: IProductCatService.cs
 * Project: TechFixBackend.Services
 * Description: Interface defining the service layer for managing product categories. 
 *              Provides method signatures for retrieving, creating, updating, and deleting product categories, 
 *              as well as handling pagination and retrieving detailed category information.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;

namespace TechFixBackend.Services
{
    public interface IProductCatService
    {
        Task<(List<ProductCatDto> productCats, long totalProductCats)> GetAllProductCatsAsync(int pageNumber, int pageSize);
        Task<List<ProductCatDto>> GetAllProductCatsAsync();
        Task<ProductCatDto> GetProductCatByIdAsync(string productCatId);
        Task<ProductCat> CreateProductCatAsync(ProductCatCreateDto productCatDto);
        Task<bool> UpdateProductCatAsync(string productCatId, ProductCatUpdateDto productCatDto);
        Task<bool> DeleteProductCatAsync(string productCatId);
    }
}
