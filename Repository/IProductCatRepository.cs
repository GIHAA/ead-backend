/*
 * File: IProductCatRepository.cs
 * Project: Healthy Bites
 * Description: This file defines the interface IProductCatRepository for the Healthy Bites system. It outlines the methods for interacting 
 *              with product categories, including retrieval, creation, updating, and deletion of categories.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Interfaces:
 * - IProductCatRepository: Interface that defines the repository methods for managing product categories in the Healthy Bites system.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
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
