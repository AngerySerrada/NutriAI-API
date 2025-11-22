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
        // CRUD INGREDIENTES
        // =======================

        /// <summary>
        /// Crea un nuevo ingrediente
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearIngrediente([FromBody] CrearIngredienteRequest request)
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
                var resultado = await _ingredienteService.CrearIngredienteAsync(request, CancellationToken.None);
                return CreatedAtAction(nameof(GetIngredienteById), new { id = resultado.IdIngrediente }, resultado);
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
        /// Actualiza un ingrediente existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarIngrediente(int id, [FromBody] ActualizarIngredienteRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (id != request.IdIngrediente)
            {
                return BadRequest(new { message = "El ID del ingrediente en la URL no coincide con el ID en el cuerpo de la solicitud." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos de entrada inválidos.", errors = ModelState });
            }

            try
            {
                var resultado = await _ingredienteService.ActualizarIngredienteAsync(request, CancellationToken.None);
                
                if (resultado == null)
                {
                    return NotFound(new { message = "Ingrediente no encontrado." });
                }

                return Ok(new { message = "Ingrediente actualizado exitosamente.", data = resultado });
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
        /// Elimina un ingrediente por su ID
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarIngrediente(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID del ingrediente debe ser mayor a 0." });
            }

            try
            {
                var resultado = await _ingredienteService.EliminarIngredienteAsync(id, CancellationToken.None);
                
                if (resultado)
                {
                    return Ok(new { message = "Ingrediente eliminado exitosamente." });
                }
                else
                {
                    return NotFound(new { message = "Ingrediente no encontrado." });
                }
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

        // =======================
        // ENDPOINTS PARA GRÁFICOS
        // =======================

        /// <summary>
        /// Obtiene estadísticas nutricionales totales de un usuario para gráficos
        /// </summary>
        [HttpGet("estadisticas/{idUsuario:int}")]
        public async Task<IActionResult> GetEstadisticasUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var estadisticas = await _ingredienteService.GetEstadisticasUsuarioAsync(idUsuario, CancellationToken.None);
                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene distribución de macronutrientes por categoría para gráficos de barras/líneas
        /// </summary>
        [HttpGet("graficos/macronutrientes-categoria/{idUsuario:int}")]
        public async Task<IActionResult> GetMacronutrientesPorCategoria(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var datos = await _ingredienteService.GetMacronutrientesPorCategoriaAsync(idUsuario, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene los ingredientes más consumidos por un usuario para gráficos de ranking
        /// </summary>
        [HttpGet("graficos/mas-consumidos/{idUsuario:int}")]
        public async Task<IActionResult> GetIngredientesMasConsumidos(int idUsuario, [FromQuery] int top = 10)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (top <= 0 || top > 50)
            {
                return BadRequest(new { message = "El parámetro 'top' debe estar entre 1 y 50." });
            }

            try
            {
                var datos = await _ingredienteService.GetIngredientesMasConsumidosAsync(idUsuario, top, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene calorías por categoría para gráficos de torta/dona
        /// </summary>
        [HttpGet("graficos/calorias-categoria/{idUsuario:int}")]
        public async Task<IActionResult> GetCaloriasPorCategoria(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var datos = await _ingredienteService.GetCaloriasPorCategoriaAsync(idUsuario, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el balance nutricional con porcentajes de macronutrientes para gráficos de torta
        /// </summary>
        [HttpGet("graficos/balance-nutricional/{idUsuario:int}")]
        public async Task<IActionResult> GetBalanceNutricional(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var datos = await _ingredienteService.GetBalanceNutricionalAsync(idUsuario, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene el historial de ingredientes por fecha para gráficos de línea temporal
        /// </summary>
        [HttpGet("graficos/historial/{idUsuario:int}")]
        public async Task<IActionResult> GetHistorialIngredientes(int idUsuario, [FromQuery] int dias = 30)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            if (dias <= 0 || dias > 365)
            {
                return BadRequest(new { message = "El parámetro 'dias' debe estar entre 1 y 365." });
            }

            try
            {
                var datos = await _ingredienteService.GetHistorialIngredientesAsync(idUsuario, dias, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene la variedad de ingredientes por categoría para gráficos de distribución
        /// </summary>
        [HttpGet("graficos/variedad-categorias/{idUsuario:int}")]
        public async Task<IActionResult> GetVariedadCategorias(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var datos = await _ingredienteService.GetVariedadCategoriasAsync(idUsuario, CancellationToken.None);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene un resumen completo de todas las estadísticas para dashboard
        /// </summary>
        [HttpGet("graficos/dashboard/{idUsuario:int}")]
        public async Task<IActionResult> GetDashboardCompleto(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { message = "El ID del usuario debe ser mayor a 0." });
            }

            try
            {
                var estadisticas = await _ingredienteService.GetEstadisticasUsuarioAsync(idUsuario, CancellationToken.None);
                var balanceNutricional = await _ingredienteService.GetBalanceNutricionalAsync(idUsuario, CancellationToken.None);
                var caloriasPorCategoria = await _ingredienteService.GetCaloriasPorCategoriaAsync(idUsuario, CancellationToken.None);
                var masConsumidos = await _ingredienteService.GetIngredientesMasConsumidosAsync(idUsuario, 5, CancellationToken.None);
                var variedad = await _ingredienteService.GetVariedadCategoriasAsync(idUsuario, CancellationToken.None);

                return Ok(new
                {
                    estadisticasGenerales = estadisticas,
                    balanceNutricional,
                    caloriasPorCategoria,
                    ingredientesMasConsumidos = masConsumidos,
                    variedadCategorias = variedad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}