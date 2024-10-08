/*
 * File: IProductService.cs
 * Project: Healthy Bites
 * Description: This file defines the IProductService interface for the Healthy Bites system. It outlines the service methods for managing products, 
 *              including methods for product retrieval, creation, updating, and deletion, as well as handling product categories, vendors, 
 *              and search functionality.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Interfaces:
 * - IProductService: Interface that defines the service methods for managing products in the Healthy Bites system.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;
using HealthyBites.Dtos;

namespace HealthyBites.Services
{
    public interface IProductService
    {
        // Retrieves a paginated list of products for admin use
        Task<(List<ProductWithVendorDto> products, long totalProducts)> GetAllProductsAsync(int pageNumber, int pageSize, string userId, string search = "");
        // Retrieves a paginated list of products for a specific vendor
        Task<List<ProductWithVendorDto>> GetProductsByCategoryAsync(string categoryId);
        // Retrieves a specific product by its ID
        Task<ProductWithVendorDto> GetProductByIdAsync(string productId);
        // Creates a new product
        Task<Product> CreateProductAsync(ProductCreateDto productDto);
        // Updates an existing product
        Task<bool> UpdateProductAsync(string productId, ProductUpdateDto productDto);
        // Deletes an existing product
        Task<bool> DeleteProductAsync(string productId);
    }
}
