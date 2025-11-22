using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.AI;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using NutriAI_Data.Models;

namespace NutriAI_Services.Services.AI
{
    public class ConversacionService : IConversacionService
    {
        private readonly AppDbContext _context;

        public ConversacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConversacionDto?> CrearConversacionAsync(CrearConversacionRequest request, CancellationToken cancellationToken)
        {
            // Verificar que el usuario existe
            var usuarioExists = await _context.Usuarios
                .AnyAsync(u => u.IdUsuario == request.IdUsuario && u.Activo, cancellationToken);

            if (!usuarioExists)
                return null;

            var conversacion = new Conversacione
            {
                IdUsuario = request.IdUsuario,
                Titulo = request.Titulo ?? "Nueva Conversación",
                FechaCreacion = DateTime.UtcNow,
                Activa = true
            };

            _context.Conversaciones.Add(conversacion);
            await _context.SaveChangesAsync(cancellationToken);

            return new ConversacionDto
            {
                IdConversacion = conversacion.IdConversacion,
                IdConversation = conversacion.IdConversacion,
                IdUsuario = conversacion.IdUsuario,
                Titulo = conversacion.Titulo ?? string.Empty,
                Title = conversacion.Titulo ?? string.Empty,
                FechaCreacion = conversacion.FechaCreacion,
                FechaActualizacion = conversacion.FechaActualizacion,
                Activa = conversacion.Activa,
                TotalMensajes = 0,
                Messages = new List<AIMessageDto>()
            };
        }

