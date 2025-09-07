using Microsoft.AspNetCore.Mvc;
using adea_solution_web_api.Models;
using adea_solution_web_api.Services;
using adea_solution_web_api.Constants;

namespace adea_solution_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CajasController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly ILogger<CajasController> _logger;

        public CajasController(IDataService dataService, ILogger<CajasController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las cajas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caja>>> GetAllCajas()
        {
            try
            {
                var cajas = await _dataService.GetAllCajasAsync();
                return Ok(cajas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las cajas");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Obtiene una caja por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Caja>> GetCaja(int id)
        {
            try
            {
                var caja = await _dataService.GetCajaByIdAsync(id);
                if (caja == null)
                    return NotFound(string.Format(ErrorMessages.CajaNotFound, id));

                return Ok(caja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la caja {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Crea una nueva caja
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Caja>> CreateCaja(CreateCajaRequest request)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(request.Estado))
                    return BadRequest(ErrorMessages.EstadoRequired);

                if (request.Estado.Length != 3)
                    return BadRequest(ErrorMessages.EstadoLength);

                if (string.IsNullOrWhiteSpace(request.Ubicacion_Id))
                    return BadRequest(ErrorMessages.UbicacionRequired);

                var ubicaciones = await _dataService.GetUbicacionesAsync();
                if (!ubicaciones.Contains(request.Ubicacion_Id))
                    return BadRequest(string.Format(ErrorMessages.UbicacionInvalid, string.Join(", ", ubicaciones)));

                var caja = await _dataService.CreateCajaAsync(request);
                return CreatedAtAction(nameof(GetCaja), new { id = caja.Caja_Id }, caja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la caja");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Actualiza una caja existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Caja>> UpdateCaja(int id, UpdateCajaRequest request)
        {
            try
            {
                if (id != request.Caja_Id)
                    return BadRequest(ErrorMessages.IdMismatch);

                // Validaciones
                if (string.IsNullOrWhiteSpace(request.Estado))
                    return BadRequest(ErrorMessages.EstadoRequired);

                if (request.Estado.Length != 3)
                    return BadRequest(ErrorMessages.EstadoLength);

                if (string.IsNullOrWhiteSpace(request.Ubicacion_Id))
                    return BadRequest(ErrorMessages.UbicacionRequired);

                var ubicaciones = await _dataService.GetUbicacionesAsync();
                if (!ubicaciones.Contains(request.Ubicacion_Id))
                    return BadRequest(string.Format(ErrorMessages.UbicacionInvalid, string.Join(", ", ubicaciones)));

                var caja = await _dataService.UpdateCajaAsync(request);
                if (caja == null)
                    return NotFound(string.Format(ErrorMessages.CajaNotFound, id));

                return Ok(caja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la caja {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Elimina una caja
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCaja(int id)
        {
            try
            {
                var expedientesCount = await _dataService.GetExpedientesCountByCajaIdAsync(id);
                if (expedientesCount > 0)
                    return BadRequest(ErrorMessages.CannotDeleteCajaWithExpedientes);

                var deleted = await _dataService.DeleteCajaAsync(id);
                if (!deleted)
                    return NotFound(string.Format(ErrorMessages.CajaNotFound, id));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la caja {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Obtiene los expedientes de una caja específica
        /// </summary>
        [HttpGet("{id}/expedientes")]
        public async Task<ActionResult<IEnumerable<Expediente>>> GetExpedientesByCaja(int id)
        {
            try
            {
                var caja = await _dataService.GetCajaByIdAsync(id);
                if (caja == null)
                    return NotFound(string.Format(ErrorMessages.CajaNotFound, id));

                var expedientes = await _dataService.GetExpedientesByCajaIdAsync(id);
                return Ok(expedientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los expedientes de la caja {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }
    }
}
