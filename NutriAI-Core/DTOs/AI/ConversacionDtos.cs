using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.AI
{
    // ============================================
    // DTOs para Conversaciones
    // ============================================

    public sealed class ConversacionDto
    {
        public int IdConversation { get; set; }
        public int IdConversacion { get; set; }
        public int IdUsuario { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public List<AIMessageDto> Messages { get; set; } = new();
        public bool Activa { get; set; }
        public int TotalMensajes { get; set; }
        public string? PrimerMensaje { get; set; }
    }

    public sealed class MensajeConversacionDto
    {
        public int IdMensaje { get; set; }
        public int IdConversacion { get; set; }
        public string Rol { get; set; } = string.Empty; // 'user', 'assistant', 'system'
        public string Contenido { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public int? TokensUtilizados { get; set; }
        public string? ContextoIncluido { get; set; }
    }

    public sealed class ConversacionDetalleDto
    {
        public int IdConversacion { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool Activa { get; set; }
        public List<MensajeConversacionDto> Mensajes { get; set; } = new();
    }

    // ============================================
    // Requests para Conversaciones
    // ============================================

    public sealed class CrearConversacionRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string? Titulo { get; set; }
    }

    public sealed class GuardarMensajeRequest
    {
        [Required(ErrorMessage = "El ID de la conversación es requerido")]
        public int IdConversacion { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(user|assistant|system)$", ErrorMessage = "El rol debe ser 'user', 'assistant' o 'system'")]
        public string Rol { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contenido es requerido")]
        public string Contenido { get; set; } = string.Empty;

        public int? TokensUtilizados { get; set; }
        public string? ContextoIncluido { get; set; }
    }

    // Nuevo: Request para filtrar historial
    public sealed class FiltrarHistorialRequest
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? BuscarEnTitulo { get; set; }
        public string? BuscarEnContenido { get; set; }
        public bool? SoloActivas { get; set; } = true;
        public int? Limite { get; set; } = 50;
        public int? Offset { get; set; } = 0;
    }

    // ============================================
    // ViewModels para la UI
    // ============================================

    public class HistorialConversacionesViewModel
    {
        public List<ConversacionDto> Conversaciones { get; set; } = new();
        public int TotalConversaciones { get; set; }
        public int TotalMensajes { get; set; }
    }

    public class ConversacionViewModel
    {
        public ConversacionDetalleDto Conversacion { get; set; } = new();
        public bool PuedeEditar { get; set; }
    }
}
