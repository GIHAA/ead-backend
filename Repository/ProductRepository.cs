/*
 * File: ProductRepository.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductRepository class, implementing the IProductRepository interface for interacting with the products 
 *              stored in MongoDB. It provides functionality for retrieving, creating, updating, and deleting products, as well as handling 
 *              product searches, pagination, category filtering, and stock management.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductRepository: Implements methods to manage CRUD operations on products in the Healthy Bites system using MongoDB.
 */

using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDBContext context)
        {
            _products = context.Products;
        }

        // Retrieves a paginated list of products for admin use 
        public async Task<List<Product>> GetProductsAdminAsync(int pageNumber, int pageSize, string search = "")
        {
            // Apply search filter and status filter (only Active or Promoted)
            var searchFilter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                                   p.ProductDescription.ToLower().Contains(search.ToLower()));

            var statusFilter = Builders<Product>.Filter.In(p => p.ProductStatus, new[] { ProductStatus.Active, ProductStatus.Promoted });

            var combinedFilter = Builders<Product>.Filter.And(searchFilter, statusFilter);

            return await _products.Find(combinedFilter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        // Retrieves a paginated list of products for a specific vendor
        public async Task<List<Product>> GetProductsAsync(int pageNumber, int pageSize, string userId, string search = "")
        {
            // Apply search filter and status filter (only Active or Promoted)
            var searchFilter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                                   p.ProductDescription.ToLower().Contains(search.ToLower()));


            var userFilter = Builders<Product>.Filter.Where(p => p.VendorId == userId);

            var combinedFilter = Builders<Product>.Filter.And(searchFilter, userFilter);

            return await _products.Find(combinedFilter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }


        // Retrieves the total count of products for admin use
        public async Task<long> GetTotalProductsAdminAsync(string search = "")
        {
            // Apply search filter
            var filter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                             p.ProductDescription.ToLower().Contains(search.ToLower()));

            return await _products.CountDocumentsAsync(filter);
        }

        // Retrieves the total count of products for a specific vendor
        public async Task<long> GetTotalProductsAsync(string search = "", string userId = "")
        {
            // Apply search filter
            var searchFilter = Builders<Product>.Filter.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                                                   p.ProductDescription.ToLower().Contains(search.ToLower()));

            var userFilter = Builders<Product>.Filter.Where(p => p.VendorId == userId);

            var combinedFilter = Builders<Product>.Filter.And(searchFilter, userFilter);

            return await _products.CountDocumentsAsync(combinedFilter);
        }

        // Retrieves a specific product by its ID
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        // Creates a new product
        public async Task CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        // Retrieves a paginated list of products by category
        public async Task<long> GetTotalProductsAsync()
        {
            return await _products.CountDocumentsAsync(u => true);
        }

        // Updates an existing product
        public async Task<bool> UpdateProductAsync(string productId, Product updatedProduct)
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == productId, updatedProduct);
            return result.ModifiedCount > 0;
        }

        // Deletes an existing product
        public async Task<bool> DeleteProductAsync(string productId)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == productId);
            return result.DeletedCount > 0;
        }

        // Retrieves a paginated list of products by category
        Task<List<Product>> IProductRepository.GetProductsByCategoryAsync(string categoryId)
        {
            var filter = Builders<Product>.Filter.Where(p => p.CategoryId == categoryId);
            return _products.Find(filter).ToListAsync();
        }

        // Decreases the stock quantity of a product
        public async Task<bool> DecreaseProductQuantityAsync(string productId, int quantity)
        {
            var filter = Builders<Product>.Filter.Where(p => p.Id == productId && p.StockQuantity >= quantity);
            var update = Builders<Product>.Update.Inc(p => p.StockQuantity, -quantity);
            var result = await _products.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

    }
}
