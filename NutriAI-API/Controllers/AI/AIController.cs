using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.AI;
using NutriAI_Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NutriAI_API.Controllers.AI
{
    [Route("api/ai")]
    [ApiController]
    [Authorize]
    public class AIController : ControllerBase
    {
        private readonly IConversacionService _conversacionService;

        public AIController(IConversacionService conversacionService)
        {
            _conversacionService = conversacionService;
        }

        #region Conversaciones

        /// <summary>
        /// Crear una nueva conversación con un mensaje inicial
        /// </summary>
        [HttpPost("conversaciones/nueva")]
        public async Task<IActionResult> CrearConversacionConMensaje([FromBody] CrearConversacionConMensajeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Crear la conversación
                var conversacionRequest = new CrearConversacionRequest
                {
                    IdUsuario = request.IdUsuario,
                    Titulo = request.Titulo
                };

                var conversacion = await _conversacionService.CrearConversacionAsync(conversacionRequest, CancellationToken.None);
                
                if (conversacion == null)
                {
                    return BadRequest(new { message = "No se pudo crear la conversación. Verifique que el usuario existe y esté activo." });
                }

                // Agregar el mensaje inicial si se proporciona
                if (!string.IsNullOrWhiteSpace(request.MensajeInicial))
                {
                    var mensajeRequest = new GuardarMensajeRequest
                    {
                        IdConversacion = conversacion.IdConversacion,
                        Rol = "user",
                        Contenido = request.MensajeInicial
                    };

                    var mensaje = await _conversacionService.GuardarMensajeAsync(mensajeRequest, CancellationToken.None);
                    
                    if (mensaje != null)
                    {
                        // Obtener la conversación actualizada con el mensaje
                        var conversacionCompleta = await _conversacionService.ObtenerConversacionPorIdAsync(
                            conversacion.IdConversacion, CancellationToken.None);
                        
                        return CreatedAtAction(nameof(ObtenerConversacion), 
                            new { id = conversacion.IdConversacion }, conversacionCompleta);
                    }
                }

                return CreatedAtAction(nameof(ObtenerConversacion), 
                    new { id = conversacion.IdConversacion }, conversacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener conversación completa por ID
        /// </summary>
        [HttpGet("conversaciones/{id}")]
        public async Task<IActionResult> ObtenerConversacion(int id)
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
        /// Obtener historial de conversaciones de un usuario
        /// </summary>
        [HttpGet("usuarios/{idUsuario}/conversaciones")]
        public async Task<IActionResult> ObtenerConversacionesUsuario(int idUsuario)
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

        #endregion

        #region Mensajes

        /// <summary>
        /// Agregar mensaje a una conversación existente
        /// </summary>
        [HttpPost("conversaciones/{idConversacion}/mensajes")]
        public async Task<IActionResult> AgregarMensaje(int idConversacion, [FromBody] AgregarMensajeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mensajeRequest = new GuardarMensajeRequest
                {
                    IdConversacion = idConversacion,
                    Rol = request.Rol,
                    Contenido = request.Contenido,
                    TokensUtilizados = request.TokensUtilizados,
                    ContextoIncluido = request.ContextoIncluido
                };

                var mensaje = await _conversacionService.GuardarMensajeAsync(mensajeRequest, CancellationToken.None);
                
                if (mensaje == null)
                {
                    return BadRequest(new { message = "No se pudo guardar el mensaje. Verifique que la conversación existe." });
                }

                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener todos los mensajes de una conversación
        /// </summary>
        [HttpGet("conversaciones/{idConversacion}/mensajes")]
        public async Task<IActionResult> ObtenerMensajesConversacion(int idConversacion)
        {
            try
            {
                var mensajes = await _conversacionService.ObtenerMensajesPorConversacionAsync(idConversacion, CancellationToken.None);
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Intercambio de mensajes con IA (usuario envía mensaje, se guarda respuesta de IA)
        /// </summary>
        [HttpPost("conversaciones/{idConversacion}/intercambio")]
        public async Task<IActionResult> IntercambioMensajes(int idConversacion, [FromBody] IntercambioMensajesRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mensajesGuardados = new List<MensajeConversacionDto>();

                // Guardar mensaje del usuario
                var mensajeUsuarioRequest = new GuardarMensajeRequest
                {
                    IdConversacion = idConversacion,
                    Rol = "user",
                    Contenido = request.MensajeUsuario
                };

                var mensajeUsuario = await _conversacionService.GuardarMensajeAsync(mensajeUsuarioRequest, CancellationToken.None);
                
                if (mensajeUsuario != null)
                {
                    mensajesGuardados.Add(mensajeUsuario);
                }

                // Guardar respuesta de la IA si se proporciona
                if (!string.IsNullOrWhiteSpace(request.RespuestaIA))
                {
                    var mensajeIARequest = new GuardarMensajeRequest
                    {
                        IdConversacion = idConversacion,
                        Rol = "assistant",
                        Contenido = request.RespuestaIA,
                        TokensUtilizados = request.TokensUtilizados,
                        ContextoIncluido = request.ContextoIncluido
                    };

                    var mensajeIA = await _conversacionService.GuardarMensajeAsync(mensajeIARequest, CancellationToken.None);
                    
                    if (mensajeIA != null)
                    {
                        mensajesGuardados.Add(mensajeIA);
                    }
                }

                if (!mensajesGuardados.Any())
                {
                    return BadRequest(new { message = "No se pudo guardar ningún mensaje. Verifique que la conversación existe." });
                }

                return Ok(new 
                { 
                    message = $"Se guardaron {mensajesGuardados.Count} mensajes correctamente",
                    mensajes = mensajesGuardados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        #endregion

        #region Administración de Conversaciones

        /// <summary>
        /// Actualizar título de conversación
        /// </summary>
        [HttpPut("conversaciones/{id}/titulo")]
        public async Task<IActionResult> ActualizarTitulo(int id, [FromBody] ActualizarTituloRequest request)
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

                return Ok(new { message = "Título actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Archivar/Desactivar conversación
        /// </summary>
        [HttpDelete("conversaciones/{id}")]
        public async Task<IActionResult> ArchivarConversacion(int id)
        {
            try
            {
                var resultado = await _conversacionService.DesactivarConversacionAsync(id, CancellationToken.None);
                
                if (!resultado)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(new { message = "Conversación archivada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Restaurar conversación archivada
        /// </summary>
        [HttpPut("conversaciones/{id}/restaurar")]
        public async Task<IActionResult> RestaurarConversacion(int id)
        {
            try
            {
                var resultado = await _conversacionService.ActivarConversacionAsync(id, CancellationToken.None);
                
                if (!resultado)
                {
                    return NotFound(new { message = "Conversación no encontrada" });
                }

                return Ok(new { message = "Conversación restaurada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        #endregion
    }

    #region Request DTOs

    public class CrearConversacionConMensajeRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string? Titulo { get; set; }

        public string? MensajeInicial { get; set; }
    }

    public class AgregarMensajeRequest
    {
        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(user|assistant|system)$", ErrorMessage = "El rol debe ser 'user', 'assistant' o 'system'")]
        public string Rol { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contenido es requerido")]
        public string Contenido { get; set; } = string.Empty;

        public int? TokensUtilizados { get; set; }
        public string? ContextoIncluido { get; set; }
    }

    public class IntercambioMensajesRequest
    {
        [Required(ErrorMessage = "El mensaje del usuario es requerido")]
        public string MensajeUsuario { get; set; } = string.Empty;

        public string? RespuestaIA { get; set; }
        public int? TokensUtilizados { get; set; }
        public string? ContextoIncluido { get; set; }
    }

    #endregion
}