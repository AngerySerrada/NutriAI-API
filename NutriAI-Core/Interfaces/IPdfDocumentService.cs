using NutriAI_Core.DTOs.PDF;

namespace NutriAI_Core.Interfaces
{
    /// <summary>
    /// Servicio para gestionar documentos PDF de conversaciones
    /// </summary>
    public interface IPdfDocumentService
    {
        /// <summary>
        /// Guardar un nuevo documento PDF
        /// </summary>
        Task<GuardarPdfResponse> GuardarPdfAsync(GuardarPdfDocumentRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Obtener todos los PDFs de un usuario con paginación
        /// </summary>
        Task<PdfDocumentListResponse> ObtenerPdfsPorUsuarioAsync(int idUsuario, int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Obtener un PDF por su ID (sin contenido binario)
        /// </summary>
        Task<PdfDocumentDto?> ObtenerPdfPorIdAsync(int idPdfDocument, CancellationToken cancellationToken);

        /// <summary>
        /// Obtener un PDF por su ID con contenido binario
        /// </summary>
        Task<PdfDocumentDetalleDto?> ObtenerPdfDetalleAsync(int idPdfDocument, CancellationToken cancellationToken);

        /// <summary>
        /// Descargar un PDF (retorna contenido binario y metadata)
        /// </summary>
        Task<(byte[] content, string fileName, string contentType)?> DescargarPdfAsync(int idPdfDocument, CancellationToken cancellationToken);

        /// <summary>
        /// Eliminar un PDF
        /// </summary>
        Task<bool> EliminarPdfAsync(int idPdfDocument, CancellationToken cancellationToken);

        /// <summary>
        /// Obtener PDFs por conversación
        /// </summary>
        Task<IEnumerable<PdfDocumentDto>> ObtenerPdfsPorConversacionAsync(int idConversation, CancellationToken cancellationToken);
    }
}
