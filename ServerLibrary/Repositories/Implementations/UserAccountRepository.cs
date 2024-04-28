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
using Humanizer;
using System.Net.Mail;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace ServerLibrary.Repositories.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config, Glo2GoDbContext dbContext) : IUserAccount
    {
        public async Task<GeneralResponse> RegisterAsync(UserRegisterDto user)
        {
            if (user == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var checkUser = await FindUserByEmail(user.Email);
            if (!IsPasswordStrong(user.Password))
                return new GeneralResponse(false, "Oops! Your current password doesn't meet the requirements. " +
                    "A strong password must contain at least one digit, " +
                    "one lower case letter, one upper case letter, and one special character. " +
                    "Also, it must be between 8 and 15 characters long. " +
                    "Let's try again with these guidelines in mind!");

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

            // check, create and assign role
            var checkAdminRole = await dbContext.SystemRoles.FirstOrDefaultAsync(_ => _.Name!.Equals(Constants.Admin));
            if (checkAdminRole is null)
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

        private bool IsPasswordStrong(string password)
        {
            // Minimum and Maximum Length of password
            if (password.Length < 8 || password.Length > 15)
                return false;

            // Check if password contains at least one digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check if password contains at least one lower case letter
            if (!password.Any(char.IsLower))
                return false;

            // Check if password contains at least one upper case letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check if password contains at least one special character
            if (!password.Any(ch => !Char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }


        public async Task<LoginResponse> LoginAsync(UserLoginDto user)
        {
            if (user == null) return new LoginResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var Traveler = await FindUserByEmail(user.Email!);

            if (Traveler is null)
            {
                return new LoginResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }


            if ((bool)Traveler.IsLocked)
            {
                return new LoginResponse(false, "This account has been locked due to too many failed login attempts. " +
                    "Please contact our support team at glo2go-support@gmail.com or call us at (123) 456-7890 for assistance.");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, Traveler.TravelerPass))
            {
                Traveler.FailedLoginAttempt++;
                if (Traveler.FailedLoginAttempt >= 6)
                {
                    Traveler.IsLocked = true;
                }
                await dbContext.SaveChangesAsync();

                if (Traveler.FailedLoginAttempt == 1) 
                { 
                    return new LoginResponse(false, "Oops! It seems the password you entered isn’t valid.");
                } 
                else if (Traveler.FailedLoginAttempt == 5)
                {
                    return new LoginResponse(false, "Attention! Your login attempt was unsuccessful. " +
                           "This is your final attempt before your account is locked. " +
                           "After multiple unsuccessful attempts, your account will be locked for security reasons. " +
                           "If you've forgotten your password, please visit the 'Forgot Password' page to reset it.");
                }
                else
                {
                    return new LoginResponse(false, "Oops! Your login attempt was unsuccessful. " +
                        "This is your " + Traveler.FailedLoginAttempt + " failed attempt. " +
                        "Please note that after multiple unsuccessful attempts, " +
                        "your account may be locked for security reasons. " +
                        "Please double-check your credentials and try again");
                }
                
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

            if (Traveler.FailedLoginAttempt > 0)
            {
                Traveler.FailedLoginAttempt = 0;
                Traveler.IsLocked = false;
                await dbContext.SaveChangesAsync();
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

        public async Task<LoginResponse> AdminLoginAsync(UserLoginDto user)
        {
            if (user == null) return new LoginResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var admin = await FindUserByEmail(user.Email!);

            if (admin is null)
            {
                return new LoginResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, admin.TravelerPass))
            {
                return new LoginResponse(false, "Oops! It seems the email or password you entered isn’t valid.");
            }

            var getUserRoles = await FindUserRole(admin.Id);

            if (getUserRoles is null)
            {
                return new LoginResponse(false, "Sorry, the user role you entered was not found.");
            }

            // Check if the user's role is 'admin'
            if (getUserRoles.RoleId != 2) // Assuming 2 is the roleId for 'admin'
            {
                return new LoginResponse(false, "Sorry, your access has been denied. This area is reserved for administrators only.");
            }

            var getRoleName = await FindRoleName(getUserRoles.RoleId!);

            if (getRoleName is null)
            {
                return new LoginResponse(false, "Sorry, the user role you entered was not found.");
            }

            string jwtToken = GenerateToken(admin, getRoleName!.Name!);
            string refreshToken = GenerateRefreshToken();

            var findUser = await dbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.userId!.Equals(admin.Id));

            if (findUser is not null)
            {
                findUser!.Token = refreshToken;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDB(new RefreshTokenInfo() { Token = refreshToken, userId = admin.Id });
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

        public async Task<GeneralResponse> UpdateTravelerAsync(UserUpdateDTO user)
        {
            // Find the traveler by email
            var traveler = await dbContext.Travelers
                .Include(t => t.Address) // Include the Address in the query
                .FirstOrDefaultAsync(t => t.TravelerEmail == user.TravelerEmail);

            if (traveler == null)
            {
                return new GeneralResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }

            // Update the traveler's properties
            traveler.Name = user.Name ?? traveler.Name;
            traveler.FirstName = user.FirstName;
            traveler.LastName = user.LastName;
            traveler.ProfilePic = user.ProfilePic ?? traveler.ProfilePic;
            traveler.Gender = user.Gender ?? traveler.Gender;

            // If the password is provided, hash it and update the traveler's password
            if (!string.IsNullOrEmpty(user.TravelerPass))
            {
                traveler.TravelerPass = BCrypt.Net.BCrypt.HashPassword(user.TravelerPass);
            }

            // If the traveler has an address, update it
            if (traveler.Address != null && user.Address != null)
            {
                traveler.Address.TravelAddress = user.Address.TravelAddress ?? traveler.Address.TravelAddress;
                traveler.Address.Country = user.Address.Country ?? traveler.Address.Country;
            }

            // Save the changes to the database
            dbContext.Travelers.Update(traveler);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Great news! Your account has been updated successfully.");
        }

        public async Task<GeneralResponse> DeleteTravelerAsync(UserDeleteDTO user)
        {
            if (user == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var deleteUser = await FindUser(user);
            if (deleteUser == null) return new GeneralResponse(false, "Oops! The user you’re trying to access doesn’t exist. Please check the details and try again.");

            dbContext.Travelers.Remove(deleteUser);
            await dbContext.SaveChangesAsync();

            return new GeneralResponse(true, "Hello Admin! The user has been successfully deleted.");
        }

        public async Task<GeneralResponse> SendEmailAsync(UserForgotPasswordDto user)
        {
            SmtpClient _smtpClient;

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential
                {
                    UserName = "go2goinc@gmail.com",
                    Password = "intk hirt mlad ucnc"
                }
            };

            var getUser = await dbContext.Travelers
                .Include(t => t.Address) // Include the Address in the query
                .FirstOrDefaultAsync(t => t.TravelerEmail == user.Email!);

            if (getUser == null)
            {
                return new GeneralResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }

            // Generate a JWT for password reset
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config.Value.Key); // Replace with your secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, getUser.TravelerEmail)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token is valid for 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resetToken = tokenHandler.WriteToken(token);

            // Create the password reset link
            var resetLink = $"http://localhost:3000/reset-password?token={resetToken}";

            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("glo2goinc@gmail.com", "Glo2Go"),
                    Subject = "**Reset Your Password Request**",
                    Body = "**Hello!**" +
                    "\r\n\r\nWe've received a request to reset your password. " +
                    "To help you get back into your account quickly, we've generated a password reset link for you.\r\n\r\n" +
                    $"**Reset Link: Click here to reset your password**\r\n\r\n" +
                    "This link will expire in 1 hour. If you didn't make this request, please ignore this email or contact our support team.\r\n\r\n" +
                    "**Best regards,\r\nGlo2Go Team**\r\n",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(getUser.TravelerEmail!);

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                throw;
            }

            return new GeneralResponse(true, "Success! An email with a password reset link has been sent to your account. Please check your inbox and follow the instructions to reset your password.");
        }

        public async Task<GeneralResponse> SendResetAsync(UserForgotPasswordDto user)
        {
            SmtpClient _smtpClient;

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential
                {
                    UserName = "go2goinc@gmail.com",
                    Password = "intk hirt mlad ucnc"
                }
            };

            var getUser = await dbContext.Travelers
                .Include(t => t.Address) // Include the Address in the query
                .FirstOrDefaultAsync(t => t.TravelerEmail == user.Email!);

            if (getUser == null)
            {
                return new GeneralResponse(false, "Sorry, we couldn’t find an account with that information. Please double-check your details and try again.");
            }

            // Generate a JWT for password reset
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config.Value.Key); // Replace with your secret key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, getUser.TravelerEmail)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token is valid for 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resetToken = tokenHandler.WriteToken(token);

            // Create the password reset link
            var resetLink = $"http://localhost:3000/glo2go/reset-password?token={resetToken}";

            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("glo2goinc@gmail.com", "Glo2Go"),
                    Subject = "**Reset Your Password Request**",
                    Body = "**Hello!**" +
                    "\r\n\r\nWe've received a request to reset your password. " +
                    "To help you get back into your account quickly, we've generated a password reset link for you.\r\n\r\n" +
                    $"**Reset Link: {resetLink} **\r\n\r\n" +
                    "This link will expire in 1 hour. If you didn't make this request, please ignore this email or contact our support team.\r\n\r\n" +
                    "**Best regards,\r\nGlo2Go Team**\r\n",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(getUser.TravelerEmail!);

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                throw;
            }

            return new GeneralResponse(true, "Success! An email with a password reset link has been sent to your account. Please check your inbox and follow the instructions to reset your password.");
        }


        private async Task<Traveler> FindUser(UserDeleteDTO user)
        {
            return await dbContext.Travelers.FirstOrDefaultAsync(_ => _.TravelerEmail!.Equals(user.Email!));
        }

        private string GenerateSecureID(int length = 16)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
