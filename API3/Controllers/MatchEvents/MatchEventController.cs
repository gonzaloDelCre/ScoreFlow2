using Application.MatchEvents.DTOs;
using Application.MatchEvents.UseCases;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.MatchEvents
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchEventController : ControllerBase
    {
        private readonly ILogger<MatchEventController> _logger;
        private readonly MatchEventUseCaseHandler _handler;

        public MatchEventController(
            ILogger<MatchEventController> logger,
            MatchEventUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Crea un nuevo evento de partido
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> Create([FromBody] MatchEventRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de evento con datos nulos");
                return BadRequest("Datos de evento inválidos");
            }

            try
            {
                var result = await _handler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear evento");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un evento de partido existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MatchEventRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID de evento no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Evento con ID {id} no encontrado");
                    return NotFound($"Evento con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar evento");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar evento");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todos los eventos de partido
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchEventResponseDTO>>> GetAll()
        {
            try
            {
                var list = await _handler.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un evento por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchEventResponseDTO>> GetById(int id)
        {
            try
            {
                var item = await _handler.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"Evento con ID {id} no encontrado");
                    return NotFound($"Evento con ID {id} no encontrado");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener evento por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los eventos de un partido
        /// </summary>
        [HttpGet("por-match/{matchId}")]
        public async Task<ActionResult<IEnumerable<MatchEventResponseDTO>>> GetByMatch(int matchId)
        {
            try
            {
                var list = await _handler.GetByMatchAsync(matchId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos por partido");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los eventos de un jugador
        /// </summary>
        [HttpGet("por-player/{playerId}")]
        public async Task<ActionResult<IEnumerable<MatchEventResponseDTO>>> GetByPlayer(int playerId)
        {
            try
            {
                var list = await _handler.GetByPlayerAsync(playerId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos por jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los eventos por tipo
        /// </summary>
        [HttpGet("por-type/{type}")]
        public async Task<ActionResult<IEnumerable<MatchEventResponseDTO>>> GetByType(EventType type)
        {
            try
            {
                var list = await _handler.GetByTypeAsync(type);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos por tipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los eventos en un rango de minutos
        /// </summary>
        [HttpGet("por-minute-range")]
        public async Task<ActionResult<IEnumerable<MatchEventResponseDTO>>> GetByMinuteRange(
            [FromQuery] int from,
            [FromQuery] int to)
        {
            try
            {
                var list = await _handler.GetByMinuteRangeAsync(from, to);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener eventos por rango de minuto");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un evento por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _handler.DeleteAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Evento con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Evento con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar evento");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
