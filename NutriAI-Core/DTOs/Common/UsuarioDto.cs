using System;
using System.Collections.Generic;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = default!;
        public string Correo { get; set; } = default!;
        public int Edad { get; set; }
        public int IdComuna { get; set; }
        public int IdNivelActividad { get; set; }
        public int IdSexo { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public string Username { get; set; } = default!;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Información relacionada para mayor comodidad
        public ComunaDto? Comuna { get; set; }
        public NivelActividadDto? NivelActividad { get; set; }
        public SexoDto? Sexo { get; set; }
        public List<PerfilDto>? Perfiles { get; set; }
    }
}