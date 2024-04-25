using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using BaseLibrary.Models;
using Microsoft.Extensions.Options;
using ServerLibrary.Context;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Linq;

namespace ServerLibrary.Repositories.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config, Glo2GoDbContext dbContext) : IUserAccount
    {
        public async Task<GeneralResponse> RegisterAsync(UserRegisterDto user)
        {
            if (user == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");



            // Validate individual fields for emptiness or basic format
/*            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.ConfirmPass))
            {
                return await Task.FromResult(new GeneralResponse(false, "Oops! It looks like you missed a spot. Please fill in the empty field to continue."));
            }*/


            /*            if (user.Password != user.ConfirmPass)
                        {
                            return new GeneralResponse(false, "Oops! The passwords don’t match. Please make sure you’re entering the same password in both fields.");
                        }

                        var validEmail = await IsValidEmail(user.Email);

                        // Further validation for email format
                        if (!validEmail) return new GeneralResponse(false, "Oops! It seems like there might be a mistake. Could you please enter a valid email address?");*/

            var checkUser = await FindUserByEmail(user.Email);
            // Perform the password hashing while the user check is running
            var hashedPasswordTask = Task.Run(() => BCrypt.Net.BCrypt.HashPassword(user.Password));

            if (checkUser != null) return new GeneralResponse(false, "It looks like you’re already registered! Please proceed to login.");

            // Get the hashed password (assumed to be ready or near completion)
            var hashedPassword = await hashedPasswordTask;

            var traveler = await AddToDB(new Traveler()
            {
                TravelerEmail = user.Email,
                TravelerPass = hashedPassword
            });

            // Parallel check for roles
            var checkAdminRoleTask = dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
            var checkUserRoleTask = dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.User));

            // Await all the potentially parallel tasks
            await Task.WhenAll(checkAdminRoleTask, checkUserRoleTask);

            // check, create and assign role
            var checkAdminRole = await dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
            if (checkAdminRole != null)
            {
                var createAdminRole = await AddToDB(new SystemRole() { Name = Constants.Admin });
                await AddToDB(new UserRole() { RoleId = createAdminRole.Id, UserId = traveler.Id});
                return new GeneralResponse(true, "Great news! Your Admin account has been successfully created.");
            }

            var checkUserRole = await dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (checkUserRole is null)
            {
                response = await AddToDB(new SystemRole() { Name = Constants.User });
                await AddToDB(new UserRole() { RoleId = response.Id, UserId = traveler.Id });
            }
            else
            {
                await AddToDB(new UserRole() { RoleId = checkUserRole.Id, UserId = traveler.Id });
            }

            return new GeneralResponse(false, "Success! Your account has been created.");
        }

        public async Task<LoginResponse> LoginAsync(UserLoginDto user)
        {
            if (user == null) return new LoginResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var Traveler = await FindUserByEmail(user.Email!);

            if (Traveler is null)
            {
                return new LoginResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, Traveler.TravelerPass))
            {
                return new LoginResponse(false, "Oops! It seems the email or password you entered isn’t valid.");
            }

            var getUserRoles = await FindUserRole(Traveler.Id);

            if (getUserRoles is null)
            {
                return new LoginResponse(false, "Sorry, the user role you entered was not found.");
            }

            var getRoleName = await FindRoleName(getUserRoles.RoleId!);

            if (getRoleName is null)
            {
                return new LoginResponse(false, "Sorry, the user role you entered was not found.");
            }

            string jwtToken = GenerateToken(Traveler, getRoleName!.Name!);
            string refreshToken = GenerateRefreshToken();

            var findUser = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.userId!.Equals(Traveler.Id));

            if (findUser is not null)
            {
                findUser!.Token = refreshToken;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDB(new RefreshTokenInfo() { Token = refreshToken, userId = Traveler.Id });
            }

            return new LoginResponse(true, "Great! You’ve successfully logged in. Welcome back!", jwtToken, refreshToken);

        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            if (refreshToken == null) return new LoginResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var findToken = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.Token!.Equals(refreshToken.Token));
            
            if (findToken is null)
            {
                return new LoginResponse(false, "Heads up! A refresh token is required. Please obtain a new token to continue.");
            }

            var user = await dbContext.Travelers.FirstOrDefaultAsync(_ => _.Id == findToken.userId);
            
            if (user is null)
            {
                return new LoginResponse(false, "Oops! We couldn’t generate a refresh token because the user was not found.");
            }

            var userRole = await FindUserRole(user.Id);
            var roleName = await FindRoleName(userRole.RoleId);
            string jwtToken = GenerateToken(user, roleName.Name!);
            string Token = GenerateRefreshToken();

            var updateRefreshToken = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.userId == user.Id);
            if (updateRefreshToken is null)
            {
                return new LoginResponse(false, "Oops! We couldn’t generate a refresh token because the user has not signed in.");
            }

            updateRefreshToken.Token = Token;
            await dbContext.SaveChangesAsync();
            return new LoginResponse(true, "Success! Your token has been refreshed.", jwtToken, Token);
        }

        private async Task<UserRole> FindUserRole(int userId)
        {
            return await dbContext.UserRoles.FirstOrDefaultAsync(_ => _.UserId == userId);
        }

        private async Task<SystemRole> FindRoleName(int roleId)
        {
            return await dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Id == roleId);
        }
        private async Task<Traveler> FindUserByEmail(string email)
        {
            return await dbContext.Travelers.FirstOrDefaultAsync(_ => _.TravelerEmail!.ToLower()!.Equals(email!.ToLower()));
        }
        private async Task<T> AddToDB<T>(T model)
        {
                var result = dbContext.Add(model!);
                await dbContext.SaveChangesAsync();
                return (T)result.Entity;
        }

        private string GenerateToken(Traveler user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.TravelerEmail!),
                new Claim(ClaimTypes.Role, role!),
            };

            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credential
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private Task<bool> IsValidEmail(string email)
        {
            var attribute = new EmailAddressAttribute();
            return Task.FromResult(attribute.IsValid(email));
        }
    } 
}
