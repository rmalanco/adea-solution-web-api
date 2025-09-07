using Microsoft.AspNetCore.Mvc;
using adea_solution_web_api.Models;
using adea_solution_web_api.Services;
using adea_solution_web_api.Constants;

namespace adea_solution_web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpedientesController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly ILogger<ExpedientesController> _logger;

        public ExpedientesController(IDataService dataService, ILogger<ExpedientesController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los expedientes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expediente>>> GetAllExpedientes()
        {
            try
            {
                var expedientes = await _dataService.GetAllExpedientesAsync();
                return Ok(expedientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los expedientes");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Obtiene un expediente por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Expediente>> GetExpediente(int id)
        {
            try
            {
                var expediente = await _dataService.GetExpedienteByIdAsync(id);
                if (expediente == null)
                    return NotFound(string.Format(ErrorMessages.ExpedienteNotFound, id));

                return Ok(expediente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el expediente {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Crea un nuevo expediente
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Expediente>> CreateExpediente(CreateExpedienteRequest request)
        {
            try
            {
                // Validaciones
                if (request.Caja_Id <= 0)
                    return BadRequest(ErrorMessages.CajaIdRequired);

                if (string.IsNullOrWhiteSpace(request.Nombre_Empleado))
                    return BadRequest(ErrorMessages.NombreEmpleadoRequired);

                if (request.Nombre_Empleado.Length > 100)
                    return BadRequest(ErrorMessages.NombreEmpleadoLength);

                if (string.IsNullOrWhiteSpace(request.Tipo_Expediente))
                    return BadRequest(ErrorMessages.TipoExpedienteRequired);

                var tiposExpediente = await _dataService.GetTiposExpedienteAsync();
                if (!tiposExpediente.Contains(request.Tipo_Expediente))
                    return BadRequest(string.Format(ErrorMessages.TipoExpedienteInvalid, string.Join(", ", tiposExpediente)));

                var expediente = await _dataService.CreateExpedienteAsync(request);
                return CreatedAtAction(nameof(GetExpediente), new { id = expediente.Expediente_Id }, expediente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el expediente");
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Actualiza un expediente existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Expediente>> UpdateExpediente(int id, UpdateExpedienteRequest request)
        {
            try
            {
                if (id != request.Expediente_Id)
                    return BadRequest(ErrorMessages.IdMismatch);

                // Validaciones
                if (request.Caja_Id <= 0)
                    return BadRequest(ErrorMessages.CajaIdRequired);

                if (string.IsNullOrWhiteSpace(request.Nombre_Empleado))
                    return BadRequest(ErrorMessages.NombreEmpleadoRequired);

                if (request.Nombre_Empleado.Length > 100)
                    return BadRequest(ErrorMessages.NombreEmpleadoLength);

                if (string.IsNullOrWhiteSpace(request.Tipo_Expediente))
                    return BadRequest(ErrorMessages.TipoExpedienteRequired);

                var tiposExpediente = await _dataService.GetTiposExpedienteAsync();
                if (!tiposExpediente.Contains(request.Tipo_Expediente))
                    return BadRequest(string.Format(ErrorMessages.TipoExpedienteInvalid, string.Join(", ", tiposExpediente)));

                var expediente = await _dataService.UpdateExpedienteAsync(request);
                if (expediente == null)
                    return NotFound(string.Format(ErrorMessages.ExpedienteNotFound, id));

                return Ok(expediente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el expediente {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }

        /// <summary>
        /// Elimina un expediente
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpediente(int id)
        {
            try
            {
                var deleted = await _dataService.DeleteExpedienteAsync(id);
                if (!deleted)
                    return NotFound(string.Format(ErrorMessages.ExpedienteNotFound, id));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el expediente {Id}", id);
                return StatusCode(500, ErrorMessages.InternalServerError);
            }
        }
    }
}
