using NutriAI_Core.DTOs.Auth;

namespace NutriAI_Core.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken ct);
        Task<TokenResponse> RefreshPairAsync(string accessToken, string refreshToken, CancellationToken ct);

        Task<int> RegisterAsync(RegisterRequest request, CancellationToken ct);
        Task<string> ForgotPasswordAsync(string emailOrLogin, CancellationToken ct);
        Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken ct);
        Task<bool> ChangePasswordAsync(string login, string currentPassword, string newPassword, CancellationToken ct);
    }
}
