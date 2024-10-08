/*
 * File: ProductService.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductService class, which implements the IProductService interface for managing products in the Healthy Bites system.
 *              It provides methods for retrieving, creating, updating, and deleting products, along with handling vendor and category associations, 
 *              pagination, and search functionality.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductService: Implements business logic for managing products, including interactions with vendors and categories in the Healthy Bites system.
 */

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;
using TechFixBackend.Repository;


namespace TechFixBackend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductCatRepository _producuCatsRepository;
        private readonly NotificationService _notificationService;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository , IProductCatRepository producuCatsRepository, NotificationService notificationService)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _producuCatsRepository = producuCatsRepository;
            _notificationService = notificationService;
        }

        // Retrieves all products with vendor details populated
        public async Task<(List<ProductWithVendorDto> products, long totalProducts)> GetAllProductsAsync(int pageNumber, int pageSize, string userId, string search = "")
        {
            // Ensure valid page number and size
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;


            // Get user details
            User user = await _userRepository.GetUserByIdAsync(userId);
            bool isAdmin = user.Role == "admin";


            List<Product> products;
            long totalProducts;

            // Call the appropriate repository methods depending on user role
            if (isAdmin)
            {
                products = await _productRepository.GetProductsAdminAsync(pageNumber, pageSize, search);
                totalProducts = await _productRepository.GetTotalProductsAdminAsync(search);
            }
            else
            {
                products = await _productRepository.GetProductsAsync(pageNumber, pageSize, userId, search);
                totalProducts = await _productRepository.GetTotalProductsAsync(search, userId);
            }

            // Map products to include vendor and category information
            var productsWithVendors = new List<ProductWithVendorDto>();
            foreach (var product in products)
            {
                var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
                var category = await _producuCatsRepository.GetProductCatByIdAsync(product.CategoryId);

                var productWithVendor = new ProductWithVendorDto
                {
                    Id = product.Id,
                    Vendor = vendor, // Populate vendor details
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Category = category, // Populate category details
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ProductStatus = product.ProductStatus,
                    ProductImageUrl = product.ProductImageUrl
                };

                productsWithVendors.Add(productWithVendor);
            }

            return (productsWithVendors, totalProducts);
        }

        // Retrieves a specific product by its ID with vendor details populated
        public async Task<ProductWithVendorDto> GetProductByIdAsync(string productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return null;

            // Fetch the vendor based on VendorId  using existing method
            var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
            var category = await _producuCatsRepository.GetProductCatByIdAsync(product.CategoryId);

            // Map product to include vendor information
            return new ProductWithVendorDto
            {
                Id = product.Id,
                Vendor = vendor, // Populate vendor details
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Category = category, // Populate category details
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ProductStatus = product.ProductStatus,
                ProductImageUrl = product.ProductImageUrl
            };
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto productDto)
        {
            var vendor = await _userRepository.GetUserByIdAsync(productDto.VendorId);
            if (vendor == null)
            {
                throw new Exception("Vendor not found");
            }

            var category = await _producuCatsRepository.GetProductCatByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            var product = new Product
            {
                VendorId = productDto.VendorId,
                ProductName = productDto.ProductName,
                ProductDescription = productDto.ProductDescription,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                ProductImageUrl = productDto.ProductImageUrl
            };

            await _productRepository.CreateProductAsync(product);

            await _notificationService.SendNotificationWithDetailsAsync(vendor.Id,$"Product '{product.ProductName}' has been created.",product.Id,null);

            return product;
        }
        public async Task<bool> UpdateProductAsync(string productId, ProductUpdateDto productDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null) return false;

            // Check for VendorId only if it's provided in the update DTO
            if (!string.IsNullOrEmpty(productDto.VendorId))
            {
                var vendor = await _userRepository.GetUserByIdAsync(productDto.VendorId);
                if (vendor == null)
                {
                    throw new Exception("Vendor not found");
                }
                existingProduct.VendorId = productDto.VendorId;
            }

            if (!string.IsNullOrEmpty(productDto.CategoryId))
            {
                var category = await _producuCatsRepository.GetProductCatByIdAsync(productDto.CategoryId);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                existingProduct.CategoryId = productDto.CategoryId;
            }

            if (!string.IsNullOrEmpty(productDto.ProductName))
                existingProduct.ProductName = productDto.ProductName;

            if (!string.IsNullOrEmpty(productDto.ProductDescription))
                existingProduct.ProductDescription = productDto.ProductDescription;

            if (productDto.Price > 0)
                existingProduct.Price = productDto.Price;

            if (productDto.StockQuantity >= 0)
                existingProduct.StockQuantity = productDto.StockQuantity;

            if (Enum.IsDefined(typeof(ProductStatus), productDto.ProductStatus))
            {
                existingProduct.ProductStatus = productDto.ProductStatus;
            }

            if (!string.IsNullOrEmpty(productDto.ProductImageUrl))
                existingProduct.ProductImageUrl = productDto.ProductImageUrl;

            var updateSuccess = await _productRepository.UpdateProductAsync(productId, existingProduct);

            if (updateSuccess)
            {
                await _notificationService.SendNotificationWithDetailsAsync(
                    productDto.VendorId,
                    $"Product '{existingProduct.ProductName}' has been updated.",
                    existingProduct.Id,
                    null 
                );
            }

            return updateSuccess;
           // return await _productRepository.UpdateProductAsync(productId, existingProduct);
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var deleteSuccess = await _productRepository.DeleteProductAsync(productId);

            if (deleteSuccess)
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(productId);

                var vendor = await _userRepository.GetUserByIdAsync(existingProduct.VendorId);
                await _notificationService.SendNotificationWithDetailsAsync(
                    vendor.Id,
                    $"Product '{existingProduct.ProductName}' has been deleted.",
                    null, 
                    null
                );
            }

            return deleteSuccess;
            //return await _productRepository.DeleteProductAsync(productId);
        }


        public async Task<List<ProductWithVendorDto>> GetProductsByCategoryAsync(string categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            var productsWithVendors = new List<ProductWithVendorDto>();

            foreach (var product in products)
            {
                var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
                var category = await _producuCatsRepository.GetProductCatByIdAsync(product.CategoryId);

                var productWithVendor = new ProductWithVendorDto
                {
                    Id = product.Id,
                    Vendor = vendor,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Category = category,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ProductStatus = product.ProductStatus,
                    ProductImageUrl = product.ProductImageUrl
                };

                productsWithVendors.Add(productWithVendor);
            }

            return productsWithVendors;
        }
    }
}
