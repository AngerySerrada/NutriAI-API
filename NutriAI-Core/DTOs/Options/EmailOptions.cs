namespace NutriAI_Core.DTOs.Options
{
    public sealed class EmailOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; } = 587;
        public string From { get; set; } = default!;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseSsl { get; set; } = true;
    }
}
