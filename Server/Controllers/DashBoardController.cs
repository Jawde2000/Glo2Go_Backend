using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ServerLibrary.Repositories.Implementations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using BaseLibrary.Responses;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DashBoardController(IDashboard dashboardInterface) : ControllerBase
    {
        [Route("api/activities/count-by-type")]
        [HttpGet]
        public async Task<IActionResult> GetActivitiesCountByType()
        {
            var result = await dashboardInterface.GetActivitiesCountByTypeAsync();
            return Ok(result);
        }

        [Route("api/reports/count-by-type")]
        [HttpGet]
        public async Task<IActionResult> GetReportsCountByType()
        {
            var result = await dashboardInterface.GetReportsCountByTypeAsync();
            return Ok(result);
        }

        [Route("api/reviews/count-by-rating")]
        [HttpGet]
        public async Task<IActionResult> GetReviewsCountByRating()
        {
            var result = await dashboardInterface.GetReviewsCountByRatingAsync();
            return Ok(result);
        }


        [Route("api/reviews/recent")]
        [HttpGet]
        public async Task<IActionResult> GetRecentReviews([FromQuery] int count)
        {
            var result = await dashboardInterface.GetRecentReviewsAsync(count);
            return Ok(result);
        }

        [Route("api/reports/recent")]
        [HttpGet]
        public async Task<IActionResult> GetRecentReports([FromQuery] int count)
        {
            var result = await dashboardInterface.GetRecentReportsAsync(count);
            return Ok(result);
        }

        [Route("api/activities/total-count")]
        [HttpGet]
        public async Task<IActionResult> GetTotalActivitiesCount()
        {
            var result = await dashboardInterface.GetTotalActivitiesCountAsync();
            return Ok(result);
        }

        [Route("api/reports/total-count")]
        [HttpGet]
        public async Task<IActionResult> GetTotalReportsCount()
        {
            var result = await dashboardInterface.GetTotalReportsCountAsync();
            return Ok(result);
        }

        [Route("api/reviews/total-count")]
        [HttpGet]
        public async Task<IActionResult> GetTotalReviewsCount()
        {
            var result = await dashboardInterface.GetTotalReviewsCountAsync();
            return Ok(result);
        }

        [Route("api/reviews/average-rating")]
        [HttpGet]
        public async Task<IActionResult> GetAverageReviewRating()
        {
            var result = await dashboardInterface.GetAverageReviewRatingAsync();
            return Ok(result);
        }

        [Route("api/users/most-active")]
        [HttpGet]
        public async Task<IActionResult> GetMostActiveUsers([FromQuery] int count)
        {
            var result = await dashboardInterface.GetMostActiveUsersAsync(count);
            return Ok(result);
        }

        [Route("api/reports/count-by-site")]
        [HttpGet]
        public async Task<IActionResult> GetReportsCountBySite()
        {
            var result = await dashboardInterface.GetReportsCountBySiteAsync();
            return Ok(result);
        }

        [HttpGet("total-sites-count")]
        public async Task<IActionResult> GetTotalSitesCount()
        {
            try
            {
                var count = await dashboardInterface.GetTotalSitesCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("total-users-count")]
        public async Task<IActionResult> GetTotalUsersCount()
        {
            try
            {
                var count = await dashboardInterface.GetTotalUsersCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-latest-report")]
        public async Task<IActionResult> GetLatestReportsAsync(int count)
        {
            try
            {
                var counts = await dashboardInterface.GetLatestReportsAsync(count);
                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-popular-site")]
        public async Task<IActionResult> GetMostPopularSitesAsync(int count)
        {
            try
            {
                var counts = await dashboardInterface.GetMostPopularSitesAsync(count);
                return Ok(counts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
