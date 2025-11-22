using NutriAI_Core.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.Interfaces
{
    public interface IIngredienteService
    {
        /// <summary>
        /// Obtiene todos los ingredientes disponibles
        /// </summary>
        Task<IEnumerable<IngredienteDto>> GetIngredientesAsync(CancellationToken ct = default);

        /// <summary>
        /// Obtiene un ingrediente por su ID
        /// </summary>
        Task<IngredienteDto?> GetIngredienteByIdAsync(int idIngrediente, CancellationToken ct = default);

        /// <summary>
        /// Busca ingredientes por nombre
        /// </summary>
        Task<IEnumerable<IngredienteDto>> SearchIngredientesByNombreAsync(string nombre, CancellationToken ct = default);

        /// <summary>
        /// Obtiene ingredientes filtrados por categoría
        /// </summary>
        Task<IEnumerable<IngredienteDto>> GetIngredientesByCategoriaAsync(string categoria, CancellationToken ct = default);

        /// <summary>
        /// Obtiene todas las categorías de ingredientes disponibles
        /// </summary>
        Task<IEnumerable<string>> GetCategoriasAsync(CancellationToken ct = default);

        // =======================
        // CRUD INGREDIENTES
        // =======================

        /// <summary>
        /// Crea un nuevo ingrediente
        /// </summary>
        Task<IngredienteDto> CrearIngredienteAsync(CrearIngredienteRequest request, CancellationToken ct = default);

        /// <summary>
        /// Actualiza un ingrediente existente
        /// </summary>
        Task<IngredienteDto?> ActualizarIngredienteAsync(ActualizarIngredienteRequest request, CancellationToken ct = default);

        /// <summary>
        /// Elimina un ingrediente por su ID
        /// </summary>
        Task<bool> EliminarIngredienteAsync(int idIngrediente, CancellationToken ct = default);

        // =======================
        // ASOCIACIONES USUARIO-INGREDIENTE
        // =======================

        /// <summary>
        /// Asocia un ingrediente con un usuario
        /// </summary>
        Task<UsuarioIngredienteDto> AsociarIngredienteAsync(AsociarIngredienteRequest request, CancellationToken ct = default);

        /// <summary>
        /// Desasocia un ingrediente de un usuario
        /// </summary>
        Task<bool> DesasociarIngredienteAsync(int idUsuario, int idIngrediente, CancellationToken ct = default);

        /// <summary>
        /// Obtiene todos los ingredientes asociados a un usuario
        /// </summary>
        Task<IEnumerable<UsuarioIngredienteDto>> GetIngredientesUsuarioAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Verifica si un ingrediente está asociado a un usuario
        /// </summary>
        Task<bool> IsIngredienteAsociadoAsync(int idUsuario, int idIngrediente, CancellationToken ct = default);

        /// <summary>
        /// Actualiza la cantidad de gramos de un ingrediente asociado
        /// </summary>
        Task<bool> ActualizarCantidadIngredienteAsync(ActualizarCantidadIngredienteRequest request, CancellationToken ct = default);

        /// <summary>
        /// Obtiene una asociación específica usuario-ingrediente
        /// </summary>
        Task<UsuarioIngredienteDto?> GetUsuarioIngredienteAsync(int idUsuario, int idIngrediente, CancellationToken ct = default);

        /// <summary>
        /// Elimina una asociación específica por su ID
        /// </summary>
        Task<bool> EliminarAsociacionAsync(int idUsuarioIngrediente, CancellationToken ct = default);

        // =======================
        // MÉTODOS PARA GRÁFICOS
        // =======================

        /// <summary>
        /// Obtiene estadísticas nutricionales totales de los ingredientes de un usuario
        /// </summary>
        Task<EstadisticasNutricionalesDto> GetEstadisticasUsuarioAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Obtiene distribución de macronutrientes por categoría para un usuario
        /// </summary>
        Task<IEnumerable<MacronutrientesPorCategoriaDto>> GetMacronutrientesPorCategoriaAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Obtiene los ingredientes más consumidos por un usuario (top N)
        /// </summary>
        Task<IEnumerable<IngredienteMasConsumidoDto>> GetIngredientesMasConsumidosAsync(int idUsuario, int top = 10, CancellationToken ct = default);

        /// <summary>
        /// Obtiene comparativa de calorías por categoría
        /// </summary>
        Task<IEnumerable<CaloriasPorCategoriaDto>> GetCaloriasPorCategoriaAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Obtiene el balance nutricional diario de un usuario
        /// </summary>
        Task<BalanceNutricionalDto> GetBalanceNutricionalAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Obtiene el historial de ingredientes agregados por fecha (últimos N días)
        /// </summary>
        Task<IEnumerable<HistorialIngredientesDto>> GetHistorialIngredientesAsync(int idUsuario, int dias = 30, CancellationToken ct = default);

        /// <summary>
        /// Obtiene estadísticas de variedad de ingredientes por categoría
        /// </summary>
        Task<IEnumerable<VariedadCategoriasDto>> GetVariedadCategoriasAsync(int idUsuario, CancellationToken ct = default);
    }
}