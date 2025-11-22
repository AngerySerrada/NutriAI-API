using NutriAI_Core.DTOs.AI;

namespace NutriAI_Core.Interfaces
{
    public interface IConversacionService
    {
        // Gestión de conversaciones
        Task<ConversacionDto?> CrearConversacionAsync(CrearConversacionRequest request, CancellationToken cancellationToken);
        Task<ConversacionDetalleDto?> ObtenerConversacionPorIdAsync(int idConversacion, CancellationToken cancellationToken);
        Task<IEnumerable<ConversacionDto>> ObtenerConversacionesPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken);
        Task<bool> ActualizarTituloConversacionAsync(int idConversacion, string nuevoTitulo, CancellationToken cancellationToken);
        Task<bool> DesactivarConversacionAsync(int idConversacion, CancellationToken cancellationToken);
        Task<bool> ActivarConversacionAsync(int idConversacion, CancellationToken cancellationToken);
        
        // Gestión de mensajes
        Task<MensajeConversacionDto?> GuardarMensajeAsync(GuardarMensajeRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<MensajeConversacionDto>> ObtenerMensajesPorConversacionAsync(int idConversacion, CancellationToken cancellationToken);
        
        // ViewModels
        Task<HistorialConversacionesViewModel> ObtenerHistorialConversacionesAsync(int idUsuario, CancellationToken cancellationToken);
        Task<ConversacionViewModel?> ObtenerConversacionViewModelAsync(int idConversacion, int idUsuario, CancellationToken cancellationToken);
    }
}
