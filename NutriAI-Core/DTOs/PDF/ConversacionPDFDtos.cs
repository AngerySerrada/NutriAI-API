using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.PDF
{
    // ============================================
    // DTOs para PDFs de Conversaciones
    // ============================================

    /// <summary>
    /// DTO para mostrar información de un PDF de conversación
    /// </summary>
    public sealed class PdfDocumentDto
    {
        public int IdPdfDocument { get; set; }
        public int IdUsuario { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long FileSize { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ContentType { get; set; } = "application/pdf";
        public int? IdConversation { get; set; }
        
        // Para mostrar información formateada en la UI
        public string TamanoFormateado => FormatFileSize(FileSize);
        
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }

    /// <summary>
    /// DTO para obtener el PDF completo con su contenido binario
    /// </summary>
    public sealed class PdfDocumentDetalleDto
    {
        public int IdPdfDocument { get; set; }
        public int IdUsuario { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long FileSize { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ContentType { get; set; } = "application/pdf";
        public int? IdConversation { get; set; }
        public byte[] PdfContent { get; set; } = Array.Empty<byte>();
    }

    // ============================================
    // Requests para PDFs de Conversaciones
    // ============================================

    /// <summary>
    /// Request para guardar un nuevo PDF (usado por el servicio)
    /// </summary>
    public sealed class GuardarPdfDocumentRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es requerido")]
        [StringLength(255, ErrorMessage = "El nombre del archivo no puede exceder 255 caracteres")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Description { get; set; }

        public int? IdConversation { get; set; }

        [Required(ErrorMessage = "El contenido del PDF es requerido")]
        public byte[] PdfContent { get; set; } = Array.Empty<byte>();
        
        public long FileSize { get; set; }
        public string ContentType { get; set; } = "application/pdf";
    }

    /// <summary>
    /// Request para paginación de PDFs
    /// </summary>
    public sealed class ObtenerPdfsRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor a 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "El tamaño de página debe estar entre 1 y 100")]
        public int PageSize { get; set; } = 10;
    }

    // ============================================
    // Responses
    // ============================================

    /// <summary>
    /// Response para la operación de guardar PDF
    /// </summary>
    public sealed class GuardarPdfResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? IdPdfDocument { get; set; }
    }

    /// <summary>
    /// Response con lista paginada de PDFs
    /// </summary>
    public sealed class PdfDocumentListResponse
    {
        public List<PdfDocumentDto> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }

    // ============================================
    // ViewModels para la UI
    // ============================================

    public sealed class MisPdfsViewModel
    {
        public List<PdfDocumentDto> PDFs { get; set; } = new();
        public int TotalPDFs { get; set; }
        public long EspacioTotalBytes { get; set; }
        
        public string EspacioTotalFormateado
        {
            get
            {
                var sizes = new[] { "B", "KB", "MB", "GB" };
                double len = EspacioTotalBytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return $"{len:0.##} {sizes[order]}";
            }
        }
    }
}
