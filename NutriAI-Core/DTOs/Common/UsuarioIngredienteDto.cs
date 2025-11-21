using System;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class UsuarioIngredienteDto
    {
        public int IdUsuarioIngrediente { get; set; }
        public int IdUsuario { get; set; }
        public int IdIngrediente { get; set; }
        public decimal? CantidadGramos { get; set; }
        public DateTime FechaRegistro { get; set; }
        
        // Información del ingrediente incluida para mayor comodidad
        public IngredienteDto? Ingrediente { get; set; }
    }
}