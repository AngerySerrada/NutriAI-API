using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.DTOs.Common
{
    public sealed class IngredienteDto
    {
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carbohidratos { get; set; }
        public decimal Grasas { get; set; }
    }
}