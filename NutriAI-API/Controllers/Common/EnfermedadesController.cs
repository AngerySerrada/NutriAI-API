using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace NutriAI_API.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnfermedadesController : ControllerBase
    {
        private readonly IEnfermedadService _enfermedadService;

        public EnfermedadesController(IEnfermedadService enfermedadService)
        {
            _enfermedadService = enfermedadService;
        }

        // =======================
        // ENFERMEDADES
        // =======================

        /// <summary>
        /// Obtiene todas las enfermedades disponibles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEnfermedades()
        {
            try
            {
                var enfermedades = await _enfermedadService.GetEnfermedadesAsync();
                return Ok(enfermedades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene una enfermedad por su ID
        /// </summary>
        [HttpGet("{idEnfermedad:int}")]
        public async Task<IActionResult> GetEnfermedadById(int idEnfermedad)
        {
            if (idEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la enfermedad debe ser mayor a 0." });
            }

            try
            {
                var enfermedad = await _enfermedadService.GetEnfermedadByIdAsync(idEnfermedad);
                
                if (enfermedad == null)
                {
                    return NotFound(new { message = "Enfermedad no encontrada." });
                }

                return Ok(enfermedad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Busca enfermedades por nombre
        /// </summary>
        [HttpGet("buscar")]
        public async Task<IActionResult> SearchEnfermedadesByNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { message = "El nombre de búsqueda no puede estar vacío." });
            }

            try
            {
                var enfermedades = await _enfermedadService.SearchEnfermedadesByNombreAsync(nombre);
                return Ok(enfermedades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // =======================
        // ASOCIACIONES USUARIO-ENFERMEDAD
        // =======================

        /// <summary>
        /// Asocia una enfermedad con un usuario
        /// </summary>
        [HttpPost("asociar")]
        public async Task<IActionResult> AsociarEnfermedad([FromBody] AsociarEnfermedadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.IdUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (request.IdEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la enfermedad debe ser mayor a 0." });
            }

            try
            {
                var asociacion = await _enfermedadService.AsociarEnfermedadAsync(request);
                return Ok(asociacion);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Desasocia una enfermedad de un usuario
        /// </summary>
        [HttpDelete("desasociar")]
        public async Task<IActionResult> DesasociarEnfermedad([FromQuery] int idUsuario, [FromQuery] int idEnfermedad)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la enfermedad debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _enfermedadService.DesasociarEnfermedadAsync(idUsuario, idEnfermedad);
                
                if (resultado)
                {
                    return Ok(new { message = "Enfermedad desasociada exitosamente." });
                }
                else
                {
                    return NotFound(new { message = "No se encontró la asociación especificada." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene todas las enfermedades asociadas a un usuario
        /// </summary>
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<IActionResult> GetEnfermedadesUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var enfermedades = await _enfermedadService.GetEnfermedadesUsuarioAsync(idUsuario);
                return Ok(enfermedades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Verifica si una enfermedad está asociada a un usuario
        /// </summary>
        [HttpGet("verificar-asociacion")]
        public async Task<IActionResult> IsEnfermedadAsociada([FromQuery] int idUsuario, [FromQuery] int idEnfermedad)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la enfermedad debe ser mayor a 0." });
            }

            try
            {
                var estaAsociada = await _enfermedadService.IsEnfermedadAsociadaAsync(idUsuario, idEnfermedad);
                return Ok(new { estaAsociada });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualiza la información de una enfermedad asociada
        /// </summary>
        [HttpPut("actualizar-asociacion")]
        public async Task<IActionResult> ActualizarEnfermedad([FromBody] ActualizarEnfermedadRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.IdUsuarioEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la asociación debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _enfermedadService.ActualizarEnfermedadAsync(request);
                
                if (resultado)
                {
                    return Ok(new { message = "Información de enfermedad actualizada exitosamente." });
                }
                else
                {
                    return NotFound(new { message = "No se encontró la asociación especificada." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene una asociación específica usuario-enfermedad
        /// </summary>
        [HttpGet("asociacion")]
        public async Task<IActionResult> GetUsuarioEnfermedad([FromQuery] int idUsuario, [FromQuery] int idEnfermedad)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la enfermedad debe ser mayor a 0." });
            }

            try
            {
                var asociacion = await _enfermedadService.GetUsuarioEnfermedadAsync(idUsuario, idEnfermedad);
                
                if (asociacion == null)
                {
                    return NotFound(new { message = "No se encontró la asociación entre el usuario y la enfermedad especificados." });
                }

                return Ok(asociacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Elimina una asociación específica por su ID
        /// </summary>
        [HttpDelete("eliminar-asociacion/{idUsuarioEnfermedad:int}")]
        public async Task<IActionResult> EliminarAsociacion(int idUsuarioEnfermedad)
        {
            if (idUsuarioEnfermedad <= 0)
            {
                return BadRequest(new { message = "El ID de la asociación debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _enfermedadService.EliminarAsociacionAsync(idUsuarioEnfermedad);
                
                if (resultado)
                {
                    return Ok(new { message = "Asociación eliminada exitosamente." });
                }
                else
                {
                    return NotFound(new { message = "No se encontró la asociación especificada." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
