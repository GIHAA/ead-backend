using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IProductCatRepository
    {
        Task<List<ProductCat>> GetProductCatsAsync(int pageNumber, int pageSize);
        Task<List<ProductCat>> GetAllProductCatAsync();
        Task<ProductCat> GetProductCatByIdAsync(string productCatId);
        Task<long> GetTotalProductCatsAsync();
        Task CreateProductCatAsync(ProductCat productCat);
        Task<bool> UpdateProductCatAsync(string productCatId, ProductCat updatedProductCat);
        Task<bool> DeleteProductCatAsync(string productCatId);
    }
}
