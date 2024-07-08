using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReviewController(ISiteReview reviewInterface) : ControllerBase
    {
        [HttpPost("review")]
        public async Task<IActionResult> AddSiteReviewAsync(ReviewDTO review)
        {
            if (review == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await reviewInterface.AddSiteReviewAsync(review);
            return Ok(result);
        }

        [HttpPost("spcfs/review")]
        public async Task<IActionResult> GetSiteReviewAsync(ViewReviewDTO review)
        {
            if (review == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await reviewInterface.GetSiteReviewAsync(review);
            return Ok(result);
        }

        [HttpPut("update/review")]
        public async Task<IActionResult> UpdateSiteReviewAsync(UpdateReviewDTO review)
        {
            if (review == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await reviewInterface.UpdateSiteReviewAsync(review);
            return Ok(result);
        }

        [HttpGet("random/site")]
        public async Task<IActionResult> GetRandomSiteReviewAsync()
        {
            
            var result = await reviewInterface.GetRandomSiteReviewAsync();
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var randomReview = JsonConvert.DeserializeObject<List<Review>>(result.Message);

            return Ok(randomReview);
        }

        [HttpDelete("delete-review")]
        public async Task<IActionResult> DeleteSiteReviewAsync(DeleteReviewDTO deleteReview)
        {
            var result = await reviewInterface.DeleteSiteReviewAsync(deleteReview);
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
