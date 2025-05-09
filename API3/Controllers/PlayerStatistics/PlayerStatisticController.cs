using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.PlayerStatistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatisticsController : ControllerBase
    {
        private readonly ILogger<PlayerStatisticsController> _logger;
        private readonly PlayerStatisticUseCaseHandler _handler;

        public PlayerStatisticsController(
            ILogger<PlayerStatisticsController> logger,
            PlayerStatisticUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Obtiene todas las estadísticas de jugadores
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStatisticResponseDTO>>> GetAll()
        {
            try
            {
                var list = await _handler.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una estadística de jugador por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerStatisticResponseDTO>> GetById(int id)
        {
            try
            {
                var stat = await _handler.GetByIdAsync(id);
                if (stat == null)
                {
                    _logger.LogWarning($"Estadística con ID {id} no encontrada");
                    return NotFound($"Estadística con ID {id} no encontrada");
                }
                return Ok(stat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadística por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene las estadísticas de un jugador
        /// </summary>
        [HttpGet("por-player/{playerId}")]
        public async Task<ActionResult<IEnumerable<PlayerStatisticResponseDTO>>> GetByPlayer(int playerId)
        {
            try
            {
                var list = await _handler.GetByPlayerAsync(playerId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas por jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene las estadísticas de un partido
        /// </summary>
        [HttpGet("por-match/{matchId}")]
        public async Task<ActionResult<IEnumerable<PlayerStatisticResponseDTO>>> GetByMatch(int matchId)
        {
            try
            {
                var list = await _handler.GetByMatchAsync(matchId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas por partido");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea una nueva estadística de jugador
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] PlayerStatisticRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de estadística con datos nulos");
                return BadRequest("Datos de la estadística inválidos");
            }

            try
            {
                var result = await _handler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear estadística");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza una estadística de jugador existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlayerStatisticRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID de la estadística no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Estadística con ID {id} no encontrada");
                    return NotFound($"Estadística con ID {id} no encontrada");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar estadística");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estadística");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina una estadística por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _handler.DeleteAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Estadística con ID {id} no encontrada o no pudo ser eliminada");
                    return NotFound($"Estadística con ID {id} no encontrada");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar estadística");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
