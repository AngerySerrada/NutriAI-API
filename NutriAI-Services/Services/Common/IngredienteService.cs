using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Services.Services.Common
{
    public sealed class IngredienteService : IIngredienteService
    {
        private readonly AppDbContext _ctx;

        public IngredienteService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<IngredienteDto>> GetIngredientesAsync(CancellationToken ct = default)
        {
            return await _ctx.Ingredientes
                .AsNoTracking()
                .OrderBy(i => i.Nombre)
                .Select(i => new IngredienteDto
                {
                    IdIngrediente = i.IdIngrediente,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Calorias = i.Calorias,
                    Proteinas = i.Proteinas,
                    Carbohidratos = i.Carbohidratos,
                    Grasas = i.Grasas
                })
                .ToListAsync(ct);
        }

        public async Task<IngredienteDto?> GetIngredienteByIdAsync(int idIngrediente, CancellationToken ct = default)
        {
            return await _ctx.Ingredientes
                .AsNoTracking()
                .Where(i => i.IdIngrediente == idIngrediente)
                .Select(i => new IngredienteDto
                {
                    IdIngrediente = i.IdIngrediente,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Calorias = i.Calorias,
                    Proteinas = i.Proteinas,
                    Carbohidratos = i.Carbohidratos,
                    Grasas = i.Grasas
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<IngredienteDto>> SearchIngredientesByNombreAsync(string nombre, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Enumerable.Empty<IngredienteDto>();

            var nombreNormalizado = nombre.Trim().ToLower();

            return await _ctx.Ingredientes
                .AsNoTracking()
                .Where(i => i.Nombre.ToLower().Contains(nombreNormalizado))
                .OrderBy(i => i.Nombre)
                .Select(i => new IngredienteDto
                {
                    IdIngrediente = i.IdIngrediente,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Calorias = i.Calorias,
                    Proteinas = i.Proteinas,
                    Carbohidratos = i.Carbohidratos,
                    Grasas = i.Grasas
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<IngredienteDto>> GetIngredientesByCategoriaAsync(string categoria, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                return Enumerable.Empty<IngredienteDto>();

            var categoriaNormalizada = categoria.Trim().ToLower();

            return await _ctx.Ingredientes
                .AsNoTracking()
                .Where(i => i.Categoria.ToLower() == categoriaNormalizada)
                .OrderBy(i => i.Nombre)
                .Select(i => new IngredienteDto
                {
                    IdIngrediente = i.IdIngrediente,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Calorias = i.Calorias,
                    Proteinas = i.Proteinas,
                    Carbohidratos = i.Carbohidratos,
                    Grasas = i.Grasas
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<string>> GetCategoriasAsync(CancellationToken ct = default)
        {
            return await _ctx.Ingredientes
                .AsNoTracking()
                .Select(i => i.Categoria)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(ct);
        }

        // =======================
        // ASOCIACIONES USUARIO-INGREDIENTE
        // =======================

        public async Task<UsuarioIngredienteDto> AsociarIngredienteAsync(AsociarIngredienteRequest request, CancellationToken ct = default)
        {
            // Verificar que el usuario existe y está activo
            var usuarioExiste = await _ctx.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.IdUsuario == request.IdUsuario && u.Activo, ct);

            if (!usuarioExiste)
                throw new InvalidOperationException("Usuario no encontrado o inactivo.");

            // Verificar que el ingrediente existe
            var ingredienteExiste = await _ctx.Ingredientes
                .AsNoTracking()
                .AnyAsync(i => i.IdIngrediente == request.IdIngrediente, ct);

            if (!ingredienteExiste)
                throw new InvalidOperationException("Ingrediente no encontrado.");

            // Verificar si ya existe la asociación
            var asociacionExistente = await _ctx.UsuarioIngredientes
                .FirstOrDefaultAsync(ui => ui.IdUsuario == request.IdUsuario && ui.IdIngrediente == request.IdIngrediente, ct);

            if (asociacionExistente != null)
            {
                // Si ya existe, actualizar la cantidad y fecha
                asociacionExistente.CantidadGramos = request.CantidadGramos;
                asociacionExistente.FechaRegistro = DateTime.UtcNow;
                await _ctx.SaveChangesAsync(ct);

                return await MapToUsuarioIngredienteDto(asociacionExistente, ct);
            }

            // Crear nueva asociación
            var nuevaAsociacion = new UsuarioIngrediente
            {
                IdUsuario = request.IdUsuario,
                IdIngrediente = request.IdIngrediente,
                CantidadGramos = request.CantidadGramos,
                FechaRegistro = DateTime.UtcNow
            };

            _ctx.UsuarioIngredientes.Add(nuevaAsociacion);
            await _ctx.SaveChangesAsync(ct);

            return await MapToUsuarioIngredienteDto(nuevaAsociacion, ct);
        }

        public async Task<bool> DesasociarIngredienteAsync(int idUsuario, int idIngrediente, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioIngredientes
                .FirstOrDefaultAsync(ui => ui.IdUsuario == idUsuario && ui.IdIngrediente == idIngrediente, ct);

            if (asociacion == null)
                return false;

            _ctx.UsuarioIngredientes.Remove(asociacion);
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<UsuarioIngredienteDto>> GetIngredientesUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            return await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .OrderBy(ui => ui.IdIngredienteNavigation.Nombre)
                .Select(ui => new UsuarioIngredienteDto
                {
                    IdUsuarioIngrediente = ui.IdUsuarioIngrediente,
                    IdUsuario = ui.IdUsuario,
                    IdIngrediente = ui.IdIngrediente,
                    CantidadGramos = ui.CantidadGramos,
                    FechaRegistro = ui.FechaRegistro,
                    Ingrediente = new IngredienteDto
                    {
                        IdIngrediente = ui.IdIngredienteNavigation.IdIngrediente,
                        Nombre = ui.IdIngredienteNavigation.Nombre,
                        Categoria = ui.IdIngredienteNavigation.Categoria,
                        Calorias = ui.IdIngredienteNavigation.Calorias,
                        Proteinas = ui.IdIngredienteNavigation.Proteinas,
                        Carbohidratos = ui.IdIngredienteNavigation.Carbohidratos,
                        Grasas = ui.IdIngredienteNavigation.Grasas
                    }
                })
                .ToListAsync(ct);
        }

        public async Task<bool> IsIngredienteAsociadoAsync(int idUsuario, int idIngrediente, CancellationToken ct = default)
        {
            return await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .AnyAsync(ui => ui.IdUsuario == idUsuario && ui.IdIngrediente == idIngrediente, ct);
        }

        public async Task<bool> ActualizarCantidadIngredienteAsync(ActualizarCantidadIngredienteRequest request, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioIngredientes
                .FirstOrDefaultAsync(ui => ui.IdUsuarioIngrediente == request.IdUsuarioIngrediente, ct);

            if (asociacion == null)
                return false;

            asociacion.CantidadGramos = request.CantidadGramos;
            asociacion.FechaRegistro = DateTime.UtcNow; // Actualizar fecha de registro

            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        public async Task<UsuarioIngredienteDto?> GetUsuarioIngredienteAsync(int idUsuario, int idIngrediente, CancellationToken ct = default)
        {
            return await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario && ui.IdIngrediente == idIngrediente)
                .Include(ui => ui.IdIngredienteNavigation)
                .Select(ui => new UsuarioIngredienteDto
                {
                    IdUsuarioIngrediente = ui.IdUsuarioIngrediente,
                    IdUsuario = ui.IdUsuario,
                    IdIngrediente = ui.IdIngrediente,
                    CantidadGramos = ui.CantidadGramos,
                    FechaRegistro = ui.FechaRegistro,
                    Ingrediente = new IngredienteDto
                    {
                        IdIngrediente = ui.IdIngredienteNavigation.IdIngrediente,
                        Nombre = ui.IdIngredienteNavigation.Nombre,
                        Categoria = ui.IdIngredienteNavigation.Categoria,
                        Calorias = ui.IdIngredienteNavigation.Calorias,
                        Proteinas = ui.IdIngredienteNavigation.Proteinas,
                        Carbohidratos = ui.IdIngredienteNavigation.Carbohidratos,
                        Grasas = ui.IdIngredienteNavigation.Grasas
                    }
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> EliminarAsociacionAsync(int idUsuarioIngrediente, CancellationToken ct = default)
        {
            var asociacion = await _ctx.UsuarioIngredientes
                .FirstOrDefaultAsync(ui => ui.IdUsuarioIngrediente == idUsuarioIngrediente, ct);

            if (asociacion == null)
                return false;

            _ctx.UsuarioIngredientes.Remove(asociacion);
            await _ctx.SaveChangesAsync(ct);
            return true;
        }

        // =======================
        // MÉTODOS AUXILIARES
        // =======================

        private async Task<UsuarioIngredienteDto> MapToUsuarioIngredienteDto(UsuarioIngrediente usuarioIngrediente, CancellationToken ct = default)
        {
            var ingrediente = await _ctx.Ingredientes
                .AsNoTracking()
                .Where(i => i.IdIngrediente == usuarioIngrediente.IdIngrediente)
                .Select(i => new IngredienteDto
                {
                    IdIngrediente = i.IdIngrediente,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Calorias = i.Calorias,
                    Proteinas = i.Proteinas,
                    Carbohidratos = i.Carbohidratos,
                    Grasas = i.Grasas
                })
                .FirstOrDefaultAsync(ct);

            return new UsuarioIngredienteDto
            {
                IdUsuarioIngrediente = usuarioIngrediente.IdUsuarioIngrediente,
                IdUsuario = usuarioIngrediente.IdUsuario,
                IdIngrediente = usuarioIngrediente.IdIngrediente,
                CantidadGramos = usuarioIngrediente.CantidadGramos,
                FechaRegistro = usuarioIngrediente.FechaRegistro,
                Ingrediente = ingrediente
            };
        }
    }
}