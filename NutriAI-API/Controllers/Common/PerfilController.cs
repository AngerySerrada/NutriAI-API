using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.Common
{
    [Route("api/perfiles")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPerfiles()
        {
            try
            {
                var perfiles = await _perfilService.GetAllPerfilesAsync(CancellationToken.None);
                return Ok(perfiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfilById(int id)
        {
            try
            {
                var perfil = await _perfilService.GetPerfilByIdAsync(id, CancellationToken.None);
                if (perfil == null)
                {
                    return NotFound(new { message = "Perfil no encontrado" });
                }

                return Ok(perfil);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}/usuarios")]
        public async Task<IActionResult> GetUsuariosByPerfil(int id)
        {
            try
            {
                var usuarios = await _perfilService.GetUsuariosByPerfilAsync(id, CancellationToken.None);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost("{idPerfil}/usuarios/{idUsuario}")]
        public async Task<IActionResult> AssignPerfilToUsuario(int idPerfil, int idUsuario)
        {
            try
            {
                var result = await _perfilService.AssignPerfilToUsuarioAsync(idUsuario, idPerfil, CancellationToken.None);
                if (!result)
                {
                    return BadRequest(new { message = "No se pudo asignar el perfil al usuario. Verifique que ambos existan." });
                }

                return Ok(new { message = "Perfil asignado correctamente al usuario" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{idPerfil}/usuarios/{idUsuario}")]
        public async Task<IActionResult> RemovePerfilFromUsuario(int idPerfil, int idUsuario)
        {
            try
            {
                var result = await _perfilService.RemovePerfilFromUsuarioAsync(idUsuario, idPerfil, CancellationToken.None);
                if (!result)
                {
                    return NotFound(new { message = "Relación usuario-perfil no encontrada o ya inactiva" });
                }

                return Ok(new { message = "Perfil removido correctamente del usuario" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("usuario-perfil-relations")]
        public async Task<IActionResult> GetUsuarioPerfilRelations()
        {
            try
            {
                var relations = await _perfilService.GetUsuarioPerfilRelationsAsync(CancellationToken.None);
                return Ok(relations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}