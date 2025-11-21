using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NutriAI_Core.DTOs.Auth;
using NutriAI_Core.DTOs.Options;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Services.Services.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtOptions _jwt;
        private readonly EmailOptions? _emailOptions;

        public AuthService(AppDbContext ctx, IOptions<JwtOptions> jwt, IOptions<EmailOptions>? emailOptions = null)
        {
            _ctx = ctx;
            _jwt = jwt.Value;
            _emailOptions = emailOptions?.Value;
        }

        // =======================
        // LOGIN
        // =======================

        public async Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            // Normalizamos el login de entrada
            var login = request.Login?
                .Replace(".", string.Empty)   // quita puntos
                .Replace(" ", string.Empty)   // quita espacios
                .Trim();

            var user = await _ctx.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.Replace(".", "").Replace(" ", "") == login, ct);

            if (user is null || !user.Activo)
                throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

            // Verificar la contraseña usando salt y hash
            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

            var version = ComputeVersion(user);

            var access = GenerateAccessToken(user.Username, version);
            var refresh = GenerateRefreshToken(user.Username, version);

            return new TokenResponse
            {
                AccessToken = access.Token,
                ExpiresAtUtc = access.ExpiresAtUtc,
                RefreshToken = refresh.Token,
                RefreshExpiresAtUtc = refresh.ExpiresAtUtc
            };
        }

        // =======================
        // REFRESH TOKEN
        // =======================

        public async Task<TokenResponse> RefreshPairAsync(string accessToken, string refreshToken, CancellationToken ct)
        {
            try
            {
                // Validar el refresh token
                var principal = ValidateJwt(refreshToken, _jwt.RefreshKey, false);
                var username = GetSubject(principal);
                var tokenType = GetClaim(principal, "typ");
                var version = GetClaim(principal, "ver");

                if (string.IsNullOrWhiteSpace(username) || tokenType != "refresh")
                    throw new UnauthorizedAccessException("Token de actualización inválido.");

                // Verificar que el usuario existe y está activo
                var user = await _ctx.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == username, ct);

                if (user is null || !user.Activo)
                    throw new UnauthorizedAccessException("Usuario no encontrado o inactivo.");

                // Verificar que la versión del token coincide con la actual del usuario
                var currentVersion = ComputeVersion(user);
                if (version != currentVersion)
                    throw new UnauthorizedAccessException("Token de actualización obsoleto.");

                // Generar nuevos tokens
                var newAccess = GenerateAccessToken(user.Username, currentVersion);
                var newRefresh = GenerateRefreshToken(user.Username, currentVersion);

                return new TokenResponse
                {
                    AccessToken = newAccess.Token,
                    ExpiresAtUtc = newAccess.ExpiresAtUtc,
                    RefreshToken = newRefresh.Token,
                    RefreshExpiresAtUtc = newRefresh.ExpiresAtUtc
                };
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Token de actualización inválido.");
            }
        }

        // =======================
        // REGISTER
        // =======================

        public async Task<int> RegisterAsync(RegisterRequest request, CancellationToken ct)
        {
            // Verificar que el username no existe
            var existingUser = await _ctx.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == request.Login || u.Correo == request.Email, ct);

            if (existingUser != null)
                throw new InvalidOperationException("El usuario o correo ya existe.");

            // Validar que las referencias foráneas existen
            var comunaExists = await _ctx.Comunas
                .AsNoTracking()
                .AnyAsync(c => c.IdComuna == request.IdComuna && c.Activo, ct);
            
            if (!comunaExists)
                throw new InvalidOperationException("La comuna seleccionada no es válida.");

            var nivelActividadExists = await _ctx.NivelesActividads
                .AsNoTracking()
                .AnyAsync(n => n.IdNivelActividad == request.IdNivelActividad && n.Activo, ct);
            
            if (!nivelActividadExists)
                throw new InvalidOperationException("El nivel de actividad seleccionado no es válido.");

            var sexoExists = await _ctx.Sexos
                .AsNoTracking()
                .AnyAsync(s => s.IdSexo == request.IdSexo, ct);
            
            if (!sexoExists)
                throw new InvalidOperationException("El sexo seleccionado no es válido.");

            // Crear hash y salt para la contraseña
            var (passwordHash, passwordSalt) = HashPassword(request.Password);

            var user = new Usuario
            {
                Username = request.Login,
                Correo = request.Email,
                Nombre = request.NombreUsuario ?? request.Login,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FechaCreacion = DateTime.UtcNow,
                Activo = true,
                
                // Campos adicionales del usuario
                Edad = request.Edad,
                IdComuna = request.IdComuna,
                IdNivelActividad = request.IdNivelActividad,
                IdSexo = request.IdSexo,
                FechaNacimiento = request.FechaNacimiento,
                Altura = request.Altura,
                Peso = request.Peso
            };

            _ctx.Usuarios.Add(user);
            await _ctx.SaveChangesAsync(ct);

            return user.IdUsuario;
        }

        // =======================
        // FORGOT PASSWORD
        // =======================

        public async Task<string> ForgotPasswordAsync(string emailOrLogin, CancellationToken ct)
        {
            var user = await _ctx.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == emailOrLogin || u.Username == emailOrLogin, ct);

            if (user is null || !user.Activo)
                throw new InvalidOperationException("Usuario no encontrado.");

            // Generar token de reset (válido por 1 hora)
            var resetToken = GeneratePasswordResetToken(user.Username);
            
            // En una implementación real, aquí se enviaría el email
            // Por ahora retornamos el token directamente
            return resetToken;
        }

        // =======================
        // RESET PASSWORD
        // =======================

        public async Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken ct)
        {
            try
            {
                var principal = ValidateJwt(token, _jwt.Key, true);
                var username = GetSubject(principal);
                var tokenType = GetClaim(principal, "typ");

                if (string.IsNullOrWhiteSpace(username) || tokenType != "reset")
                    return false;

                var user = await _ctx.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == username, ct);

                if (user is null || !user.Activo)
                    return false;

                // Actualizar la contraseña
                var (passwordHash, passwordSalt) = HashPassword(newPassword);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.FechaActualizacion = DateTime.UtcNow;

                await _ctx.SaveChangesAsync(ct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // =======================
        // CHANGE PASSWORD
        // =======================

        public async Task<bool> ChangePasswordAsync(string login, string currentPassword, string newPassword, CancellationToken ct)
        {
            var user = await _ctx.Usuarios
                .FirstOrDefaultAsync(u => u.Username == login, ct);

            if (user is null || !user.Activo)
                return false;

            // Verificar la contraseña actual
            if (!VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
                return false;

            // Actualizar a la nueva contraseña
            var (passwordHash, passwordSalt) = HashPassword(newPassword);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.FechaActualizacion = DateTime.UtcNow;

            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        // =======================
        // HELPERS
        // =======================

        private static string? GetSubject(ClaimsPrincipal principal)
        {
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        }

        private static string? GetClaim(ClaimsPrincipal principal, string type)
        {
            return principal?.FindFirst(type)?.Value;
        }

        private (string Token, DateTime ExpiresAtUtc) GenerateAccessToken(string username, string version)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username no puede ser nulo.", nameof(username));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, username),
                new(ClaimTypes.Name, username),
                new("ver", version),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        private (string Token, DateTime ExpiresAtUtc) GenerateRefreshToken(string username, string version)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username no puede ser nulo.", nameof(username));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.RefreshKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(_jwt.RefreshExpireDays);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, username),
                new("typ", "refresh"),
                new("ver", version),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        private string GeneratePasswordResetToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1); // Token válido por 1 hora

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, username),
                new("typ", "reset"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal ValidateJwt(string token, string key, bool validateLifetime)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = validateLifetime,
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero
            };

            return handler.ValidateToken(token, parameters, out _);
        }

        private static string ComputeVersion(Usuario user)
        {
            using var sha = SHA256.Create();
            var combined = $"{user.Username}:{Convert.ToBase64String(user.PasswordHash)}:{user.FechaActualizacion?.Ticks ?? user.FechaCreacion.Ticks}";
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Crea un hash seguro de la contraseña usando PBKDF2 con salt aleatorio
        /// </summary>
        private static (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            // Generar salt aleatorio de 32 bytes
            var salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Crear hash usando PBKDF2 con 100,000 iteraciones
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            return (hash, salt);
        }

        /// <summary>
        /// Verifica si la contraseña proporcionada coincide con el hash almacenado
        /// </summary>
        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash == null || storedSalt == null)
                return false;

            // Recrear el hash usando la misma salt
            using var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 100000, HashAlgorithmName.SHA256);
            var computedHash = pbkdf2.GetBytes(32);

            // Comparar los hashes de forma segura
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}
