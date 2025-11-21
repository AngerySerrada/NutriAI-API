using System;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class UsuarioEnfermedadDto
    {
        public int IdUsuarioEnfermedad { get; set; }
        public int IdUsuario { get; set; }
        public int IdEnfermedad { get; set; }
        public DateOnly? FechaDiagnostico { get; set; }
        public string? Observaciones { get; set; }
        
        // Información de la enfermedad incluida para mayor comodidad
        public EnfermedadDto? Enfermedad { get; set; }
    }
}
