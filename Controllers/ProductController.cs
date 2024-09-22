using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound(new { Message = "Product not found" });

            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            try
            {
                var product = await _productService.CreateProductAsync(productDto);
                return Ok(new { Message = "Product created successfully", Product = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductUpdateDto productDto)
        {
            var updated = await _productService.UpdateProductAsync(id, productDto);
            if (updated) return Ok(new { Message = "Product updated successfully" });

            return BadRequest(new { Message = "Update failed" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (deleted) return Ok(new { Message = "Product deleted successfully" });

            return NotFound(new { Message = "Product not found" });
        }
    }
}
