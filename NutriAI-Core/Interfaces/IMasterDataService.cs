using NutriAI_Core.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Core.Interfaces
{
    public interface IMasterDataService
    {
        Task<IEnumerable<ComunaDto>> GetComunasAsync(CancellationToken ct = default);
        Task<IEnumerable<NivelActividadDto>> GetNivelesActividadAsync(CancellationToken ct = default);
        Task<IEnumerable<SexoDto>> GetSexosAsync(CancellationToken ct = default);
    }
}