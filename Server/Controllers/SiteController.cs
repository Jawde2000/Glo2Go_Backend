﻿using Microsoft.AspNetCore.Http;
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
    public class SiteController : Controller
    {
        private readonly ISiteAccount _siteInterface;

        public SiteController(ISiteAccount siteInterface)
        {
            _siteInterface = siteInterface;
        }

        [HttpPost("AddSite")]
        public async Task<IActionResult> AddSiteAsync(AddSiteDto site)
        {
            if (site == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _siteInterface.AddSiteAsync(site);
            return Ok(result);
        }

        [HttpDelete("DeleteSite")]
        public async Task<IActionResult> DeleteSiteAsync(DeleteSiteDTO site)
        {
            if (site == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _siteInterface.DeleteSiteAsync(site);

            Console.WriteLine(result);
            return Ok(result);
        }

        [HttpPut("UpdateSite")]
        public async Task<IActionResult> UpdateSiteAsync(UpdateSiteDTO site)
        {
            if (site == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _siteInterface.UpdateSiteAsync(site);
            return Ok(result);
        }

        [HttpGet("ViewSites")]
        public async Task<IActionResult> ViewSitesAsync()
        {
            var result = await _siteInterface.ViewSitesAsync();
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var sites = JsonConvert.DeserializeObject<List<Site>>(result.Message);

            return Ok(sites);
        }

        [HttpPost("site")]
        public async Task<IActionResult> GetSiteAsync(SiteDetailsDTO site)
        {
            if (site == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _siteInterface.GetSiteAsync(site);

            var details = JsonConvert.DeserializeObject<Site>(result.Message);

            return Ok(details);
        }

        [HttpGet("TopPopularSites")]
        public async Task<IActionResult> GetTop3PopularSitesAsync()
        {
            var result = await _siteInterface.GetTop3PopularSitesAsync();
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var top3Sites = JsonConvert.DeserializeObject<List<Site>>(result.Message);

            return Ok(top3Sites);
        }

        [HttpGet("GetRecommendedSites")]
        public async Task<IActionResult> GetRecommendedSitesAsync()
        {
            var result = await _siteInterface.GetRecommendedSitesAsync();

            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var GetRecommendedSitesAsync = JsonConvert.DeserializeObject<List<Site>>(result.Message);

            return Ok(GetRecommendedSitesAsync);
        }
    }
}
