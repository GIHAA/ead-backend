using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService feedbackservice;

        public FeedbackController(IFeedbackService productService)
        {
            feedbackservice = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedbacks(int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await feedbackservice.GetAllFeedbacksAsync(pageNumber, pageSize);
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(string id)
        {
            var product = await feedbackservice.GetFeedbackByIdAsync(id);
            if (product == null) return NotFound(new { Message = "Feedback not found" });

            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDto productDto)
        {
            try
            {
                var product = await feedbackservice.CreateFeedbackAsync(productDto);
                return Ok(new { Message = "Feedback created successfully", Feedback = product });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(string id, [FromBody] FeedbackUpdateDto productDto)
        {
            var updated = await feedbackservice.UpdateFeedbackAsync(id, productDto);
            if (updated) return Ok(new { Message = "Feedback updated successfully" });

            return BadRequest(new { Message = "Update failed" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(string id)
        {
            var deleted = await feedbackservice.DeleteFeedbackAsync(id);
            if (deleted) return Ok(new { Message = "Feedback deleted successfully" });

            return NotFound(new { Message = "Feedback not found" });
        }
    }
}
