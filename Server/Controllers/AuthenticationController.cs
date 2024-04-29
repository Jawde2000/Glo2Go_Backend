using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;

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

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLoginAsync(UserLoginDto User)
        {
            if (User == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.AdminLoginAsync(User);
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

        [HttpPut("updateTraveler")]
        public async Task<IActionResult> UpdateTravelerAsync(UserUpdateDTO traveler)
        {
            if (traveler == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.UpdateTravelerAsync(traveler);
            return Ok(result);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> SendEmailAsync(UserForgotPasswordDto traveler)
        {
            if (traveler == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.SendEmailAsync(traveler);
            return Ok(result);
        }

        [HttpPost("passwordreset")]
        public async Task<IActionResult> SendResetAsync(UserForgotPasswordDto traveler)
        {
            if (traveler == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.SendResetAsync(traveler);
            return Ok(result);
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteTravelerAsync(UserDeleteDTO traveler)
        {
            if (traveler == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.DeleteTravelerAsync(traveler);
            return Ok(result);
        }

    }
}
