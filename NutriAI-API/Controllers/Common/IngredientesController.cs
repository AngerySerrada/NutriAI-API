using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.Common
{
    [Route("api/ingredientes")]
    [ApiController]
    [Authorize]
    public class IngredientesController : ControllerBase
    {
        private readonly IIngredienteService _ingredienteService;

        public IngredientesController(IIngredienteService ingredienteService)
        {
            _ingredienteService = ingredienteService;
        }

        /// <summary>
        /// Obtiene todos los ingredientes disponibles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetIngredientes()
        {
            try
            {
                var ingredientes = await _ingredienteService.GetIngredientesAsync(CancellationToken.None);
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene un ingrediente por su ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetIngredienteById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID del ingrediente debe ser mayor a 0." });
            }

            try
            {
                var ingrediente = await _ingredienteService.GetIngredienteByIdAsync(id, CancellationToken.None);
                
                if (ingrediente == null)
                {
                    return NotFound(new { message = "Ingrediente no encontrado." });
                }

                return Ok(ingrediente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Busca ingredientes por nombre
        /// </summary>
        [HttpGet("buscar")]
        public async Task<IActionResult> SearchIngredientes([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest(new { message = "El parámetro 'nombre' es requerido para la búsqueda." });
            }

            if (nombre.Trim().Length < 2)
            {
                return BadRequest(new { message = "El término de búsqueda debe tener al menos 2 caracteres." });
            }

            try
            {
                var ingredientes = await _ingredienteService.SearchIngredientesByNombreAsync(nombre, CancellationToken.None);
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene ingredientes filtrados por categoría
        /// </summary>
        [HttpGet("categoria/{categoria}")]
        public async Task<IActionResult> GetIngredientesByCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
            {
                return BadRequest(new { message = "La categoría no puede estar vacía." });
            }

            try
            {
                var ingredientes = await _ingredienteService.GetIngredientesByCategoriaAsync(categoria, CancellationToken.None);
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene todas las categorías de ingredientes disponibles
        /// </summary>
        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            try
            {
                var categorias = await _ingredienteService.GetCategoriasAsync(CancellationToken.None);
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        // =======================
        // ASOCIACIONES USUARIO-INGREDIENTE
        // =======================

        /// <summary>
        /// Asocia un ingrediente con un usuario
        /// </summary>
        [HttpPost("asociar")]
        public async Task<IActionResult> AsociarIngrediente([FromBody] AsociarIngredienteRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos de entrada inválidos.", errors = ModelState });
            }

            try
            {
                var resultado = await _ingredienteService.AsociarIngredienteAsync(request, CancellationToken.None);
                return Ok(new { message = "Ingrediente asociado exitosamente.", data = resultado });
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
        /// Desasocia un ingrediente de un usuario
        /// </summary>
        [HttpDelete("desasociar")]
        public async Task<IActionResult> DesasociarIngrediente([FromQuery] int idUsuario, [FromQuery] int idIngrediente)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idIngrediente <= 0)
            {
                return BadRequest(new { message = "El ID del ingrediente debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _ingredienteService.DesasociarIngredienteAsync(idUsuario, idIngrediente, CancellationToken.None);
                
                if (resultado)
                {
                    return Ok(new { message = "Ingrediente desasociado exitosamente." });
                }
                else
                {
                    return NotFound(new { message = "No se encontró la asociación entre el usuario e ingrediente especificados." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene todos los ingredientes asociados a un usuario
        /// </summary>
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<IActionResult> GetIngredientesUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var ingredientes = await _ingredienteService.GetIngredientesUsuarioAsync(idUsuario, CancellationToken.None);
                return Ok(ingredientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Verifica si un ingrediente está asociado a un usuario
        /// </summary>
        [HttpGet("verificar-asociacion")]
        public async Task<IActionResult> VerificarAsociacion([FromQuery] int idUsuario, [FromQuery] int idIngrediente)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idIngrediente <= 0)
            {
                return BadRequest(new { message = "El ID del ingrediente debe ser mayor a 0." });
            }

            try
            {
                var estaAsociado = await _ingredienteService.IsIngredienteAsociadoAsync(idUsuario, idIngrediente, CancellationToken.None);
                return Ok(new { estaAsociado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualiza la cantidad de gramos de un ingrediente asociado
        /// </summary>
        [HttpPut("actualizar-cantidad")]
        public async Task<IActionResult> ActualizarCantidadIngrediente([FromBody] ActualizarCantidadIngredienteRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos de entrada inválidos.", errors = ModelState });
            }

            try
            {
                var resultado = await _ingredienteService.ActualizarCantidadIngredienteAsync(request, CancellationToken.None);
                
                if (resultado)
                {
                    return Ok(new { message = "Cantidad actualizada exitosamente." });
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
        /// Obtiene una asociación específica usuario-ingrediente
        /// </summary>
        [HttpGet("asociacion")]
        public async Task<IActionResult> GetUsuarioIngrediente([FromQuery] int idUsuario, [FromQuery] int idIngrediente)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (idIngrediente <= 0)
            {
                return BadRequest(new { message = "El ID del ingrediente debe ser mayor a 0." });
            }

            try
            {
                var asociacion = await _ingredienteService.GetUsuarioIngredienteAsync(idUsuario, idIngrediente, CancellationToken.None);
                
                if (asociacion == null)
                {
                    return NotFound(new { message = "No se encontró la asociación entre el usuario e ingrediente especificados." });
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
        [HttpDelete("eliminar-asociacion/{idUsuarioIngrediente:int}")]
        public async Task<IActionResult> EliminarAsociacion(int idUsuarioIngrediente)
        {
            if (idUsuarioIngrediente <= 0)
            {
                return BadRequest(new { message = "El ID de la asociación debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _ingredienteService.EliminarAsociacionAsync(idUsuarioIngrediente, CancellationToken.None);
                
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