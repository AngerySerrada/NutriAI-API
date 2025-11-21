namespace NutriAI_Core.DTOs.Common
{
    public sealed class EnfermedadDto
    {
        public int IdEnfermedad { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
