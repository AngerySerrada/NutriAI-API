using NutriAI_Core.DTOs.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NutriAI_Core.Interfaces
{
    public interface IEnfermedadService
    {
        /// <summary>
        /// Obtiene todas las enfermedades disponibles
        /// </summary>
        Task<IEnumerable<EnfermedadDto>> GetEnfermedadesAsync(CancellationToken ct = default);

        /// <summary>
        /// Obtiene una enfermedad por su ID
        /// </summary>
        Task<EnfermedadDto?> GetEnfermedadByIdAsync(int idEnfermedad, CancellationToken ct = default);

        /// <summary>
        /// Busca enfermedades por nombre
        /// </summary>
        Task<IEnumerable<EnfermedadDto>> SearchEnfermedadesByNombreAsync(string nombre, CancellationToken ct = default);

        // =======================
        // ASOCIACIONES USUARIO-ENFERMEDAD
        // =======================

        /// <summary>
        /// Asocia una enfermedad con un usuario
        /// </summary>
        Task<UsuarioEnfermedadDto> AsociarEnfermedadAsync(AsociarEnfermedadRequest request, CancellationToken ct = default);

        /// <summary>
        /// Desasocia una enfermedad de un usuario
        /// </summary>
        Task<bool> DesasociarEnfermedadAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default);

        /// <summary>
        /// Obtiene todas las enfermedades asociadas a un usuario
        /// </summary>
        Task<IEnumerable<UsuarioEnfermedadDto>> GetEnfermedadesUsuarioAsync(int idUsuario, CancellationToken ct = default);

        /// <summary>
        /// Verifica si una enfermedad está asociada a un usuario
        /// </summary>
        Task<bool> IsEnfermedadAsociadaAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default);

        /// <summary>
        /// Actualiza la información de una enfermedad asociada
        /// </summary>
        Task<bool> ActualizarEnfermedadAsync(ActualizarEnfermedadRequest request, CancellationToken ct = default);

        /// <summary>
        /// Obtiene una asociación específica usuario-enfermedad
        /// </summary>
        Task<UsuarioEnfermedadDto?> GetUsuarioEnfermedadAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default);

        /// <summary>
        /// Elimina una asociación específica por su ID
        /// </summary>
        Task<bool> EliminarAsociacionAsync(int idUsuarioEnfermedad, CancellationToken ct = default);
    }
}
