using BattleFace.Application.DTOs;

namespace BattleFace.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> GoogleAsync(GoogleRequestDto request);

        Task ForgotPasswordAsync(string email, string originUrl);
        Task ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
