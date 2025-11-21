using System;
using System.Collections.Generic;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class PerfilDto
    {
        public int IdPerfil { get; set; }
        public string Descripcion { get; set; } = default!;
    }
}