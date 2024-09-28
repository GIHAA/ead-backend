using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCatController : ControllerBase
    {
        private readonly IProductCatService _productCatService;

        public ProductCatController(IProductCatService productCatService)
        {
            _productCatService = productCatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductCats(int pageNumber = 1, int pageSize = 10)
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

                // Get users and total count
                var (pagedProductCats, totalProductCats) = await _productCatService.GetAllProductCatsAsync(pageNumber, pageSize);

                // Check if no users are found
                if (pagedProductCats == null || !pagedProductCats.Any())
                {
                    return NotFound(new { Message = "No users found." });
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalProductCats / pageSize);

                // Ensure pageNumber does not exceed totalPages
                if (pageNumber > totalPages && totalPages > 0)
                {
                    return BadRequest(new { Message = "Page number exceeds total pages." });
                }

                var response = new
                {
                    TotalRecords = totalProductCats,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    ProductCats = pagedProductCats  
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Message = "An error occurred while retrieving users.", Details = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProductCats()
        {
            var productCats = await _productCatService.GetAllProductCatsAsync();
            if (productCats == null || !productCats.Any()) return NotFound(new { Message = "No productCats found" });

            return Ok(productCats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCatById(string id)
        {
            var productCat = await _productCatService.GetProductCatByIdAsync(id);
            if (productCat == null) return NotFound(new { Message = "ProductCat not found" });

            return Ok(productCat);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductCat([FromBody] ProductCatCreateDto productCatDto)
        {
            try
            {
                var productCat = await _productCatService.CreateProductCatAsync(productCatDto);
                return Ok(new { Message = "ProductCat created successfully", ProductCat = productCat });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCat(string id, [FromBody] ProductCatUpdateDto productCatDto)
        {
            var updated = await _productCatService.UpdateProductCatAsync(id, productCatDto);
            if (updated) return Ok(new { Message = "ProductCat updated successfully" });

            return BadRequest(new { Message = "Update failed" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCat(string id)
        {
            var deleted = await _productCatService.DeleteProductCatAsync(id);
            if (deleted) return Ok(new { Message = "ProductCat deleted successfully" });

            return NotFound(new { Message = "ProductCat not found" });
        }
    }
}