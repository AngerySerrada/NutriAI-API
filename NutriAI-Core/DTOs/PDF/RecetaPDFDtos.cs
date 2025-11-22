using System.ComponentModel.DataAnnotations;

namespace NutriAI_Core.DTOs.PDF
{
    // ============================================
    // DTOs para PDFs de Recetas
    // ============================================

    public sealed class RecetaPDFDto
    {
        public int IdRecetaPDF { get; set; }
        public int IdUsuario { get; set; }
        public string NombrePDF { get; set; } = string.Empty;
        public long TamanoBytes { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public int NumeroRecetas { get; set; }
        public string? NombresRecetas { get; set; }
        
        // Para mostrar información en la UI
        public string TamanoFormateado => FormatFileSize(TamanoBytes);
        
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

    public sealed class RecetaPDFDetalleDto
    {
        public int IdRecetaPDF { get; set; }
        public string NombrePDF { get; set; } = string.Empty;
        public byte[] ArchivoPDF { get; set; } = Array.Empty<byte>();
        public long TamanoBytes { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public List<int> RecetasIds { get; set; } = new();
    }

    // ============================================
    // Requests para PDFs
    // ============================================

    public sealed class GenerarRecetaPDFRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una receta")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una receta")]
        public List<int> RecetasIds { get; set; } = new();

        [StringLength(200, ErrorMessage = "El nombre del PDF no puede exceder 200 caracteres")]
        public string? NombrePDF { get; set; }
    }

    public sealed class GuardarRecetaPDFRequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre del PDF es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombrePDF { get; set; } = string.Empty;

        [Required(ErrorMessage = "El archivo PDF es requerido")]
        public byte[] ArchivoPDF { get; set; } = Array.Empty<byte>();

        [Required(ErrorMessage = "Debe incluir al menos una receta")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos una receta")]
        public List<int> RecetasIds { get; set; } = new();
    }

    // ============================================
    // ViewModels para la UI
    // ============================================

    public class MisPDFsViewModel
    {
        public List<RecetaPDFDto> PDFs { get; set; } = new();
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
