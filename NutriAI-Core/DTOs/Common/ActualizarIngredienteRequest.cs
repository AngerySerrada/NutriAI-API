using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class ActualizarIngredienteRequest
    {
        [Required(ErrorMessage = "El ID del ingrediente es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del ingrediente debe ser mayor a 0")]
        public int IdIngrediente { get; set; }

        [Required(ErrorMessage = "El nombre del ingrediente es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es requerida")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Las calorías son requeridas")]
        [Range(0, 9999.99, ErrorMessage = "Las calorías deben estar entre 0 y 9999.99")]
        public decimal Calorias { get; set; }

        [Required(ErrorMessage = "Las proteínas son requeridas")]
        [Range(0, 9999.99, ErrorMessage = "Las proteínas deben estar entre 0 y 9999.99")]
        public decimal Proteinas { get; set; }

        [Required(ErrorMessage = "Los carbohidratos son requeridos")]
        [Range(0, 9999.99, ErrorMessage = "Los carbohidratos deben estar entre 0 y 9999.99")]
        public decimal Carbohidratos { get; set; }

        [Required(ErrorMessage = "Las grasas son requeridas")]
        [Range(0, 9999.99, ErrorMessage = "Las grasas deben estar entre 0 y 9999.99")]
        public decimal Grasas { get; set; }
    }
}