        public async Task<ConversacionDetalleDto?> ObtenerConversacionPorIdAsync(int idConversacion, CancellationToken cancellationToken)
        {
            var conversacion = await _context.Conversaciones
                .Include(c => c.MensajesConversacions)
                .Where(c => c.IdConversacion == idConversacion)
                .Select(c => new ConversacionDetalleDto
                {
                    IdConversacion = c.IdConversacion,
                    IdUsuario = c.IdUsuario,
                    Titulo = c.Titulo ?? string.Empty,
                    FechaCreacion = c.FechaCreacion,
                    FechaActualizacion = c.FechaActualizacion,
                    Activa = c.Activa,
                    Mensajes = c.MensajesConversacions
                        .OrderBy(m => m.FechaEnvio)
                        .Select(m => new MensajeConversacionDto
                        {
                            IdMensaje = m.IdMensaje,
                            IdConversacion = m.IdConversacion,
                            Rol = m.Rol,
                            Contenido = m.Contenido,
                            FechaEnvio = m.FechaEnvio,
                            TokensUtilizados = m.TokensUtilizados,
                            ContextoIncluido = m.ContextoIncluido
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return conversacion;
        }

        public async Task<IEnumerable<ConversacionDto>> ObtenerConversacionesPorUsuarioAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var conversaciones = await _context.Conversaciones
                .Include(c => c.MensajesConversacions)
                .Where(c => c.IdUsuario == idUsuario)
                .OrderByDescending(c => c.FechaActualizacion ?? c.FechaCreacion)
                .Select(c => new ConversacionDto
                {
                    IdConversacion = c.IdConversacion,
                    IdConversation = c.IdConversacion,
                    IdUsuario = c.IdUsuario,
                    Titulo = c.Titulo ?? string.Empty,
                    Title = c.Titulo ?? string.Empty,
                    FechaCreacion = c.FechaCreacion,
                    FechaActualizacion = c.FechaActualizacion,
                    Activa = c.Activa,
                    TotalMensajes = c.MensajesConversacions.Count,
                    PrimerMensaje = c.MensajesConversacions
                        .OrderBy(m => m.FechaEnvio)
                        .Select(m => m.Contenido)
                        .FirstOrDefault(),
                    Messages = c.MensajesConversacions
                        .OrderBy(m => m.FechaEnvio)
                        .Select(m => new AIMessageDto
                        {
                            Role = m.Rol,
                            Content = m.Contenido,
                            Timestamp = m.FechaEnvio,
                            TokensUsed = m.TokensUtilizados
                        }).ToList()
                })
                .ToListAsync(cancellationToken);

            return conversaciones;
        }

        public async Task<bool> ActualizarTituloConversacionAsync(int idConversacion, string nuevoTitulo, CancellationToken cancellationToken)
        {
            var conversacion = await _context.Conversaciones
                .FirstOrDefaultAsync(c => c.IdConversacion == idConversacion, cancellationToken);

            if (conversacion == null)
                return false;

            conversacion.Titulo = nuevoTitulo;
            conversacion.FechaActualizacion = DateTime.UtcNow;

            _context.Conversaciones.Update(conversacion);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DesactivarConversacionAsync(int idConversacion, CancellationToken cancellationToken)
        {
            var conversacion = await _context.Conversaciones
                .FirstOrDefaultAsync(c => c.IdConversacion == idConversacion, cancellationToken);

            if (conversacion == null)
                return false;

            conversacion.Activa = false;
            conversacion.FechaActualizacion = DateTime.UtcNow;

            _context.Conversaciones.Update(conversacion);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ActivarConversacionAsync(int idConversacion, CancellationToken cancellationToken)
        {
            var conversacion = await _context.Conversaciones
                .FirstOrDefaultAsync(c => c.IdConversacion == idConversacion, cancellationToken);

            if (conversacion == null)
                return false;

            conversacion.Activa = true;
            conversacion.FechaActualizacion = DateTime.UtcNow;

            _context.Conversaciones.Update(conversacion);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<MensajeConversacionDto?> GuardarMensajeAsync(GuardarMensajeRequest request, CancellationToken cancellationToken)
        {
            // Verificar que la conversación existe
            var conversacionExists = await _context.Conversaciones
                .AnyAsync(c => c.IdConversacion == request.IdConversacion, cancellationToken);

            if (!conversacionExists)
                return null;

            var mensaje = new MensajesConversacion
            {
                IdConversacion = request.IdConversacion,
                Rol = request.Rol,
                Contenido = request.Contenido,
                FechaEnvio = DateTime.UtcNow,
                TokensUtilizados = request.TokensUtilizados,
                ContextoIncluido = request.ContextoIncluido
            };

            _context.MensajesConversacions.Add(mensaje);

            // Actualizar fecha de actualización de la conversación
            var conversacion = await _context.Conversaciones
                .FirstOrDefaultAsync(c => c.IdConversacion == request.IdConversacion, cancellationToken);

            if (conversacion != null)
            {
                conversacion.FechaActualizacion = DateTime.UtcNow;
                _context.Conversaciones.Update(conversacion);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new MensajeConversacionDto
            {
                IdMensaje = mensaje.IdMensaje,
                IdConversacion = mensaje.IdConversacion,
                Rol = mensaje.Rol,
                Contenido = mensaje.Contenido,
                FechaEnvio = mensaje.FechaEnvio,
                TokensUtilizados = mensaje.TokensUtilizados,
                ContextoIncluido = mensaje.ContextoIncluido
            };
        }

        public async Task<IEnumerable<MensajeConversacionDto>> ObtenerMensajesPorConversacionAsync(int idConversacion, CancellationToken cancellationToken)
        {
            var mensajes = await _context.MensajesConversacions
                .Where(m => m.IdConversacion == idConversacion)
                .OrderBy(m => m.FechaEnvio)
                .Select(m => new MensajeConversacionDto
                {
                    IdMensaje = m.IdMensaje,
                    IdConversacion = m.IdConversacion,
                    Rol = m.Rol,
                    Contenido = m.Contenido,
                    FechaEnvio = m.FechaEnvio,
                    TokensUtilizados = m.TokensUtilizados,
                    ContextoIncluido = m.ContextoIncluido
                })
                .ToListAsync(cancellationToken);

            return mensajes;
        }

        public async Task<HistorialConversacionesViewModel> ObtenerHistorialConversacionesAsync(int idUsuario, CancellationToken cancellationToken)
        {
            var conversaciones = await ObtenerConversacionesPorUsuarioAsync(idUsuario, cancellationToken);
            var conversacionesList = conversaciones.ToList();

            return new HistorialConversacionesViewModel
            {
                Conversaciones = conversacionesList,
                TotalConversaciones = conversacionesList.Count,
                TotalMensajes = conversacionesList.Sum(c => c.TotalMensajes)
            };
        }

        public async Task<ConversacionViewModel?> ObtenerConversacionViewModelAsync(int idConversacion, int idUsuario, CancellationToken cancellationToken)
        {
            var conversacion = await ObtenerConversacionPorIdAsync(idConversacion, cancellationToken);

            if (conversacion == null)
                return null;

            var puedeEditar = conversacion.IdUsuario == idUsuario;

            return new ConversacionViewModel
            {
                Conversacion = conversacion,
                PuedeEditar = puedeEditar
            };
        }
    }
}
