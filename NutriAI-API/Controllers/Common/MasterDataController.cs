using Microsoft.AspNetCore.Mvc;
using NutriAI_Core.Interfaces;

namespace NutriAI_API.Controllers.Common
{
    [Route("api/masterdata")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;

        public MasterDataController(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        [HttpGet("comunas")]
        public async Task<IActionResult> GetComunas()
        {
            try
            {
                var comunas = await _masterDataService.GetComunasAsync(CancellationToken.None);
                return Ok(comunas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("niveles-actividad")]
        public async Task<IActionResult> GetNivelesActividad()
        {
            try
            {
                var niveles = await _masterDataService.GetNivelesActividadAsync(CancellationToken.None);
                return Ok(niveles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("sexos")]
        public async Task<IActionResult> GetSexos()
        {
            try
            {
                var sexos = await _masterDataService.GetSexosAsync(CancellationToken.None);
                return Ok(sexos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("registration-data")]
        public async Task<IActionResult> GetRegistrationData()
        {
            try
            {
                var comunas = await _masterDataService.GetComunasAsync(CancellationToken.None);
                var niveles = await _masterDataService.GetNivelesActividadAsync(CancellationToken.None);
                var sexos = await _masterDataService.GetSexosAsync(CancellationToken.None);

                return Ok(new
                {
                    comunas,
                    nivelesActividad = niveles,
                    sexos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}