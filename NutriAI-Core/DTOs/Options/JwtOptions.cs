namespace NutriAI_Core.DTOs.Options
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpireMinutes { get; set; }
        public string RefreshKey { get; set; } = default!;
        public int RefreshExpireDays { get; set; }
    }
}
