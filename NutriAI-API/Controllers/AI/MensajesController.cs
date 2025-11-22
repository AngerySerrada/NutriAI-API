using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.AI;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.AI
{
    [Route("api/mensajes")]
    [ApiController]
    [Authorize]
    public class MensajesController : ControllerBase
    {
        private readonly IConversacionService _conversacionService;

        public MensajesController(IConversacionService conversacionService)
        {
            _conversacionService = conversacionService;
        }

        /// <summary>
        /// Guardar un nuevo mensaje en una conversación
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GuardarMensaje([FromBody] GuardarMensajeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mensaje = await _conversacionService.GuardarMensajeAsync(request, CancellationToken.None);
                
                if (mensaje == null)
                {
                    return BadRequest(new { message = "No se pudo guardar el mensaje. Verifique que la conversación existe." });
                }

                return CreatedAtAction(nameof(ObtenerMensajesPorConversacion), 
                    new { idConversacion = mensaje.IdConversacion }, mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener todos los mensajes de una conversación
        /// </summary>
        [HttpGet("conversacion/{idConversacion}")]
        public async Task<IActionResult> ObtenerMensajesPorConversacion(int idConversacion)
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
        /// Guardar múltiples mensajes en una conversación (útil para conversaciones con IA)
        /// </summary>
        [HttpPost("batch")]
        public async Task<IActionResult> GuardarMensajesBatch([FromBody] List<GuardarMensajeRequest> requests)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (requests == null || !requests.Any())
                {
                    return BadRequest(new { message = "Debe proporcionar al menos un mensaje" });
                }

                var mensajesGuardados = new List<MensajeConversacionDto>();

                foreach (var request in requests)
                {
                    var mensaje = await _conversacionService.GuardarMensajeAsync(request, CancellationToken.None);
                    
                    if (mensaje != null)
                    {
                        mensajesGuardados.Add(mensaje);
                    }
                }

                if (!mensajesGuardados.Any())
                {
                    return BadRequest(new { message = "No se pudo guardar ningún mensaje. Verifique que las conversaciones existen." });
                }

                return Ok(new 
                { 
                    message = $"Se guardaron {mensajesGuardados.Count} de {requests.Count} mensajes correctamente",
                    mensajes = mensajesGuardados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}