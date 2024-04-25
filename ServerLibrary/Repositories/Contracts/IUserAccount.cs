using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IUserAccount
    {
        public Task<GeneralResponse> RegisterAsync(UserRegisterDto user);
        public Task<LoginResponse> LoginAsync(UserLoginDto user);

        public Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken);
    }
}
