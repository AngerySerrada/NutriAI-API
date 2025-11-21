using Microsoft.EntityFrameworkCore;
using NutriAI_Core.DTOs.Common;
using NutriAI_Core.Interfaces;
using NutriAI_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Services.Services.Common
{
    public sealed class MasterDataService : IMasterDataService
    {
        private readonly AppDbContext _ctx;

        public MasterDataService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<ComunaDto>> GetComunasAsync(CancellationToken ct = default)
        {
            return await _ctx.Comunas
                .AsNoTracking()
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .Select(c => new ComunaDto
                {
                    IdComuna = c.IdComuna,
                    Nombre = c.Nombre
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<NivelActividadDto>> GetNivelesActividadAsync(CancellationToken ct = default)
        {
            return await _ctx.NivelesActividads
                .AsNoTracking()
                .Where(n => n.Activo)
                .OrderBy(n => n.IdNivelActividad)
                .Select(n => new NivelActividadDto
                {
                    IdNivelActividad = n.IdNivelActividad,
                    Descripcion = n.Descripcion
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<SexoDto>> GetSexosAsync(CancellationToken ct = default)
        {
            return await _ctx.Sexos
                .AsNoTracking()
                .OrderBy(s => s.IdSexo)
                .Select(s => new SexoDto
                {
                    IdSexo = s.IdSexo,
                    Descripcion = s.Descripcion
                })
                .ToListAsync(ct);
        }
    }
}