using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.DTOs;
using BaseLibrary.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServerLibrary.Repositories.Implementations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using BaseLibrary.Responses;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticationController: ControllerBase
    {
        private readonly IUserAccount _accountInterface;

        public AuthenticationController(IUserAccount accountInterface)
        {
            _accountInterface = accountInterface;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto User)
        {
            if (User == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.RegisterAsync(User);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterUserWithRoleAsync([FromBody] UserRegisterAdminDto user)
        {
            if (User == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.RegisterUserWithRoleAsync(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto User)
        {
            if (User == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.LoginAsync(User);
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

            return BadRequest(result.Message);
            //return BadRequest("404 not found");
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLoginAsync([FromBody] UserLoginDto User)
        {
            if (User == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.AdminLoginAsync(User);

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

            return BadRequest(result.Message);
            //return BadRequest("404 not found");
        }

        [HttpPost("check-token-validation")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest("Invalid token request.");
            }

            var result = await _accountInterface.ValidateTokenAsync(tokenRequest.Token);
            return Ok(result);
        }

        [HttpPost("invalidate-token")]
        public async Task<IActionResult> InvalidateCurrentTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.Token))
            {
                return BadRequest("Invalid token request.");
            }

            var result = await _accountInterface.InvalidateCurrentTokenAsync(tokenRequest.Token);
            return Ok(result);
        }

        // Define a model to match the request body
        public class TokenRequest
        {
            [Required]
            public string Token { get; set; }
        }



        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto token)
        {
            if (token == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.RefreshTokenAsync(token);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateTraveler")]
        public async Task<IActionResult> UpdateTravelerAsync([FromBody] UserUpdateDTO traveler)
        {
            if (traveler == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.UpdateTravelerAsync(traveler);
            return Ok(result);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromQuery] string token, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Token and password must be provided.");
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.UpdatePasswordAsync(token, password);
            return Ok(result);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> SendEmailAsync([FromBody] UserForgotPasswordDto traveler)
        {
            if (traveler == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.SendEmailAsync(traveler);
            return Ok(result);
        }

        [HttpPost("passwordreset")]
        public async Task<IActionResult> SendResetAsync([FromBody] UserForgotPasswordDto traveler)
        {
            if (traveler == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.SendResetAsync(traveler);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteTravelerAsync([FromBody] UserDeleteDTO traveler)
        {
            if (traveler == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.DeleteTravelerAsync(traveler);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getuser")]
        public async Task<IActionResult> ListAllUsersAsync()
        {

            var result = await _accountInterface.ListAllUsersAsync();
            if (!result.Flag)
            {
                return BadRequest(result.Message);
            }

            // Parse the JSON string back to a list of sites
            var users = JsonConvert.DeserializeObject<List<Traveler>>(result.Message!);

            return Ok(users);
        }

        [HttpPost("otp")]
        public async Task<IActionResult> CheckOtpExistAsync([FromQuery] string user, [FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(otp))
            {
                return BadRequest("User and OTP must be provided.");
                //return BadRequest("Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            var result = await _accountInterface.CheckOtpExistAsync(user, otp);
            return Ok(result);
        }

        [HttpPost("userinfo")]
        public async Task<IActionResult> GetUserInformationAsync([FromBody] UserInfoDTO user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Extract the email from the principal (set during the JWT Bearer authentication)

            var response = await _accountInterface.GetUserInformationAsync(user);
            if (!response.Flag)
                return BadRequest(response.Message);

            var userInfo = JsonConvert.DeserializeObject<Traveler>(response.Message);

            return Ok(userInfo);
        }

    }
}
