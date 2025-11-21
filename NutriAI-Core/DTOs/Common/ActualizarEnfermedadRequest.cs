using System;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class ActualizarEnfermedadRequest
    {
        public int IdUsuarioEnfermedad { get; set; }
        public DateOnly? FechaDiagnostico { get; set; }
        public string? Observaciones { get; set; }
    }
}
