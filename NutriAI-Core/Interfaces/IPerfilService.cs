using NutriAI_Core.DTOs.Common;

namespace NutriAI_Core.Interfaces
{
    public interface IPerfilService
    {
        Task<IEnumerable<PerfilDto>> GetAllPerfilesAsync(CancellationToken cancellationToken);
        Task<PerfilDto?> GetPerfilByIdAsync(int idPerfil, CancellationToken cancellationToken);
        Task<IEnumerable<UsuarioDto>> GetUsuariosByPerfilAsync(int idPerfil, CancellationToken cancellationToken);
        Task<bool> AssignPerfilToUsuarioAsync(int idUsuario, int idPerfil, CancellationToken cancellationToken);
        Task<bool> RemovePerfilFromUsuarioAsync(int idUsuario, int idPerfil, CancellationToken cancellationToken);
        Task<IEnumerable<UsuarioPerfilDto>> GetUsuarioPerfilRelationsAsync(CancellationToken cancellationToken);
    }
}