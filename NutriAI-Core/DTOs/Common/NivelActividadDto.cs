using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class NivelActividadDto
    {
        public int IdNivelActividad { get; set; }
        public string Descripcion { get; set; } = default!;
        public bool Activo { get; set; }
    }
}