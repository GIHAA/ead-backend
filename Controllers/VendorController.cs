
using Microsoft.AspNetCore.Mvc;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }



        // POST: api/vendor/create?userId=66e5759bc70122c959f124c5
        [HttpPost("create")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorCreateDto vendorDto, [FromQuery] string userId)
        {
            try
            {
                var vendor = await _vendorService.CreateVendorAsync(vendorDto, userId);
                return Ok(new { Message = "Vendor created successfully", Vendor = vendor });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        // Get all vendors with pagination
        [HttpGet]
        public async Task<IActionResult> GetVendors(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (vendors, totalVendors) = await _vendorService.GetAllVendorsAsync(pageNumber, pageSize);
                return Ok(new { Vendors = vendors, TotalCount = totalVendors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // Get a vendor by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendorById(string id)
        {
            try
            {
                var vendor = await _vendorService.GetVendorByIdAsync(id);
                if (vendor == null)
                    return NotFound(new { Message = "Vendor not found" });

                return Ok(vendor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Update a vendor by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVendor(string id, [FromBody] VendorUpdateDto vendorDto)
        {
            try
            {
                var updated = await _vendorService.UpdateVendorAsync(id, vendorDto);
                if (updated)
                    return Ok(new { Message = "Vendor updated successfully" });

                return BadRequest(new { Message = "Update failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // Delete a vendor by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(string id)
        {
            try
            {
                var deleted = await _vendorService.DeleteVendorAsync(id);
                if (deleted)
                    return Ok(new { Message = "Vendor deleted successfully" });

                return NotFound(new { Message = "Vendor not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
