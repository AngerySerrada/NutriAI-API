namespace NutriAI_Core.DTOs.AI
{
    public sealed class AIMessageDto
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int? TokensUsed { get; set; }
    }
}
