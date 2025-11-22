using NutriAI_Core.DTOs.PDF;

namespace NutriAI_Core.Interfaces
{
    public interface IRecetaPDFService
    {
        // Gestión de PDFs
        Task<RecetaPDFDto?> GuardarRecetaPDFAsync(GuardarRecetaPDFRequest request, CancellationToken cancellationToken);
        Task<RecetaPDFDetalleDto?> ObtenerRecetaPDFPorIdAsync(int idRecetaPDF, CancellationToken cancellationToken);
        Task<IEnumerable<RecetaPDFDto>> ObtenerPDFsPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken);
        Task<bool> EliminarRecetaPDFAsync(int idRecetaPDF, CancellationToken cancellationToken);
        Task<byte[]?> DescargarRecetaPDFAsync(int idRecetaPDF, CancellationToken cancellationToken);
        
        // ViewModels
        Task<MisPDFsViewModel> ObtenerMisPDFsViewModelAsync(int idUsuario, CancellationToken cancellationToken);
    }
}
