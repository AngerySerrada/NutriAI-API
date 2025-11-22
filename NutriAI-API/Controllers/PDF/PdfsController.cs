using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.DTOs.PDF;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.PDF
{
    /// <summary>
    /// Controlador para gestionar documentos PDF de conversaciones
    /// </summary>
    [Route("api/pdfs")]
    [ApiController]
    [Authorize]
    public class PdfsController : ControllerBase
    {
        private readonly IPdfDocumentService _pdfDocumentService;

        public PdfsController(IPdfDocumentService pdfDocumentService)
        {
            _pdfDocumentService = pdfDocumentService;
        }

        /// <summary>
        /// Guardar un nuevo PDF
        /// POST /api/pdfs
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GuardarPdf([FromForm] GuardarPdfFormRequest formRequest)
        {
            try
            {
                // Validaciones básicas
                if (formRequest.PdfFile == null || formRequest.PdfFile.Length == 0)
                {
                    return BadRequest(new 
                    { 
                        success = false, 
                        message = "Error de validación: El archivo PDF es requerido" 
                    });
                }

                if (string.IsNullOrWhiteSpace(formRequest.FileName))
                {
                    return BadRequest(new 
                    { 
                        success = false, 
                        message = "Error de validación: El nombre del archivo es requerido" 
                    });
                }

                if (string.IsNullOrWhiteSpace(formRequest.Title))
                {
                    return BadRequest(new 
                    { 
                        success = false, 
                        message = "Error de validación: El título es requerido" 
                    });
                }

                // Validar que el archivo sea PDF
                if (formRequest.PdfFile.ContentType != "application/pdf")
                {
                    return BadRequest(new 
                    { 
                        success = false, 
                        message = "Error de validación: El archivo debe ser de tipo PDF" 
                    });
                }

                // Leer el contenido del archivo
                byte[] pdfContent;
                using (var memoryStream = new MemoryStream())
                {
                    await formRequest.PdfFile.CopyToAsync(memoryStream);
                    pdfContent = memoryStream.ToArray();
                }

                // Crear el request para el servicio
                var request = new GuardarPdfDocumentRequest
                {
                    IdUsuario = formRequest.IdUsuario,
                    FileName = formRequest.FileName,
                    Title = formRequest.Title,
                    Description = formRequest.Description,
                    IdConversation = formRequest.IdConversation,
                    PdfContent = pdfContent,
                    FileSize = formRequest.PdfFile.Length,
                    ContentType = formRequest.PdfFile.ContentType
                };

                var resultado = await _pdfDocumentService.GuardarPdfAsync(request, CancellationToken.None);

                if (!resultado.Success)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    success = false, 
                    message = $"Error interno del servidor: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Obtener todos los PDFs de un usuario con paginación
        /// GET /api/pdfs/usuario/{userId}?pageNumber=1&pageSize=10
        /// </summary>
        [HttpGet("usuario/{userId}")]
        public async Task<IActionResult> ObtenerPdfsPorUsuario(
            int userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    return BadRequest(new { message = "Parámetros de paginación inválidos" });
                }

                var resultado = await _pdfDocumentService.ObtenerPdfsPorUsuarioAsync(
                    userId, pageNumber, pageSize, CancellationToken.None);

                return Ok(resultado.Items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener información de un PDF específico (sin contenido binario)
        /// GET /api/pdfs/{pdfId}
        /// </summary>
        [HttpGet("{pdfId}")]
        public async Task<IActionResult> ObtenerPdfPorId(int pdfId)
        {
            try
            {
                var pdf = await _pdfDocumentService.ObtenerPdfPorIdAsync(pdfId, CancellationToken.None);

                if (pdf == null)
                {
                    return NotFound(new { message = "PDF no encontrado" });
                }

                return Ok(pdf);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Descargar un PDF
        /// GET /api/pdfs/{pdfId}/download
        /// </summary>
        [HttpGet("{pdfId}/download")]
        public async Task<IActionResult> DescargarPdf(int pdfId)
        {
            try
            {
                var resultado = await _pdfDocumentService.DescargarPdfAsync(pdfId, CancellationToken.None);

                if (resultado == null)
                {
                    return NotFound(new { message = "PDF no encontrado" });
                }

                var (content, fileName, contentType) = resultado.Value;

                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Eliminar un PDF
        /// DELETE /api/pdfs/{pdfId}
        /// </summary>
        [HttpDelete("{pdfId}")]
        public async Task<IActionResult> EliminarPdf(int pdfId)
        {
            try
            {
                var resultado = await _pdfDocumentService.EliminarPdfAsync(pdfId, CancellationToken.None);

                if (!resultado)
                {
                    return NotFound(new { message = "PDF no encontrado" });
                }

                return Ok(new { message = "PDF eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener todos los PDFs de una conversación
        /// GET /api/pdfs/conversacion/{conversacionId}
        /// </summary>
        [HttpGet("conversacion/{conversacionId}")]
        public async Task<IActionResult> ObtenerPdfsPorConversacion(int conversacionId)
        {
            try
            {
                var pdfs = await _pdfDocumentService.ObtenerPdfsPorConversacionAsync(
                    conversacionId, CancellationToken.None);

                return Ok(pdfs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }

    /// <summary>
    /// Modelo para el formulario de carga de PDF (compatible con Swagger)
    /// </summary>
    public class GuardarPdfFormRequest
    {
        public int IdUsuario { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? IdConversation { get; set; }
        public IFormFile PdfFile { get; set; } = null!;
    }
}
