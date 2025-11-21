using NutriAI_Core.DTOs.Common;

namespace NutriAI_Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync(CancellationToken cancellationToken);
        Task<UsuarioDto?> GetUsuarioByIdAsync(int idUsuario, CancellationToken cancellationToken);
        Task<UsuarioDto?> GetUsuarioByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<UsuarioDto?> GetUsuarioByEmailAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<PerfilDto>> GetUsuarioPerfilesAsync(int idUsuario, CancellationToken cancellationToken);
        Task<IEnumerable<EnfermedadDto>> GetUsuarioEnfermedadesAsync(int idUsuario, CancellationToken cancellationToken);
        Task<UsuarioDto?> GetUsuarioWithPerfilesAsync(int idUsuario, CancellationToken cancellationToken);
        Task<bool> UpdateUsuarioAsync(int idUsuario, UsuarioDto usuarioDto, CancellationToken cancellationToken);
        Task<bool> DeactivateUsuarioAsync(int idUsuario, CancellationToken cancellationToken);
    }
}