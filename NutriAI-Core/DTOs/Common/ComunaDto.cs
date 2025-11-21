using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class ComunaDto
    {
        public int IdComuna { get; set; }
        public string Nombre { get; set; } = default!;
        public bool Activo { get; set; }
    }
}