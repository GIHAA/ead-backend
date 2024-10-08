/*
 * File: IProductCatRepository.cs
 * Project: Healthy Bites.Repository
 * Description: Interface for the ProductCatRepository, defining the contract for data access operations related to 
 *              product categories (ProductCat). It includes methods for retrieving, creating, updating, and deleting 
 *              product categories, as well as methods for pagination and counting the total number of categories.
 */


using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IProductCatRepository
    {
        Task<List<ProductCat>> GetProductCatsAsync(int pageNumber, int pageSize);
        Task<List<ProductCat>> GetAllProductCatAsync();
        Task<ProductCat> GetProductCatByIdAsync(string productCatId);
        Task<long> GetTotalProductCatsAsync();
        Task CreateProductCatAsync(ProductCat productCat);
        Task<bool> UpdateProductCatAsync(string productCatId, ProductCat updatedProductCat);
        Task<bool> DeleteProductCatAsync(string productCatId);
    }
}
