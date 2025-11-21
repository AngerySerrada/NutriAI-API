using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;

namespace NutriAI_Services.Services.Common
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync(CancellationToken cancellationToken)
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.IdComunaNavigation)
                .Include(u => u.IdNivelActividadNavigation)
                .Include(u => u.IdSexoNavigation)
                .Where(u => u.Activo)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Edad = u.Edad,
                    IdComuna = u.IdComuna,
                    IdNivelActividad = u.IdNivelActividad,
                    IdSexo = u.IdSexo,
                    FechaNacimiento = u.FechaNacimiento,
                    Altura = u.Altura,
                    Peso = u.Peso,
                    Username = u.Username,
                    FechaCreacion = u.FechaCreacion,
                    FechaActualizacion = u.FechaActualizacion,
                    Activo = u.Activo,
                    Comuna = new ComunaDto
                    {
                        IdComuna = u.IdComunaNavigation.IdComuna,
                        Nombre = u.IdComunaNavigation.Nombre,
                        Activo = u.IdComunaNavigation.Activo
                    },
                    NivelActividad = new NivelActividadDto
                    {
                        IdNivelActividad = u.IdNivelActividadNavigation.IdNivelActividad,
                        Descripcion = u.IdNivelActividadNavigation.Descripcion,
                        Activo = u.IdNivelActividadNavigation.Activo
                    },
                    Sexo = new SexoDto
                    {
                        IdSexo = u.IdSexoNavigation.IdSexo,
                        Descripcion = u.IdSexoNavigation.Descripcion
                    }
                })
                .ToListAsync(cancellationToken);

            return usuarios;
        }

        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdComunaNavigation)
                .Include(u => u.IdNivelActividadNavigation)
                .Include(u => u.IdSexoNavigation)
                .Where(u => u.IdUsuario == idUsuario && u.Activo)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Edad = u.Edad,
                    IdComuna = u.IdComuna,
                    IdNivelActividad = u.IdNivelActividad,
                    IdSexo = u.IdSexo,
                    FechaNacimiento = u.FechaNacimiento,
                    Altura = u.Altura,
                    Peso = u.Peso,
                    Username = u.Username,
                    FechaCreacion = u.FechaCreacion,
                    FechaActualizacion = u.FechaActualizacion,
                    Activo = u.Activo,
                    Comuna = new ComunaDto
                    {
                        IdComuna = u.IdComunaNavigation.IdComuna,
                        Nombre = u.IdComunaNavigation.Nombre,
                        Activo = u.IdComunaNavigation.Activo
                    },
                    NivelActividad = new NivelActividadDto
                    {
                        IdNivelActividad = u.IdNivelActividadNavigation.IdNivelActividad,
                        Descripcion = u.IdNivelActividadNavigation.Descripcion,
                        Activo = u.IdNivelActividadNavigation.Activo
                    },
                    Sexo = new SexoDto
                    {
                        IdSexo = u.IdSexoNavigation.IdSexo,
                        Descripcion = u.IdSexoNavigation.Descripcion
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return usuario;
        }

        public async Task<UsuarioDto?> GetUsuarioByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdComunaNavigation)
                .Include(u => u.IdNivelActividadNavigation)
                .Include(u => u.IdSexoNavigation)
                .Where(u => u.Username == username && u.Activo)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Edad = u.Edad,
                    IdComuna = u.IdComuna,
                    IdNivelActividad = u.IdNivelActividad,
                    IdSexo = u.IdSexo,
                    FechaNacimiento = u.FechaNacimiento,
                    Altura = u.Altura,
                    Peso = u.Peso,
                    Username = u.Username,
                    FechaCreacion = u.FechaCreacion,
                    FechaActualizacion = u.FechaActualizacion,
                    Activo = u.Activo,
                    Comuna = new ComunaDto
                    {
                        IdComuna = u.IdComunaNavigation.IdComuna,
                        Nombre = u.IdComunaNavigation.Nombre,
                        Activo = u.IdComunaNavigation.Activo
                    },
                    NivelActividad = new NivelActividadDto
                    {
                        IdNivelActividad = u.IdNivelActividadNavigation.IdNivelActividad,
                        Descripcion = u.IdNivelActividadNavigation.Descripcion,
                        Activo = u.IdNivelActividadNavigation.Activo
                    },
                    Sexo = new SexoDto
                    {
                        IdSexo = u.IdSexoNavigation.IdSexo,
                        Descripcion = u.IdSexoNavigation.Descripcion
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return usuario;
        }

        public async Task<UsuarioDto?> GetUsuarioByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdComunaNavigation)
                .Include(u => u.IdNivelActividadNavigation)
                .Include(u => u.IdSexoNavigation)
                .Where(u => u.Correo == email && u.Activo)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Edad = u.Edad,
                    IdComuna = u.IdComuna,
                    IdNivelActividad = u.IdNivelActividad,
                    IdSexo = u.IdSexo,
                    FechaNacimiento = u.FechaNacimiento,
                    Altura = u.Altura,
                    Peso = u.Peso,
                    Username = u.Username,
                    FechaCreacion = u.FechaCreacion,
                    FechaActualizacion = u.FechaActualizacion,
                    Activo = u.Activo,
                    Comuna = new ComunaDto
                    {
                        IdComuna = u.IdComunaNavigation.IdComuna,
                        Nombre = u.IdComunaNavigation.Nombre,
                        Activo = u.IdComunaNavigation.Activo
                    },
                    NivelActividad = new NivelActividadDto
                    {
                        IdNivelActividad = u.IdNivelActividadNavigation.IdNivelActividad,
                        Descripcion = u.IdNivelActividadNavigation.Descripcion,
                        Activo = u.IdNivelActividadNavigation.Activo
                    },
                    Sexo = new SexoDto
                    {
                        IdSexo = u.IdSexoNavigation.IdSexo,
                        Descripcion = u.IdSexoNavigation.Descripcion
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return usuario;
        }

        public async Task<IEnumerable<PerfilDto>> GetUsuarioPerfilesAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var perfiles = await _context.UsuarioPerfils
                .Include(up => up.IdPerfilNavigation)
                .Where(up => up.IdUsuario == idUsuario && up.Activo)
                .Select(up => new PerfilDto
                {
                    IdPerfil = up.IdPerfilNavigation.IdPerfil,
                    Descripcion = up.IdPerfilNavigation.Descripcion ?? string.Empty
                })
                .ToListAsync(cancellationToken);

            return perfiles;
        }

        public async Task<IEnumerable<EnfermedadDto>> GetUsuarioEnfermedadesAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var enfermedades = await _context.UsuarioEnfermedads
                .Include(ue => ue.IdEnfermedadNavigation)
                .Where(ue => ue.IdUsuario == idUsuario)
                .Select(ue => new EnfermedadDto
                {
                    IdEnfermedad = ue.IdEnfermedadNavigation.IdEnfermedad,
                    Nombre = ue.IdEnfermedadNavigation.Nombre,
                    Descripcion = ue.IdEnfermedadNavigation.Descripcion
                })
                .ToListAsync(cancellationToken);

            return enfermedades;
        }

        public async Task<UsuarioDto?> GetUsuarioWithPerfilesAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var usuario = await GetUsuarioByIdAsync(idUsuario, cancellationToken);
            if (usuario != null)
            {
                usuario.Perfiles = (await GetUsuarioPerfilesAsync(idUsuario, cancellationToken)).ToList();
            }

            return usuario;
        }

        public async Task<bool> UpdateUsuarioAsync(int idUsuario, UsuarioDto usuarioDto, CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null || !usuario.Activo)
                return false;

            usuario.Nombre = usuarioDto.Nombre;
            usuario.Correo = usuarioDto.Correo;
            usuario.Edad = usuarioDto.Edad;
            usuario.IdComuna = usuarioDto.IdComuna;
            usuario.IdNivelActividad = usuarioDto.IdNivelActividad;
            usuario.IdSexo = usuarioDto.IdSexo;
            usuario.FechaNacimiento = usuarioDto.FechaNacimiento;
            usuario.Altura = usuarioDto.Altura;
            usuario.Peso = usuarioDto.Peso;
            usuario.FechaActualizacion = DateTime.Now;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeactivateUsuarioAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                return false;

            usuario.Activo = false;
            usuario.FechaActualizacion = DateTime.Now;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}