using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController(ISiteAccount siteInterface) : Controller
    {
        [HttpPost("AddSite")]
        public async Task<IActionResult> AddSiteAsync(AddSiteDto site)
        {
            if (site == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await siteInterface.AddSiteAsync(site);
            return Ok(result);
        }
    }
}
