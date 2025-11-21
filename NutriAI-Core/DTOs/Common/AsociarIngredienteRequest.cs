using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class AsociarIngredienteRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El ID del ingrediente es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del ingrediente debe ser mayor a 0")]
        public int IdIngrediente { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "La cantidad debe estar entre 0.01 y 9999.99 gramos")]
        public decimal? CantidadGramos { get; set; }
    }
}