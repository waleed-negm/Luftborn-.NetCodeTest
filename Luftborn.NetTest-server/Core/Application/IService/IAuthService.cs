
using Core.Application.Dto;
using Core.Application.Dto.auth;

namespace Core.Application.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> RegisterAsync(RegisterDto model);
        Task<ResponseDto> ConfirmEmailAsync(string userId, string token);
        public Task<ResponseDto> EditUser(string Id, UserDto model);
        Task<ResponseDto> GetTokenAsync(LoginCredentialsDto model);
        Task<ResponseDto> ForgetPasswordAsync(string email);
        Task<ResponseDto> ResetPasswordAsync(ResetPassowrdDto model);
        Task<ResponseDto> ChangePasswordAsync(string id, UpdatePassowrdDto model);
        Task<ResponseDto> GetUserById(string id);
    }
}
