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
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest(new { Message = "Page number must be greater than 0." });
                }

                if (pageSize < 1)
                {
                    return BadRequest(new { Message = "Page size must be greater than 0." });
                }

                // Get products and total count with search
                var (pagedProducts, totalProducts) = await _productService.GetAllProductsAsync(pageNumber, pageSize, search);

                // Check if no products are found
                if (pagedProducts == null || !pagedProducts.Any())
                {
                    return NotFound(new { Message = "No products found." });
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                // Ensure pageNumber does not exceed totalPages
                if (pageNumber > totalPages && totalPages > 0)
                {
                    return BadRequest(new { Message = "Page number exceeds total pages." });
                }

                var response = new
                {
                    TotalRecords = totalProducts,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Products = pagedProducts
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving products.", Details = ex.Message });
            }
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
