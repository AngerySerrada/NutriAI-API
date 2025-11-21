namespace NutriAI_Core.DTOs.Auth
{
    public sealed class TokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshExpiresAtUtc { get; set; }
    }
}
