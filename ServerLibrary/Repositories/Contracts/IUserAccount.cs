﻿using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IUserAccount
    {
        public Task<GeneralResponse> RegisterAsync(UserRegisterDto user);
        public Task<LoginResponse> LoginAsync(UserLoginDto user);
        public Task<GeneralResponse> UpdateTravelerAsync(UserUpdateDTO user);

        public Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto refreshToken);

        public Task<GeneralResponse> SendEmailAsync(UserForgotPasswordDto user);

        public Task<GeneralResponse> SendResetAsync(UserForgotPasswordDto user);

        public Task<GeneralResponse> DeleteTravelerAsync(UserDeleteDTO user);
    }
}