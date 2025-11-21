using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.Common
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync(CancellationToken.None);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id, CancellationToken.None);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUsuarioByUsername(string username)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByUsernameAsync(username, CancellationToken.None);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsuarioByEmail(string email)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByEmailAsync(email, CancellationToken.None);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}/perfiles")]
        public async Task<IActionResult> GetUsuarioPerfiles(int id)
        {
            try
            {
                var perfiles = await _usuarioService.GetUsuarioPerfilesAsync(id, CancellationToken.None);
                return Ok(perfiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("{id}/with-perfiles")]
        public async Task<IActionResult> GetUsuarioWithPerfiles(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioWithPerfilesAsync(id, CancellationToken.None);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] NutriAI_Core.DTOs.Common.UsuarioDto usuarioDto)
        {
            try
            {
                if (id != usuarioDto.IdUsuario)
                {
                    return BadRequest(new { message = "El ID del usuario no coincide" });
                }

                var result = await _usuarioService.UpdateUsuarioAsync(id, usuarioDto, CancellationToken.None);
                if (!result)
                {
                    return NotFound(new { message = "Usuario no encontrado o inactivo" });
                }

                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUsuario(int id)
        {
            try
            {
                var result = await _usuarioService.DeactivateUsuarioAsync(id, CancellationToken.None);
                if (!result)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(new { message = "Usuario desactivado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}