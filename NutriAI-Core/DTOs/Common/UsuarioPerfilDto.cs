using System;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class UsuarioPerfilDto
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Activo { get; set; }

        // Información relacionada
        public UsuarioDto? Usuario { get; set; }
        public PerfilDto? Perfil { get; set; }
    }
}