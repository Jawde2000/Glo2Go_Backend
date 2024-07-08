using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using ServerLibrary.Repositories.Contracts;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherService WeatherInterface) : ControllerBase
    {
        [HttpPost("forecast")]
        public async Task<IActionResult> GetForecast(WeatherDTO weather)
        {
            var result = await WeatherInterface.GetForecastData(weather);
            if (result == null)
            {
                return NotFound("Forecast data not found.");
            }
            return Ok(result);
        }

        [HttpPost("historical")]
        public async Task<IActionResult> GetHistorical(WeatherDTO weather)
        {
            var result = await WeatherInterface.GetHistoricalData(weather);
            if (result == null)
            {
                return NotFound("Historical data not found.");
            }
            return Ok(result);
        }
    }
}
