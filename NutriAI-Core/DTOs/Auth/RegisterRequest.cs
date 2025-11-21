using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Auth
{
    public sealed class RegisterRequest
    {
        public string Login { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? NombreUsuario { get; set; }
        
        // Campos adicionales del usuario
        public int Edad { get; set; }
        public int IdComuna { get; set; }
        public int IdNivelActividad { get; set; }
        public int IdSexo { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
    }
}
