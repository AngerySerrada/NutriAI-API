using System;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class AsociarEnfermedadRequest
    {
        public int IdUsuario { get; set; }
        public int IdEnfermedad { get; set; }
        public DateOnly? FechaDiagnostico { get; set; }
        public string? Observaciones { get; set; }
    }
}
