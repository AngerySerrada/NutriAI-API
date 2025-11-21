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
    }
}