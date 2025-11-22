using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.PDF;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;

namespace NutriAI_Services.Services.PDF
{
    public class RecetaPDFService : IRecetaPDFService
    {
        private readonly AppDbContext _context;

        public RecetaPDFService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RecetaPDFDto?> GuardarRecetaPDFAsync(GuardarRecetaPDFRequest request, CancellationToken cancellationToken)
        {
            // Verificar que el usuario existe
            var usuarioExists = await _context.Usuarios
                .AnyAsync(u => u.IdUsuario == request.IdUsuario && u.Activo, cancellationToken);

            if (!usuarioExists)
                return null;

            // Verificar que las recetas existen y pertenecen al usuario
            var recetasValidas = await _context.Recetas
                .Where(r => request.RecetasIds.Contains(r.IdReceta) && r.IdUsuario == request.IdUsuario && r.Activo)
                .Select(r => new { r.IdReceta, r.Nombre })
                .ToListAsync(cancellationToken);

            if (recetasValidas.Count != request.RecetasIds.Count)
                return null;

            // Crear el PDF
            var recetaPdf = new RecetasPdf
            {
                IdUsuario = request.IdUsuario,
                NombrePdf = request.NombrePDF,
                ArchivoPdf = request.ArchivoPDF,
                TamanoBytes = request.ArchivoPDF.Length,
                FechaGeneracion = DateTime.UtcNow,
                NumeroRecetas = request.RecetasIds.Count
            };

            _context.RecetasPdfs.Add(recetaPdf);
            await _context.SaveChangesAsync(cancellationToken);

            // Guardar los detalles (relación con recetas)
            int orden = 1;
            foreach (var idReceta in request.RecetasIds)
            {
                var detalle = new RecetasPdfDetalle
                {
                    IdRecetaPdf = recetaPdf.IdRecetaPdf,
                    IdReceta = idReceta,
                    Orden = orden++
                };
                _context.RecetasPdfDetalles.Add(detalle);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Obtener los nombres de las recetas para el DTO
            var nombresRecetas = string.Join(", ", recetasValidas.Select(r => r.Nombre));

            return new RecetaPDFDto
            {
                IdRecetaPDF = recetaPdf.IdRecetaPdf,
                IdUsuario = recetaPdf.IdUsuario,
                NombrePDF = recetaPdf.NombrePdf,
                TamanoBytes = recetaPdf.TamanoBytes,
                FechaGeneracion = recetaPdf.FechaGeneracion,
                NumeroRecetas = recetaPdf.NumeroRecetas,
                NombresRecetas = nombresRecetas
            };
        }

        public async Task<RecetaPDFDetalleDto?> ObtenerRecetaPDFPorIdAsync(int idRecetaPDF, CancellationToken cancellationToken)
        {
            var pdf = await _context.RecetasPdfs
                .Where(p => p.IdRecetaPdf == idRecetaPDF)
                .FirstOrDefaultAsync(cancellationToken);

            if (pdf == null)
                return null;

            var recetasIds = await _context.RecetasPdfDetalles
                .Where(d => d.IdRecetaPdf == idRecetaPDF)
                .OrderBy(d => d.Orden)
                .Select(d => d.IdReceta)
                .ToListAsync(cancellationToken);

            return new RecetaPDFDetalleDto
            {
                IdRecetaPDF = pdf.IdRecetaPdf,
                NombrePDF = pdf.NombrePdf,
                ArchivoPDF = pdf.ArchivoPdf,
                TamanoBytes = pdf.TamanoBytes,
                FechaGeneracion = pdf.FechaGeneracion,
                RecetasIds = recetasIds
            };
        }

        public async Task<IEnumerable<RecetaPDFDto>> ObtenerPDFsPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var pdfs = await _context.RecetasPdfs
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.FechaGeneracion)
                .Select(p => new
                {
                    p.IdRecetaPdf,
                    p.IdUsuario,
                    p.NombrePdf,
                    p.TamanoBytes,
                    p.FechaGeneracion,
                    p.NumeroRecetas
                })
                .ToListAsync(cancellationToken);

            var resultado = new List<RecetaPDFDto>();

            foreach (var pdf in pdfs)
            {
                // Obtener los nombres de las recetas para este PDF
                var nombresRecetas = await _context.RecetasPdfDetalles
                    .Where(d => d.IdRecetaPdf == pdf.IdRecetaPdf)
                    .Join(
                        _context.Recetas,
                        detalle => detalle.IdReceta,
                        receta => receta.IdReceta,
                        (detalle, receta) => receta.Nombre
                    )
                    .ToListAsync(cancellationToken);

                resultado.Add(new RecetaPDFDto
                {
                    IdRecetaPDF = pdf.IdRecetaPdf,
                    IdUsuario = pdf.IdUsuario,
                    NombrePDF = pdf.NombrePdf,
                    TamanoBytes = pdf.TamanoBytes,
                    FechaGeneracion = pdf.FechaGeneracion,
                    NumeroRecetas = pdf.NumeroRecetas,
                    NombresRecetas = string.Join(", ", nombresRecetas)
                });
            }

            return resultado;
        }

        public async Task<bool> EliminarRecetaPDFAsync(int idRecetaPDF, CancellationToken cancellationToken)
        {
            var pdf = await _context.RecetasPdfs
                .FirstOrDefaultAsync(p => p.IdRecetaPdf == idRecetaPDF, cancellationToken);

            if (pdf == null)
                return false;

            // Eliminar los detalles primero
            var detalles = await _context.RecetasPdfDetalles
                .Where(d => d.IdRecetaPdf == idRecetaPDF)
                .ToListAsync(cancellationToken);

            _context.RecetasPdfDetalles.RemoveRange(detalles);

            // Eliminar el PDF
            _context.RecetasPdfs.Remove(pdf);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<byte[]?> DescargarRecetaPDFAsync(int idRecetaPDF, CancellationToken cancellationToken)
        {
            var pdf = await _context.RecetasPdfs
                .Where(p => p.IdRecetaPdf == idRecetaPDF)
                .Select(p => p.ArchivoPdf)
                .FirstOrDefaultAsync(cancellationToken);

            return pdf;
        }

        public async Task<MisPDFsViewModel> ObtenerMisPDFsViewModelAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var pdfs = await ObtenerPDFsPorUsuarioAsync(idUsuario, cancellationToken);
            var pdfsList = pdfs.ToList();

            return new MisPDFsViewModel
            {
                PDFs = pdfsList,
                TotalPDFs = pdfsList.Count,
                EspacioTotalBytes = pdfsList.Sum(p => p.TamanoBytes)
            };
        }
    }
}
