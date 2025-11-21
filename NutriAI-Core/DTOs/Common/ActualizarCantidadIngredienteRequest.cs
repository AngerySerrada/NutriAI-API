using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class ActualizarCantidadIngredienteRequest
    {
        [Required(ErrorMessage = "El ID de la asociación es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la asociación debe ser mayor a 0")]
        public int IdUsuarioIngrediente { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "La cantidad debe estar entre 0.01 y 9999.99 gramos")]
        public decimal? CantidadGramos { get; set; }
    }
}