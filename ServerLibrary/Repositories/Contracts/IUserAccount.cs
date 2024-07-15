using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IUserAccount
    {
        public Task<GeneralResponse> RegisterAsync(UserRegisterDto user);
        public Task<LoginResponse> LoginAsync(UserLoginDto user);

        public Task<LoginResponse> AdminLoginAsync(UserLoginDto user);

        public Task<GeneralResponse> UpdateTravelerAsync(UserUpdateDTO user);

        public Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken);

        public Task<GeneralResponse> SendEmailAsync(UserForgotPasswordDto user);

        public Task<GeneralResponse> SendResetAsync(UserForgotPasswordDto user);

        public Task<GeneralResponse> DeleteTravelerAsync(UserDeleteDTO user);

        public Task<UserList> ListAllUsersAsync();

        public Task<GeneralResponse> GetUserInformationAsync(UserInfoDTO user);

        public Task<GeneralResponse> ValidateTokenAsync(string token);

        public Task<GeneralResponse> InvalidateCurrentTokenAsync(string token);
        public Task<GeneralResponse> RegisterUserWithRoleAsync(UserRegisterAdminDto user);

        public Task<GeneralResponse> UpdatePasswordAsync(string token, string newPassword);
        public Task<GeneralResponse> CheckOtpExistAsync(string user, string otp);
    }
}
