using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;

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
    }
}
