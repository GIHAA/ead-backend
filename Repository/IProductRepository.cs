/*
 * File: IProductRepository.cs
 * Project: Healthy Bites
 * Description: This file defines the interface IProductRepository for the Healthy Bites system. It outlines the methods for interacting 
 *              with products, including retrieval, creation, updating, and deletion of products, as well as handling search functionality, 
 *              pagination, and product quantity management.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Interfaces:
 * - IProductRepository: Interface that defines the repository methods for managing products in the Healthy Bites system.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public interface IProductRepository
    {
        // Retrieves a paginated list of products for admin use
        Task<List<Product>> GetProductsAdminAsync(int pageNumber, int pageSize, string search = "");
        // Retrieves a paginated list of products for a specific vendor
        Task<bool> DecreaseProductQuantityAsync(string productId, int quantity);
        // Retrieves a paginated list of products for a specific vendor
        Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize, string userId, string search = "");
        // Retrieves a specific product by its ID
        Task<Product> GetProductByIdAsync(string productId);
        // Retrieves a list of products by category
        Task<List<Product>> GetProductsByCategoryAsync(string categoryId);
        // Retrieves the total count of products for admin use
        Task<long> GetTotalProductsAdminAsync(string search = "");
        // Retrieves the total count of products for a specific vendor
        Task<long> GetTotalProductsAsync(string search = "", string userId = "");
        // Creates a new product
        Task CreateProductAsync(Product product);
        // Updates an existing product
        Task<bool> UpdateProductAsync(string productId, Product updatedProduct);
        // Deletes an existing product
        Task<bool> DeleteProductAsync(string productId);
    }
}
