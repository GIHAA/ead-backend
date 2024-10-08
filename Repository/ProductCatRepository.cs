/*
 * File: ProductCatRepository.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCatRepository class, implementing the IProductCatRepository interface for interacting with the 
 *              product categories stored in MongoDB. It provides functionality for retrieving, creating, updating, and deleting product categories, 
 *              as well as paginated queries and total count calculations.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCatRepository: Implements methods to manage CRUD operations on product categories in the Healthy Bites system using MongoDB.
 */

using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public class ProductCatRepository : IProductCatRepository
    {
        private readonly IMongoCollection<ProductCat> _productCats;

        public ProductCatRepository(MongoDBContext context)
        {
            _productCats = context.ProductCats;
        }

        // Retrieves a paginated list of product categories
        public async Task<List<ProductCat>> GetProductCatsAsync(int pageNumber, int pageSize)
        {
            return await _productCats.Find(p => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        // Retrieves a specific product category by its ID
        public async Task<ProductCat> GetProductCatByIdAsync(string productCatId)
        {
            return await _productCats.Find(p => p.Id == productCatId).FirstOrDefaultAsync();
        }
        
        // Creates a new product category
        public async Task CreateProductCatAsync(ProductCat productCat)
        {
            await _productCats.InsertOneAsync(productCat);
        }

        // Retrieves the total count of product categories
        public async Task<long> GetTotalProductCatsAsync()
        {
            return await _productCats.CountDocumentsAsync(u => true);
        }

        // Updates an existing product category
        public async Task<bool> UpdateProductCatAsync(string productCatId, ProductCat updatedProductCat)
        {
            var result = await _productCats.ReplaceOneAsync(p => p.Id == productCatId, updatedProductCat);
            return result.ModifiedCount > 0;
        }

        // Deletes an existing product category
        public async Task<bool> DeleteProductCatAsync(string productCatId)
        {
            var result = await _productCats.DeleteOneAsync(p => p.Id == productCatId);
            return result.DeletedCount > 0;
        }

        // Retrieves all product categories
        public Task<List<ProductCat>> GetAllProductCatAsync()
        {
            return _productCats.Find(p => p.CatStatus == 0).ToListAsync();
        }
    }
}
