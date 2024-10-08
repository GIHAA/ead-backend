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
        Task<(List<ProductWithVendorDto> products, long totalProducts)> GetAllProductsAsync(int pageNumber, int pageSize, string userId, string search = "");
        Task<List<ProductWithVendorDto>> GetProductsByCategoryAsync(string categoryId);
        Task<ProductWithVendorDto> GetProductByIdAsync(string productId);
        Task<Product> CreateProductAsync(ProductCreateDto productDto);
        Task<bool> UpdateProductAsync(string productId, ProductUpdateDto productDto);
        Task<bool> DeleteProductAsync(string productId);
    }
}
