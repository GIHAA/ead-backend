/*
 * File: IProductCatService.cs
 * Project: Healthy Bites
 * Description: This file defines the IProductCatService interface for the Healthy Bites system. It outlines the service methods for managing 
 *              product categories, including methods for retrieval, creation, updating, and deletion of categories.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Interfaces:
 * - IProductCatService: Interface that defines the service methods for managing product categories in the Healthy Bites system.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;
using HealthyBites.Dtos;

namespace HealthyBites.Services
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
