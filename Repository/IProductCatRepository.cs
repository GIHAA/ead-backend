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
        // Retrieves a paginated list of product categories
        Task<List<ProductCat>> GetProductCatsAsync(int pageNumber, int pageSize);
        // Retrieves a specific product category by its ID
        Task<List<ProductCat>> GetAllProductCatAsync();
        // Retrieves a specific product category by its ID
        Task<ProductCat> GetProductCatByIdAsync(string productCatId);
        // Retrieves the total count of product categories
        Task<long> GetTotalProductCatsAsync();
        // Creates a new product category
        Task CreateProductCatAsync(ProductCat productCat);
        // Updates an existing product category
        Task<bool> UpdateProductCatAsync(string productCatId, ProductCat updatedProductCat);
        // Deletes an existing product category
        Task<bool> DeleteProductCatAsync(string productCatId);
    }
}
