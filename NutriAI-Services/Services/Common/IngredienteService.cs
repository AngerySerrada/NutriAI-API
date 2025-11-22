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
        // CRUD INGREDIENTES
        // =======================

        public async Task<IngredienteDto> CrearIngredienteAsync(CrearIngredienteRequest request, CancellationToken ct = default)
        {
            // Verificar si ya existe un ingrediente con el mismo nombre
            var ingredienteExiste = await _ctx.Ingredientes
                .AsNoTracking()
                .AnyAsync(i => i.Nombre.ToLower() == request.Nombre.ToLower(), ct);

            if (ingredienteExiste)
                throw new InvalidOperationException("Ya existe un ingrediente con ese nombre.");

            var nuevoIngrediente = new Ingrediente
            {
                Nombre = request.Nombre,
                Categoria = request.Categoria,
                Calorias = request.Calorias,
                Proteinas = request.Proteinas,
                Carbohidratos = request.Carbohidratos,
                Grasas = request.Grasas
            };

            _ctx.Ingredientes.Add(nuevoIngrediente);
            await _ctx.SaveChangesAsync(ct);

            return new IngredienteDto
            {
                IdIngrediente = nuevoIngrediente.IdIngrediente,
                Nombre = nuevoIngrediente.Nombre,
                Categoria = nuevoIngrediente.Categoria,
                Calorias = nuevoIngrediente.Calorias,
                Proteinas = nuevoIngrediente.Proteinas,
                Carbohidratos = nuevoIngrediente.Carbohidratos,
                Grasas = nuevoIngrediente.Grasas
            };
        }

        public async Task<IngredienteDto?> ActualizarIngredienteAsync(ActualizarIngredienteRequest request, CancellationToken ct = default)
        {
            var ingrediente = await _ctx.Ingredientes
                .FirstOrDefaultAsync(i => i.IdIngrediente == request.IdIngrediente, ct);

            if (ingrediente == null)
                return null;

            // Verificar si el nuevo nombre ya existe en otro ingrediente
            var nombreExiste = await _ctx.Ingredientes
                .AsNoTracking()
                .AnyAsync(i => i.Nombre.ToLower() == request.Nombre.ToLower() && i.IdIngrediente != request.IdIngrediente, ct);

            if (nombreExiste)
                throw new InvalidOperationException("Ya existe otro ingrediente con ese nombre.");

            ingrediente.Nombre = request.Nombre;
            ingrediente.Categoria = request.Categoria;
            ingrediente.Calorias = request.Calorias;
            ingrediente.Proteinas = request.Proteinas;
            ingrediente.Carbohidratos = request.Carbohidratos;
            ingrediente.Grasas = request.Grasas;

            await _ctx.SaveChangesAsync(ct);

            return new IngredienteDto
            {
                IdIngrediente = ingrediente.IdIngrediente,
                Nombre = ingrediente.Nombre,
                Categoria = ingrediente.Categoria,
                Calorias = ingrediente.Calorias,
                Proteinas = ingrediente.Proteinas,
                Carbohidratos = ingrediente.Carbohidratos,
                Grasas = ingrediente.Grasas
            };
        }

        public async Task<bool> EliminarIngredienteAsync(int idIngrediente, CancellationToken ct = default)
        {
            var ingrediente = await _ctx.Ingredientes
                .FirstOrDefaultAsync(i => i.IdIngrediente == idIngrediente, ct);

            if (ingrediente == null)
                return false;

            // Verificar si el ingrediente tiene asociaciones con usuarios
            var tieneAsociaciones = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .AnyAsync(ui => ui.IdIngrediente == idIngrediente, ct);

            if (tieneAsociaciones)
                throw new InvalidOperationException("No se puede eliminar el ingrediente porque está asociado a uno o más usuarios.");

            _ctx.Ingredientes.Remove(ingrediente);
            await _ctx.SaveChangesAsync(ct);
            return true;
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

        // =======================
        // MÉTODOS PARA GRÁFICOS
        // =======================

        public async Task<EstadisticasNutricionalesDto> GetEstadisticasUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            var ingredientes = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .ToListAsync(ct);

            if (!ingredientes.Any())
            {
                return new EstadisticasNutricionalesDto();
            }

            return new EstadisticasNutricionalesDto
            {
                TotalCalorias = ingredientes.Sum(ui => ui.IdIngredienteNavigation.Calorias * (ui.CantidadGramos ?? 0) / 100),
                TotalProteinas = ingredientes.Sum(ui => ui.IdIngredienteNavigation.Proteinas * (ui.CantidadGramos ?? 0) / 100),
                TotalCarbohidratos = ingredientes.Sum(ui => ui.IdIngredienteNavigation.Carbohidratos * (ui.CantidadGramos ?? 0) / 100),
                TotalGrasas = ingredientes.Sum(ui => ui.IdIngredienteNavigation.Grasas * (ui.CantidadGramos ?? 0) / 100),
                TotalIngredientes = ingredientes.Count,
                TotalGramos = ingredientes.Sum(ui => ui.CantidadGramos ?? 0)
            };
        }

        public async Task<IEnumerable<MacronutrientesPorCategoriaDto>> GetMacronutrientesPorCategoriaAsync(int idUsuario, CancellationToken ct = default)
        {
            var datos = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .GroupBy(ui => ui.IdIngredienteNavigation.Categoria)
                .Select(g => new MacronutrientesPorCategoriaDto
                {
                    Categoria = g.Key,
                    Proteinas = g.Sum(ui => ui.IdIngredienteNavigation.Proteinas * (ui.CantidadGramos ?? 0) / 100),
                    Carbohidratos = g.Sum(ui => ui.IdIngredienteNavigation.Carbohidratos * (ui.CantidadGramos ?? 0) / 100),
                    Grasas = g.Sum(ui => ui.IdIngredienteNavigation.Grasas * (ui.CantidadGramos ?? 0) / 100),
                    Calorias = g.Sum(ui => ui.IdIngredienteNavigation.Calorias * (ui.CantidadGramos ?? 0) / 100),
                    CantidadIngredientes = g.Count()
                })
                .OrderByDescending(m => m.Calorias)
                .ToListAsync(ct);

            return datos;
        }

        public async Task<IEnumerable<IngredienteMasConsumidoDto>> GetIngredientesMasConsumidosAsync(int idUsuario, int top = 10, CancellationToken ct = default)
        {
            var datos = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .GroupBy(ui => new
                {
                    ui.IdIngrediente,
                    ui.IdIngredienteNavigation.Nombre,
                    ui.IdIngredienteNavigation.Categoria,
                    ui.IdIngredienteNavigation.Calorias
                })
                .Select(g => new IngredienteMasConsumidoDto
                {
                    IdIngrediente = g.Key.IdIngrediente,
                    Nombre = g.Key.Nombre,
                    Categoria = g.Key.Categoria,
                    CantidadTotalGramos = g.Sum(ui => ui.CantidadGramos ?? 0),
                    CaloriasTotales = g.Sum(ui => g.Key.Calorias * (ui.CantidadGramos ?? 0) / 100),
                    VecesConsumido = g.Count()
                })
                .OrderByDescending(i => i.CantidadTotalGramos)
                .Take(top)
                .ToListAsync(ct);

            return datos;
        }

        public async Task<IEnumerable<CaloriasPorCategoriaDto>> GetCaloriasPorCategoriaAsync(int idUsuario, CancellationToken ct = default)
        {
            var datos = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .GroupBy(ui => ui.IdIngredienteNavigation.Categoria)
                .Select(g => new CaloriasPorCategoriaDto
                {
                    Categoria = g.Key,
                    TotalCalorias = g.Sum(ui => ui.IdIngredienteNavigation.Calorias * (ui.CantidadGramos ?? 0) / 100),
                    CantidadIngredientes = g.Count(),
                    PromedioCaloriasPorIngrediente = g.Average(ui => ui.IdIngredienteNavigation.Calorias * (ui.CantidadGramos ?? 0) / 100)
                })
                .OrderByDescending(c => c.TotalCalorias)
                .ToListAsync(ct);

            return datos;
        }

        public async Task<BalanceNutricionalDto> GetBalanceNutricionalAsync(int idUsuario, CancellationToken ct = default)
        {
            var estadisticas = await GetEstadisticasUsuarioAsync(idUsuario, ct);

            if (estadisticas.TotalCalorias == 0)
            {
                return new BalanceNutricionalDto();
            }

            // Calorías por gramo: Proteínas=4, Carbohidratos=4, Grasas=9
            var caloriasProteinas = estadisticas.TotalProteinas * 4;
            var caloriasCarbohidratos = estadisticas.TotalCarbohidratos * 4;
            var caloriasGrasas = estadisticas.TotalGrasas * 9;

            var totalCaloriasMacros = caloriasProteinas + caloriasCarbohidratos + caloriasGrasas;

            return new BalanceNutricionalDto
            {
                TotalCalorias = estadisticas.TotalCalorias,
                GramosProteinas = estadisticas.TotalProteinas,
                GramosCarbohidratos = estadisticas.TotalCarbohidratos,
                GramosGrasas = estadisticas.TotalGrasas,
                CaloriasProteinas = caloriasProteinas,
                CaloriasCarbohidratos = caloriasCarbohidratos,
                CaloriasGrasas = caloriasGrasas,
                PorcentajeProteinas = totalCaloriasMacros > 0 ? Math.Round((caloriasProteinas / totalCaloriasMacros) * 100, 2) : 0,
                PorcentajeCarbohidratos = totalCaloriasMacros > 0 ? Math.Round((caloriasCarbohidratos / totalCaloriasMacros) * 100, 2) : 0,
                PorcentajeGrasas = totalCaloriasMacros > 0 ? Math.Round((caloriasGrasas / totalCaloriasMacros) * 100, 2) : 0
            };
        }

        public async Task<IEnumerable<HistorialIngredientesDto>> GetHistorialIngredientesAsync(int idUsuario, int dias = 30, CancellationToken ct = default)
        {
            var fechaInicio = DateTime.UtcNow.AddDays(-dias);

            var datos = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario && ui.FechaRegistro >= fechaInicio)
                .Include(ui => ui.IdIngredienteNavigation)
                .GroupBy(ui => ui.FechaRegistro.Date)
                .Select(g => new HistorialIngredientesDto
                {
                    Fecha = g.Key,
                    CantidadIngredientes = g.Count(),
                    TotalGramos = g.Sum(ui => ui.CantidadGramos ?? 0),
                    TotalCalorias = g.Sum(ui => ui.IdIngredienteNavigation.Calorias * (ui.CantidadGramos ?? 0) / 100)
                })
                .OrderBy(h => h.Fecha)
                .ToListAsync(ct);

            return datos;
        }

        public async Task<IEnumerable<VariedadCategoriasDto>> GetVariedadCategoriasAsync(int idUsuario, CancellationToken ct = default)
        {
            var totalIngredientes = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .CountAsync(ct);

            if (totalIngredientes == 0)
            {
                return Enumerable.Empty<VariedadCategoriasDto>();
            }

            var datos = await _ctx.UsuarioIngredientes
                .AsNoTracking()
                .Where(ui => ui.IdUsuario == idUsuario)
                .Include(ui => ui.IdIngredienteNavigation)
                .GroupBy(ui => ui.IdIngredienteNavigation.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    CantidadIngredientes = g.Count(),
                    Ingredientes = g.Select(ui => ui.IdIngredienteNavigation.Nombre).Distinct().ToList()
                })
                .ToListAsync(ct);

            return datos.Select(d => new VariedadCategoriasDto
            {
                Categoria = d.Categoria,
                CantidadIngredientes = d.CantidadIngredientes,
                PorcentajeDelTotal = Math.Round((decimal)d.CantidadIngredientes / totalIngredientes * 100, 2),
                IngredientesPrincipales = d.Ingredientes.Take(5).ToList()
            })
            .OrderByDescending(v => v.CantidadIngredientes)
            .ToList();
        }
    }
}