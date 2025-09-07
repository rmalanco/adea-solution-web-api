using Microsoft.AspNetCore.Mvc;
using adea_solution_web_api.Services;
using adea_solution_web_api.Constants;

namespace adea_solution_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpcionesController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly ILogger<OpcionesController> _logger;

        public OpcionesController(IDataService dataService, ILogger<OpcionesController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene las ubicaciones disponibles para las cajas
        /// </summary>
        [HttpGet("ubicaciones")]
        public async Task<ActionResult<IEnumerable<string>>> GetUbicaciones()
        {
            try
            {
                var ubicaciones = await _dataService.GetUbicacionesAsync();
                return Ok(ubicaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ubicaciones");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Obtiene los tipos de expediente disponibles
        /// </summary>
        [HttpGet("tipos-expediente")]
        public async Task<ActionResult<IEnumerable<string>>> GetTiposExpediente()
        {
            try
            {
                var tipos = await _dataService.GetTiposExpedienteAsync();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tipos de expediente");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }
    }
}
