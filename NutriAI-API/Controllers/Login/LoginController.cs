using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.Auth;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.Login
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _service;
        
        public LoginController(IAuthService service)
        {
            _service = service;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Login y contraseña son requeridos." });
            }
            
            try
            {
                var tokenResponse = await _service.LoginAsync(request, CancellationToken.None);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(request.Login) || 
                string.IsNullOrWhiteSpace(request.Email) || 
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Login, email y contraseña son requeridos." });
            }

            // Validaciones adicionales para los campos del usuario
            if (request.Edad <= 0)
            {
                return BadRequest(new { message = "La edad debe ser mayor a 0." });
            }

            if (request.IdComuna <= 0)
            {
                return BadRequest(new { message = "Debe seleccionar una comuna válida." });
            }

            if (request.IdNivelActividad <= 0)
            {
                return BadRequest(new { message = "Debe seleccionar un nivel de actividad válido." });
            }

            if (request.IdSexo <= 0)
            {
                return BadRequest(new { message = "Debe seleccionar un sexo válido." });
            }

            if (request.Altura <= 0)
            {
                return BadRequest(new { message = "La altura debe ser mayor a 0." });
            }

            if (request.Peso <= 0)
            {
                return BadRequest(new { message = "El peso debe ser mayor a 0." });
            }

            if (request.FechaNacimiento == default(DateOnly) || request.FechaNacimiento > DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest(new { message = "La fecha de nacimiento debe ser válida y no puede ser futura." });
            }

            try
            {
                var userId = await _service.RegisterAsync(request, CancellationToken.None);
                return Ok(new { userId, message = "Usuario registrado exitosamente." });
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            try
            {
                var tokenResponse = await _service.RefreshPairAsync(request.AccessToken, request.RefreshToken, CancellationToken.None);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(request.EmailOrLogin))
            {
                return BadRequest(new { message = "Email o nombre de usuario es requerido." });
            }

            try
            {
                var resetToken = await _service.ForgotPasswordAsync(request.EmailOrLogin, CancellationToken.None);
                
                // En producción, aquí se enviaría un email y se retornaría un mensaje genérico
                // Por ahora retornamos el token para propósitos de desarrollo/testing
                return Ok(new { 
                    message = "Si el usuario existe, se ha enviado un enlace de restablecimiento.",
                    resetToken // Solo para desarrollo - quitar en producción
                });
            }
            catch (InvalidOperationException ex)
            {
                // Por seguridad, siempre retornamos el mismo mensaje para no revelar si el usuario existe
                return Ok(new { message = "Si el usuario existe, se ha enviado un enlace de restablecimiento." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest(new { message = "Token de restablecimiento es requerido." });
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { message = "Nueva contraseña es requerida." });
            }

            // Validación básica de contraseña
            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres." });
            }

            try
            {
                var result = await _service.ResetPasswordAsync(request.Token, request.NewPassword, CancellationToken.None);
                
                if (result)
                {
                    return Ok(new { message = "Contraseña restablecida exitosamente." });
                }
                else
                {
                    return BadRequest(new { message = "Token de restablecimiento inválido o expirado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "El modelo de solicitud no puede ser nulo." });
            }

            if (string.IsNullOrWhiteSpace(request.Login) || 
                string.IsNullOrWhiteSpace(request.CurrentPassword) || 
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { message = "Todos los campos son requeridos." });
            }

            // Validación básica de contraseña
            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new { message = "La nueva contraseña debe tener al menos 6 caracteres." });
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return BadRequest(new { message = "La nueva contraseña debe ser diferente a la actual." });
            }

            try
            {
                var result = await _service.ChangePasswordAsync(request.Login, request.CurrentPassword, request.NewPassword, CancellationToken.None);
                
                if (result)
                {
                    return Ok(new { message = "Contraseña cambiada exitosamente." });
                }
                else
                {
                    return BadRequest(new { message = "Usuario no encontrado o contraseña actual incorrecta." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
