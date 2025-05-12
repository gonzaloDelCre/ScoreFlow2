using Application.Standings.DTOs;
using Application.Standings.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.Standings
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandingController : ControllerBase
    {
        private readonly ILogger<StandingController> _logger;
        private readonly StandingUseCaseHandler _handler;

        public StandingController(
            ILogger<StandingController> logger,
            StandingUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Crea una nueva posición en la clasificación
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateStanding([FromBody] StandingRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de standing con datos nulos");
                return BadRequest("Datos de la clasificación inválidos");
            }

            try
            {
                var result = await _handler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetStandingById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear standing");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza una posición existente de la clasificación
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateStanding(int id, [FromBody] StandingRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID de la clasificación no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Standing con ID {id} no encontrado");
                    return NotFound($"Standing con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar standing");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar standing");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todas las posiciones de la clasificación
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetStandings()
        {
            try
            {
                var list = await _handler.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener standings");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una posición por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<StandingResponseDTO>> GetStandingById(int id)
        {
            try
            {
                var item = await _handler.GetByIdAsync(id);
                if (item == null)
                {
                    _logger.LogWarning($"Standing con ID {id} no encontrado");
                    return NotFound($"Standing con ID {id} no encontrado");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener standing por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene las posiciones de una liga
        /// </summary>
        [HttpGet("por-league/{leagueId}")]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetByLeague(int leagueId)
        {
            try
            {
                var list = await _handler.GetByLeagueAsync(leagueId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener standings por liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene la posición de un equipo en una liga
        /// </summary>
        [HttpGet("por-team-league/{teamId}/{leagueId}")]
        public async Task<ActionResult<StandingResponseDTO>> GetByTeamAndLeague(int teamId, int leagueId)
        {
            try
            {
                var item = await _handler.GetByTeamAndLeagueAsync(teamId, leagueId);
                if (item == null)
                {
                    _logger.LogWarning($"Standing para equipo {teamId} en liga {leagueId} no encontrado");
                    return NotFound($"Standing para equipo {teamId} en liga {leagueId} no encontrado");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener standing por equipo y liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene la clasificación completa de una liga
        /// </summary>
        [HttpGet("clasificacion/{leagueId}")]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetClassification(int leagueId)
        {
            try
            {
                var list = await _handler.GetClassificationAsync(leagueId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clasificación completa");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene el top N posiciones por puntos
        /// </summary>
        [HttpGet("top-by-points/{topN}")]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetTopByPoints(int topN)
        {
            try
            {
                var list = await _handler.GetTopByPointsAsync(topN);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener top por puntos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene posiciones cuyo goal difference está en el rango dado
        /// </summary>
        [HttpGet("por-gd-range")]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetByGoalDifferenceRange(
            [FromQuery] int minGD,
            [FromQuery] int maxGD)
        {
            try
            {
                var list = await _handler.GetByGoalDifferenceRangeAsync(minGD, maxGD);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener standings por rango de goal difference");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina una posición de la clasificación por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteStanding(int id)
        {
            try
            {
                var ok = await _handler.DeleteAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Standing con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Standing con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar standing");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarClasificacion(
            [FromQuery] int competitionId,
            [FromQuery] int leagueId)
        {
            if (competitionId <= 0 || leagueId <= 0)
                return BadRequest("Debe indicar competitionId y leagueId válidos");

            try
            {
                var result = await _handler.ImportStandingsAsync(competitionId, leagueId);
                return Ok($"Importación completada: {result} filas procesadas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar standings");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    }
}
