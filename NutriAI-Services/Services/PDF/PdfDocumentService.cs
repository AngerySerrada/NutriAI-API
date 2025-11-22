using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.PDF;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;

namespace NutriAI_Services.Services.PDF
{
    /// <summary>
    /// Servicio para gestionar documentos PDF de conversaciones
    /// </summary>
    public class PdfDocumentService : IPdfDocumentService
    {
        private readonly AppDbContext _context;

        public PdfDocumentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GuardarPdfResponse> GuardarPdfAsync(GuardarPdfDocumentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Validar que el usuario existe
                var usuarioExists = await _context.Usuarios
                    .AnyAsync(u => u.IdUsuario == request.IdUsuario && u.Activo, cancellationToken);

                if (!usuarioExists)
                {
                    return new GuardarPdfResponse
                    {
                        Success = false,
                        Message = "El usuario no existe o está inactivo"
                    };
                }

                // Validar que la conversación existe si se proporciona
                if (request.IdConversation.HasValue)
                {
                    var conversacionExists = await _context.Conversaciones
                        .AnyAsync(c => c.IdConversacion == request.IdConversation.Value, cancellationToken);

                    if (!conversacionExists)
                    {
                        return new GuardarPdfResponse
                        {
                            Success = false,
                            Message = "La conversación especificada no existe"
                        };
                    }
                }

                // Validar que el contenido no esté vacío
                if (request.PdfContent == null || request.PdfContent.Length == 0)
                {
                    return new GuardarPdfResponse
                    {
                        Success = false,
                        Message = "El contenido del PDF es requerido"
                    };
                }

                // Validar tamaño del archivo (máximo 10MB)
                const long maxFileSize = 10 * 1024 * 1024; // 10 MB
                if (request.FileSize > maxFileSize)
                {
                    return new GuardarPdfResponse
                    {
                        Success = false,
                        Message = "El archivo excede el tamaño máximo permitido (10MB)"
                    };
                }

                // Crear el documento PDF
                var pdfDocument = new PdfDocument
                {
                    IdUsuario = request.IdUsuario,
                    FileName = request.FileName,
                    Title = request.Title,
                    Description = request.Description,
                    FileSize = request.FileSize,
                    ContentType = request.ContentType,
                    PdfContent = request.PdfContent,
                    FechaCreacion = DateTime.UtcNow,
                    IdConversation = request.IdConversation
                };

                _context.PdfDocuments.Add(pdfDocument);
                await _context.SaveChangesAsync(cancellationToken);

                return new GuardarPdfResponse
                {
                    Success = true,
                    Message = "PDF guardado exitosamente",
                    IdPdfDocument = pdfDocument.IdPdfDocument
                };
            }
            catch (Exception ex)
            {
                return new GuardarPdfResponse
                {
                    Success = false,
                    Message = $"Error al guardar el PDF: {ex.Message}"
                };
            }
        }

        public async Task<PdfDocumentListResponse> ObtenerPdfsPorUsuarioAsync(int idUsuario, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.PdfDocuments
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.FechaCreacion);

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PdfDocumentDto
                {
                    IdPdfDocument = p.IdPdfDocument,
                    IdUsuario = p.IdUsuario,
                    FileName = p.FileName,
                    Title = p.Title,
                    Description = p.Description,
                    FileSize = p.FileSize,
                    FechaCreacion = p.FechaCreacion,
                    ContentType = p.ContentType,
                    IdConversation = p.IdConversation
                })
                .ToListAsync(cancellationToken);

            return new PdfDocumentListResponse
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PdfDocumentDto?> ObtenerPdfPorIdAsync(int idPdfDocument, CancellationToken cancellationToken)
        {
            var pdf = await _context.PdfDocuments
                .Where(p => p.IdPdfDocument == idPdfDocument)
                .Select(p => new PdfDocumentDto
                {
                    IdPdfDocument = p.IdPdfDocument,
                    IdUsuario = p.IdUsuario,
                    FileName = p.FileName,
                    Title = p.Title,
                    Description = p.Description,
                    FileSize = p.FileSize,
                    FechaCreacion = p.FechaCreacion,
                    ContentType = p.ContentType,
                    IdConversation = p.IdConversation
                })
                .FirstOrDefaultAsync(cancellationToken);

            return pdf;
        }

        public async Task<PdfDocumentDetalleDto?> ObtenerPdfDetalleAsync(int idPdfDocument, CancellationToken cancellationToken)
        {
            var pdf = await _context.PdfDocuments
                .Where(p => p.IdPdfDocument == idPdfDocument)
                .Select(p => new PdfDocumentDetalleDto
                {
                    IdPdfDocument = p.IdPdfDocument,
                    IdUsuario = p.IdUsuario,
                    FileName = p.FileName,
                    Title = p.Title,
                    Description = p.Description,
                    FileSize = p.FileSize,
                    FechaCreacion = p.FechaCreacion,
                    ContentType = p.ContentType,
                    IdConversation = p.IdConversation,
                    PdfContent = p.PdfContent
                })
                .FirstOrDefaultAsync(cancellationToken);

            return pdf;
        }

        public async Task<(byte[] content, string fileName, string contentType)?> DescargarPdfAsync(int idPdfDocument, CancellationToken cancellationToken)
        {
            var pdf = await _context.PdfDocuments
                .Where(p => p.IdPdfDocument == idPdfDocument)
                .Select(p => new
                {
                    p.PdfContent,
                    p.FileName,
                    p.ContentType
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (pdf == null)
                return null;

            return (pdf.PdfContent, pdf.FileName, pdf.ContentType);
        }

        public async Task<bool> EliminarPdfAsync(int idPdfDocument, CancellationToken cancellationToken)
        {
            var pdf = await _context.PdfDocuments
                .FirstOrDefaultAsync(p => p.IdPdfDocument == idPdfDocument, cancellationToken);

            if (pdf == null)
                return false;

            _context.PdfDocuments.Remove(pdf);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<PdfDocumentDto>> ObtenerPdfsPorConversacionAsync(int idConversation, CancellationToken cancellationToken)
        {
            var pdfs = await _context.PdfDocuments
                .Where(p => p.IdConversation == idConversation)
                .OrderByDescending(p => p.FechaCreacion)
                .Select(p => new PdfDocumentDto
                {
                    IdPdfDocument = p.IdPdfDocument,
                    IdUsuario = p.IdUsuario,
                    FileName = p.FileName,
                    Title = p.Title,
                    Description = p.Description,
                    FileSize = p.FileSize,
                    FechaCreacion = p.FechaCreacion,
                    ContentType = p.ContentType,
                    IdConversation = p.IdConversation
                })
                .ToListAsync(cancellationToken);

            return pdfs;
        }
    }
}
