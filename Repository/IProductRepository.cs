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
        Task<List<Product>> GetProductsAdminAsync(int pageNumber, int pageSize, string search = "");
        Task<bool> DecreaseProductQuantityAsync(string productId, int quantity);
        Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize, string userId, string search = "");
        Task<Product> GetProductByIdAsync(string productId);
        Task<List<Product>> GetProductsByCategoryAsync(string categoryId);
        Task<long> GetTotalProductsAdminAsync(string search = "");
        Task<long> GetTotalProductsAsync(string search = "", string userId = "");
        Task CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(string productId, Product updatedProduct);
        Task<bool> DeleteProductAsync(string productId);
    }
}
