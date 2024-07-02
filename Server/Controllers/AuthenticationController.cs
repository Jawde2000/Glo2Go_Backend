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
/*    [Authorize]*/

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

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterUserWithRoleAsync(UserRegisterAdminDto user)
        {
            if (User == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.RegisterUserWithRoleAsync(user);
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
            if (result.Flag) // Assuming there is an IsSuccess property to check if login was successful
            {
                // Assuming result.data is a JSON string, deserialize it first

                // Create a new object with all necessary properties
                var response = new
                {
                    result.Flag,
                    result.Message,
                    result.Token,
                    result.RefreshToken,
                    result.data
                };

                return Ok(response);
            }

            return BadRequest("404 not found");
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLoginAsync(UserLoginDto User)
        {
            if (User == null)
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.AdminLoginAsync(User);

            if (result.Flag) // Assuming there is an IsSuccess property to check if login was successful
            {
                // Assuming result.data is a JSON string, deserialize it first

                // Create a new object with all necessary properties
                var response = new
                {
                    result.Flag,
                    result.Message,
                    result.Token,
                    result.RefreshToken,
                    result.data
                };

                return Ok(response);
            }

            return BadRequest("404 not found");
        }

        [HttpPost("check-token-validation")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.ValidateTokenAsync(tokenRequest.Token);
            return Ok(result);
        }

        [HttpPost("invalidate-token")]
        public async Task<IActionResult> InvalidateCurrentTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await accountInterface.InvalidateCurrentTokenAsync(tokenRequest.Token);
            return Ok(result);
        }

        // Define a model to match the request body
        public class TokenRequest
        {
            public string Token { get; set; }
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

        [HttpGet("getuser")]
        public async Task<IActionResult> ListAllUsersAsync()
        {

            var result = await accountInterface.ListAllUsersAsync();
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var randomReview = JsonConvert.DeserializeObject<List<Traveler>>(result.Message!);

            return Ok(randomReview);
        }

        [HttpPost("userinfo")]
        public async Task<IActionResult> GetUserInformationAsync(UserInfoDTO user)
        {
            // Extract the email from the principal (set during the JWT Bearer authentication)

            var response = await accountInterface.GetUserInformationAsync(user);
            if (!response.Flag)
                return BadRequest(response.Message);

            var JsonResponse = JsonConvert.DeserializeObject<Traveler>(response.Message);

            return Ok(JsonResponse);
        }

    }
}
