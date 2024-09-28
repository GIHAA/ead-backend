using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;
using TechFixBackend.Repository;


namespace TechFixBackend.Services
{
    public class ProductCatService : IProductCatService
    {
        private readonly IProductCatRepository _productCatRepository;

        public ProductCatService(IProductCatRepository productCatRepository, IUserRepository userRepository)
        {
            _productCatRepository = productCatRepository;
        }

        // Retrieves all productCats with vendor details populated
        public async Task<(List<ProductCatDto> productCats , long totalProductCats) > GetAllProductCatsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var productCats = await _productCatRepository.GetProductCatsAsync(pageNumber, pageSize);
        
            var totalProductCats = await _productCatRepository.GetTotalProductCatsAsync();

            var productCatDtos = productCats.Select(pc => new ProductCatDto
            {
                Id = pc.Id,
                Category = pc.CatName,
                CatDescription = pc.CatDescription,
                ImageUrl = pc.CatImageUrl
            }).ToList();

            return (productCatDtos, totalProductCats);
        }

        // Retrieves a specific productCat by its ID with vendor details populated
        public async Task<ProductCatDto> GetProductCatByIdAsync(string productCatId)
        {
            var productCat = await _productCatRepository.GetProductCatByIdAsync(productCatId);
            if (productCat == null) return null;
            Console.WriteLine(productCat);

            // Map productCat to include vendor information
            return new ProductCatDto
            {   
                Id = productCat.Id,
                Category = productCat.CatName,
                CatDescription = productCat.CatDescription,
                ImageUrl = productCat.CatImageUrl,
            };
        }

        public async Task<ProductCat> CreateProductCatAsync(ProductCatCreateDto productCatDto)
        {
            
            var productCat = new ProductCat
            {
                CatName = productCatDto.CatName,
                CatDescription = productCatDto.CatDescription,
                CatImageUrl = productCatDto.CatImageUrl,
            };

            await _productCatRepository.CreateProductCatAsync(productCat);
            return productCat;
        }

        public async Task<bool> UpdateProductCatAsync(string productCatId, ProductCatUpdateDto productCatDto)
        {
            var existingProductCat = await _productCatRepository.GetProductCatByIdAsync(productCatId);
            if (existingProductCat == null) return false;

            existingProductCat.CatName = productCatDto.CatName ?? existingProductCat.CatName;
            existingProductCat.CatDescription = productCatDto.CatDescription ?? existingProductCat.CatDescription;
            existingProductCat.CatImageUrl = productCatDto.CatImageUrl ?? existingProductCat.CatImageUrl;

            return await _productCatRepository.UpdateProductCatAsync(productCatId, existingProductCat);
        }

        public async Task<bool> DeleteProductCatAsync(string productCatId)
        {
            return await _productCatRepository.DeleteProductCatAsync(productCatId);
        }

        public async Task< List<ProductCatDto>> GetAllProductCatsAsync()
        {
            var productCat = await _productCatRepository.GetAllProductCatAsync();
            if (productCat == null) return null;

            var productCatDtos = productCat.Select(pc => new ProductCatDto
            {
                Id = pc.Id,
                Category = pc.CatName,
                CatDescription = pc.CatDescription,
                ImageUrl = pc.CatImageUrl
            }).ToList();

            return productCatDtos;
        }
    }
}
