using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount accountInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto User)
        {
            if (User == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.RegisterAsync(User);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto User)
        {
            if (User == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.LoginAsync(User);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto token)
        {
            if (token == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.RefreshTokenAsync(token);
            return Ok(result);
        }
    }
}
