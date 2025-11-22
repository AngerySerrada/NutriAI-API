using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.AI;
using NutriAI_Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NutriAI_API.Controllers.AI
{
    [Route("api/conversaciones")]
    [ApiController]
    [Authorize]
    public class ConversacionesController : ControllerBase
    {
        private readonly IConversacionService _conversacionService;

        public ConversacionesController(IConversacionService conversacionService)
        {
            _conversacionService = conversacionService;
        }

        /// <summary>
        /// Crear una nueva conversación
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearConversacion([FromBody] CrearConversacionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var conversacion = await _conversacionService.CrearConversacionAsync(request, CancellationToken.None);
                
                if (conversacion == null)
                {
                    return BadRequest(new { message = "No se pudo crear la conversación. Verifique que el usuario existe y esté activo." });
                }

                return CreatedAtAction(nameof(ObtenerConversacionPorId), 
                    new { id = conversacion.IdConversacion }, conversacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener conversación por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerConversacionPorId(int id)
        {
            try
            {
                var conversacion = await _conversacionService.ObtenerConversacionPorIdAsync(id, CancellationToken.None);
                
                if (conversacion == null)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(conversacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener todas las conversaciones de un usuario
        /// </summary>
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ObtenerConversacionesPorUsuario(int idUsuario)
        {
            try
            {
                var conversaciones = await _conversacionService.ObtenerConversacionesPorUsuarioAsync(idUsuario, CancellationToken.None);
                return Ok(conversaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener historial completo de conversaciones de un usuario
        /// </summary>
        [HttpGet("usuario/{idUsuario}/historial")]
        public async Task<IActionResult> ObtenerHistorialConversaciones(int idUsuario)
        {
            try
            {
                var historial = await _conversacionService.ObtenerHistorialConversacionesAsync(idUsuario, CancellationToken.None);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener conversación con información para la vista (ViewModel)
        /// </summary>
        [HttpGet("{id}/usuario/{idUsuario}/view")]
        public async Task<IActionResult> ObtenerConversacionViewModel(int id, int idUsuario)
        {
            try
            {
                var viewModel = await _conversacionService.ObtenerConversacionViewModelAsync(id, idUsuario, CancellationToken.None);
                
                if (viewModel == null)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualizar el título de una conversación
        /// </summary>
        [HttpPut("{id}/titulo")]
        public async Task<IActionResult> ActualizarTituloConversacion(int id, [FromBody] ActualizarTituloRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = await _conversacionService.ActualizarTituloConversacionAsync(id, request.NuevoTitulo, CancellationToken.None);
                
                if (!resultado)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(new { message = "Título de conversación actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Desactivar una conversación
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DesactivarConversacion(int id)
        {
            try
            {
                var resultado = await _conversacionService.DesactivarConversacionAsync(id, CancellationToken.None);
                
                if (!resultado)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(new { message = "Conversación desactivada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Activar una conversación
        /// </summary>
        [HttpPut("{id}/activar")]
        public async Task<IActionResult> ActivarConversacion(int id)
        {
            try
            {
                var resultado = await _conversacionService.ActivarConversacionAsync(id, CancellationToken.None);
                
                if (!resultado)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(new { message = "Conversación activada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }

    /// <summary>
    /// DTO para actualizar el título de una conversación
    /// </summary>
    public class ActualizarTituloRequest
    {
        [Required(ErrorMessage = "El nuevo título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string NuevoTitulo { get; set; } = string.Empty;
    }
}