using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;

namespace TechFixBackend.Services
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
