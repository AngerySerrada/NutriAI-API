using System;
using System.Collections.Generic;

namespace NutriAI_Core.DTOs.Common
{
    /// <summary>
    /// Estadísticas nutricionales totales de los ingredientes de un usuario
    /// </summary>
    public sealed class EstadisticasNutricionalesDto
    {
        public decimal TotalCalorias { get; set; }
        public decimal TotalProteinas { get; set; }
        public decimal TotalCarbohidratos { get; set; }
        public decimal TotalGrasas { get; set; }
        public int TotalIngredientes { get; set; }
        public decimal TotalGramos { get; set; }
    }

    /// <summary>
    /// Distribución de macronutrientes por categoría
    /// </summary>
    public sealed class MacronutrientesPorCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Proteinas { get; set; }
        public decimal Carbohidratos { get; set; }
        public decimal Grasas { get; set; }
        public decimal Calorias { get; set; }
        public int CantidadIngredientes { get; set; }
    }

    /// <summary>
    /// Ingrediente más consumido por un usuario
    /// </summary>
    public sealed class IngredienteMasConsumidoDto
    {
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal CantidadTotalGramos { get; set; }
        public decimal CaloriasTotales { get; set; }
        public int VecesConsumido { get; set; }
    }

    /// <summary>
    /// Calorías totales por categoría
    /// </summary>
    public sealed class CaloriasPorCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal TotalCalorias { get; set; }
        public int CantidadIngredientes { get; set; }
        public decimal PromedioCaloriasPorIngrediente { get; set; }
    }

    /// <summary>
    /// Balance nutricional con porcentajes de macronutrientes
    /// </summary>
    public sealed class BalanceNutricionalDto
    {
        public decimal TotalCalorias { get; set; }
        public decimal PorcentajeProteinas { get; set; }
        public decimal PorcentajeCarbohidratos { get; set; }
        public decimal PorcentajeGrasas { get; set; }
        
        // Valores absolutos en gramos
        public decimal GramosProteinas { get; set; }
        public decimal GramosCarbohidratos { get; set; }
        public decimal GramosGrasas { get; set; }
        
        // Calorías por macronutriente
        public decimal CaloriasProteinas { get; set; }
        public decimal CaloriasCarbohidratos { get; set; }
        public decimal CaloriasGrasas { get; set; }
    }

    /// <summary>
    /// Historial de ingredientes agregados por fecha
    /// </summary>
    public sealed class HistorialIngredientesDto
    {
        public DateTime Fecha { get; set; }
        public int CantidadIngredientes { get; set; }
        public decimal TotalGramos { get; set; }
        public decimal TotalCalorias { get; set; }
    }

    /// <summary>
    /// Variedad de ingredientes por categoría
    /// </summary>
    public sealed class VariedadCategoriasDto
    {
        public string Categoria { get; set; } = string.Empty;
        public int CantidadIngredientes { get; set; }
        public decimal PorcentajeDelTotal { get; set; }
        public List<string> IngredientesPrincipales { get; set; } = new();
    }
}
