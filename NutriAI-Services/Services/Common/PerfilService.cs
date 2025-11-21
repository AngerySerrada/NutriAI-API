using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;

namespace NutriAI_Services.Services.Common
{
    public class PerfilService : IPerfilService
    {
        private readonly AppDbContext _context;

        public PerfilService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PerfilDto>> GetAllPerfilesAsync(CancellationToken cancellationToken)
        {
            var perfiles = await _context.Perfils
                .Select(p => new PerfilDto
                {
                    IdPerfil = p.IdPerfil,
                    Descripcion = p.Descripcion ?? string.Empty
                })
                .ToListAsync(cancellationToken);

            return perfiles;
        }

        public async Task<PerfilDto?> GetPerfilByIdAsync(int idPerfil, CancellationToken cancellationToken)
        {
            var perfil = await _context.Perfils
                .Where(p => p.IdPerfil == idPerfil)
                .Select(p => new PerfilDto
                {
                    IdPerfil = p.IdPerfil,
                    Descripcion = p.Descripcion ?? string.Empty
                })
                .FirstOrDefaultAsync(cancellationToken);

            return perfil;
        }

        public async Task<IEnumerable<UsuarioDto>> GetUsuariosByPerfilAsync(int idPerfil, CancellationToken cancellationToken)
        {
            var usuarios = await _context.UsuarioPerfils
                .Include(up => up.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdComunaNavigation)
                .Include(up => up.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdNivelActividadNavigation)
                .Include(up => up.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdSexoNavigation)
                .Where(up => up.IdPerfil == idPerfil && up.Activo && up.IdUsuarioNavigation.Activo)
                .Select(up => new UsuarioDto
                {
                    IdUsuario = up.IdUsuarioNavigation.IdUsuario,
                    Nombre = up.IdUsuarioNavigation.Nombre,
                    Correo = up.IdUsuarioNavigation.Correo,
                    Edad = up.IdUsuarioNavigation.Edad,
                    IdComuna = up.IdUsuarioNavigation.IdComuna,
                    IdNivelActividad = up.IdUsuarioNavigation.IdNivelActividad,
                    IdSexo = up.IdUsuarioNavigation.IdSexo,
                    FechaNacimiento = up.IdUsuarioNavigation.FechaNacimiento,
                    Altura = up.IdUsuarioNavigation.Altura,
                    Peso = up.IdUsuarioNavigation.Peso,
                    Username = up.IdUsuarioNavigation.Username,
                    FechaCreacion = up.IdUsuarioNavigation.FechaCreacion,
                    FechaActualizacion = up.IdUsuarioNavigation.FechaActualizacion,
                    Activo = up.IdUsuarioNavigation.Activo,
                    Comuna = new ComunaDto
                    {
                        IdComuna = up.IdUsuarioNavigation.IdComunaNavigation.IdComuna,
                        Nombre = up.IdUsuarioNavigation.IdComunaNavigation.Nombre,
                        Activo = up.IdUsuarioNavigation.IdComunaNavigation.Activo
                    },
                    NivelActividad = new NivelActividadDto
                    {
                        IdNivelActividad = up.IdUsuarioNavigation.IdNivelActividadNavigation.IdNivelActividad,
                        Descripcion = up.IdUsuarioNavigation.IdNivelActividadNavigation.Descripcion,
                        Activo = up.IdUsuarioNavigation.IdNivelActividadNavigation.Activo
                    },
                    Sexo = new SexoDto
                    {
                        IdSexo = up.IdUsuarioNavigation.IdSexoNavigation.IdSexo,
                        Descripcion = up.IdUsuarioNavigation.IdSexoNavigation.Descripcion
                    }
                })
                .ToListAsync(cancellationToken);

            return usuarios;
        }

        public async Task<bool> AssignPerfilToUsuarioAsync(int idUsuario, int idPerfil, CancellationToken cancellationToken)
        {
            // Verificar si el usuario y perfil existen
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.IdUsuario == idUsuario && u.Activo, cancellationToken);
            var perfilExists = await _context.Perfils.AnyAsync(p => p.IdPerfil == idPerfil, cancellationToken);

            if (!usuarioExists || !perfilExists)
                return false;

            // Verificar si la relación ya existe
            var existingRelation = await _context.UsuarioPerfils
                .FirstOrDefaultAsync(up => up.IdUsuario == idUsuario && up.IdPerfil == idPerfil, cancellationToken);

            if (existingRelation != null)
            {
                // Si existe pero está inactiva, la activamos
                if (!existingRelation.Activo)
                {
                    existingRelation.Activo = true;
                    existingRelation.FechaAsignacion = DateTime.Now;
                    _context.UsuarioPerfils.Update(existingRelation);
                }
                else
                {
                    // Ya existe y está activa
                    return true;
                }
            }
            else
            {
                // Crear nueva relación
                var nuevaRelacion = new UsuarioPerfil
                {
                    IdUsuario = idUsuario,
                    IdPerfil = idPerfil,
                    FechaAsignacion = DateTime.Now,
                    Activo = true
                };

                _context.UsuarioPerfils.Add(nuevaRelacion);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> RemovePerfilFromUsuarioAsync(int idUsuario, int idPerfil, CancellationToken cancellationToken)
        {
            var relation = await _context.UsuarioPerfils
                .FirstOrDefaultAsync(up => up.IdUsuario == idUsuario && up.IdPerfil == idPerfil && up.Activo, cancellationToken);

            if (relation == null)
                return false;

            relation.Activo = false;
            _context.UsuarioPerfils.Update(relation);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<UsuarioPerfilDto>> GetUsuarioPerfilRelationsAsync(CancellationToken cancellationToken)
        {
            var relations = await _context.UsuarioPerfils
                .Include(up => up.IdUsuarioNavigation)
                .Include(up => up.IdPerfilNavigation)
                .Where(up => up.Activo)
                .Select(up => new UsuarioPerfilDto
                {
                    IdUsuario = up.IdUsuario,
                    IdPerfil = up.IdPerfil,
                    FechaAsignacion = up.FechaAsignacion,
                    Activo = up.Activo,
                    Usuario = new UsuarioDto
                    {
                        IdUsuario = up.IdUsuarioNavigation.IdUsuario,
                        Nombre = up.IdUsuarioNavigation.Nombre,
                        Correo = up.IdUsuarioNavigation.Correo,
                        Username = up.IdUsuarioNavigation.Username,
                        Activo = up.IdUsuarioNavigation.Activo
                    },
                    Perfil = new PerfilDto
                    {
                        IdPerfil = up.IdPerfilNavigation.IdPerfil,
                        Descripcion = up.IdPerfilNavigation.Descripcion ?? string.Empty
                    }
                })
                .ToListAsync(cancellationToken);

            return relations;
        }
    }
}