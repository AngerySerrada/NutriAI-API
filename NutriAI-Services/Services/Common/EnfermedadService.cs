using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NutriAI_Services.Services.Common
{
    public class EnfermedadService : IEnfermedadService
    {
        private readonly AppDbContext _ctx;

        public EnfermedadService(AppDbContext context)
        {
            _ctx = context;
        }

        // =======================
        // ENFERMEDADES
        // =======================

        public async Task<IEnumerable<EnfermedadDto>> GetEnfermedadesAsync(CancellationToken ct = default)
        {
            return await _ctx.Enfermedades
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .Select(e => new EnfermedadDto
                {
                    IdEnfermedad = e.IdEnfermedad,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                })
                .ToListAsync(ct);
        }

        public async Task<EnfermedadDto?> GetEnfermedadByIdAsync(int idEnfermedad, CancellationToken ct = default)
        {
            return await _ctx.Enfermedades
                .AsNoTracking()
                .Where(e => e.IdEnfermedad == idEnfermedad)
                .Select(e => new EnfermedadDto
                {
                    IdEnfermedad = e.IdEnfermedad,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<EnfermedadDto>> SearchEnfermedadesByNombreAsync(string nombre, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Enumerable.Empty<EnfermedadDto>();

            var nombreNormalizado = nombre.Trim().ToLower();

            return await _ctx.Enfermedades
                .AsNoTracking()
                .Where(e => e.Nombre.ToLower().Contains(nombreNormalizado))
                .OrderBy(e => e.Nombre)
                .Select(e => new EnfermedadDto
                {
                    IdEnfermedad = e.IdEnfermedad,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                })
                .ToListAsync(ct);
        }

        // =======================
        // ASOCIACIONES USUARIO-ENFERMEDAD
        // =======================

        public async Task<UsuarioEnfermedadDto> AsociarEnfermedadAsync(AsociarEnfermedadRequest request, CancellationToken ct = default)
        {
            // Verificar que el usuario existe y está activo
            var usuarioExiste = await _ctx.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.IdUsuario == request.IdUsuario && u.Activo, ct);

            if (!usuarioExiste)
                throw new InvalidOperationException("Usuario no encontrado o inactivo.");

            // Verificar que la enfermedad existe
            var enfermedadExiste = await _ctx.Enfermedades
                .AsNoTracking()
                .AnyAsync(e => e.IdEnfermedad == request.IdEnfermedad, ct);

            if (!enfermedadExiste)
                throw new InvalidOperationException("Enfermedad no encontrada.");

            // Verificar si ya existe la asociación
            var asociacionExistente = await _ctx.UsuarioEnfermedads
                .FirstOrDefaultAsync(ue => ue.IdUsuario == request.IdUsuario && ue.IdEnfermedad == request.IdEnfermedad, ct);

            if (asociacionExistente != null)
            {
                // Si ya existe, actualizar la información
                asociacionExistente.FechaDiagnostico = request.FechaDiagnostico;
                asociacionExistente.Observaciones = request.Observaciones;
                await _ctx.SaveChangesAsync(ct);

                return await GetUsuarioEnfermedadByIdAsync(asociacionExistente.IdUsuarioEnfermedad, ct);
            }

            // Crear nueva asociación
            var nuevaAsociacion = new UsuarioEnfermedad
            {
                IdUsuario = request.IdUsuario,
                IdEnfermedad = request.IdEnfermedad,
                FechaDiagnostico = request.FechaDiagnostico,
                Observaciones = request.Observaciones
            };

            _ctx.UsuarioEnfermedads.Add(nuevaAsociacion);
            await _ctx.SaveChangesAsync(ct);

            return await GetUsuarioEnfermedadByIdAsync(nuevaAsociacion.IdUsuarioEnfermedad, ct);
        }

        public async Task<bool> DesasociarEnfermedadAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioEnfermedads
                .FirstOrDefaultAsync(ue => ue.IdUsuario == idUsuario && ue.IdEnfermedad == idEnfermedad, ct);

            if (asociacion == null)
                return false;

            _ctx.UsuarioEnfermedads.Remove(asociacion);
            await _ctx.SaveChangesAsync(ct);

            return true;
        }

        public async Task<IEnumerable<UsuarioEnfermedadDto>> GetEnfermedadesUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            return await _ctx.UsuarioEnfermedads
                .AsNoTracking()
                .Include(ue => ue.IdEnfermedadNavigation)
                .Where(ue => ue.IdUsuario == idUsuario)
                .Select(ue => new UsuarioEnfermedadDto
                {
                    IdUsuarioEnfermedad = ue.IdUsuarioEnfermedad,
                    IdUsuario = ue.IdUsuario,
                    IdEnfermedad = ue.IdEnfermedad,
                    FechaDiagnostico = ue.FechaDiagnostico,
                    Observaciones = ue.Observaciones,
                    Enfermedad = new EnfermedadDto
                    {
                        IdEnfermedad = ue.IdEnfermedadNavigation.IdEnfermedad,
                        Nombre = ue.IdEnfermedadNavigation.Nombre,
                        Descripcion = ue.IdEnfermedadNavigation.Descripcion
                    }
                })
                .ToListAsync(ct);
        }

        public async Task<bool> IsEnfermedadAsociadaAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default)
        {
            return await _ctx.UsuarioEnfermedads
                .AsNoTracking()
                .AnyAsync(ue => ue.IdUsuario == idUsuario && ue.IdEnfermedad == idEnfermedad, ct);
        }

        public async Task<bool> ActualizarEnfermedadAsync(ActualizarEnfermedadRequest request, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioEnfermedads
                .FirstOrDefaultAsync(ue => ue.IdUsuarioEnfermedad == request.IdUsuarioEnfermedad, ct);

            if (asociacion == null)
                return false;

            asociacion.FechaDiagnostico = request.FechaDiagnostico;
            asociacion.Observaciones = request.Observaciones;

            await _ctx.SaveChangesAsync(ct);

            return true;
        }

        public async Task<UsuarioEnfermedadDto?> GetUsuarioEnfermedadAsync(int idUsuario, int idEnfermedad, CancellationToken ct = default)
        {
            return await _ctx.UsuarioEnfermedads
                .AsNoTracking()
                .Include(ue => ue.IdEnfermedadNavigation)
                .Where(ue => ue.IdUsuario == idUsuario && ue.IdEnfermedad == idEnfermedad)
                .Select(ue => new UsuarioEnfermedadDto
                {
                    IdUsuarioEnfermedad = ue.IdUsuarioEnfermedad,
                    IdUsuario = ue.IdUsuario,
                    IdEnfermedad = ue.IdEnfermedad,
                    FechaDiagnostico = ue.FechaDiagnostico,
                    Observaciones = ue.Observaciones,
                    Enfermedad = new EnfermedadDto
                    {
                        IdEnfermedad = ue.IdEnfermedadNavigation.IdEnfermedad,
                        Nombre = ue.IdEnfermedadNavigation.Nombre,
                        Descripcion = ue.IdEnfermedadNavigation.Descripcion
                    }
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> EliminarAsociacionAsync(int idUsuarioEnfermedad, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioEnfermedads
                .FirstOrDefaultAsync(ue => ue.IdUsuarioEnfermedad == idUsuarioEnfermedad, ct);

            if (asociacion == null)
                return false;

            _ctx.UsuarioEnfermedads.Remove(asociacion);
            await _ctx.SaveChangesAsync(ct);

            return true;
        }

        // =======================
        // MÉTODOS PRIVADOS
        // =======================

        private async Task<UsuarioEnfermedadDto> GetUsuarioEnfermedadByIdAsync(int idUsuarioEnfermedad, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioEnfermedads
                .AsNoTracking()
                .Include(ue => ue.IdEnfermedadNavigation)
                .Where(ue => ue.IdUsuarioEnfermedad == idUsuarioEnfermedad)
                .Select(ue => new UsuarioEnfermedadDto
                {
                    IdUsuarioEnfermedad = ue.IdUsuarioEnfermedad,
                    IdUsuario = ue.IdUsuario,
                    IdEnfermedad = ue.IdEnfermedad,
                    FechaDiagnostico = ue.FechaDiagnostico,
                    Observaciones = ue.Observaciones,
                    Enfermedad = new EnfermedadDto
                    {
                        IdEnfermedad = ue.IdEnfermedadNavigation.IdEnfermedad,
                        Nombre = ue.IdEnfermedadNavigation.Nombre,
                        Descripcion = ue.IdEnfermedadNavigation.Descripcion
                    }
                })
                .FirstOrDefaultAsync(ct);

            return asociacion ?? throw new InvalidOperationException("Asociación no encontrada.");
        }
    }
}
