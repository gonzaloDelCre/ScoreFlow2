using Application.Matches.DTOs;
using Application.Matches.UseCases;
using Application.Matches.UseCases.Scraping;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.Matches
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly MatchUseCaseHandler _handler;

        public MatchController(
            ILogger<MatchController> logger,
            MatchUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Crea un nuevo partido
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateMatch([FromBody] MatchRequestDTO matchRequestDTO)
        {
            if (matchRequestDTO == null)
            {
                _logger.LogWarning("Petición de creación de partido con datos nulos");
                return BadRequest("Datos del partido inválidos");
            }

            try
            {
                var result = await _handler.CreateMatchAsync(matchRequestDTO);
                return CreatedAtAction(nameof(GetMatchById), new { id = result.ID }, result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al crear partido");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear partido");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un partido existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateMatch(int id, [FromBody] MatchRequestDTO matchRequestDTO)
        {
            if (matchRequestDTO == null || matchRequestDTO.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID del partido no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateMatchAsync(matchRequestDTO);
                if (updated == null)
                {
                    _logger.LogWarning($"Partido con ID {id} no encontrado");
                    return NotFound($"Partido con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar partido");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar partido");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todos los partidos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchResponseDTO>>> GetMatches()
        {
            try
            {
                var list = await _handler.GetAllMatchesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partidos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un partido por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResponseDTO>> GetMatchById(int id)
        {
            try
            {
                var match = await _handler.GetMatchByIdAsync(id);
                if (match == null)
                {
                    _logger.LogWarning($"Partido con ID {id} no encontrado");
                    return NotFound($"Partido con ID {id} no encontrado");
                }
                return Ok(match);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partido por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los partidos de un equipo
        /// </summary>
        [HttpGet("por-team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchResponseDTO>>> GetByTeam(int teamId)
        {
            try
            {
                var list = await _handler.GetMatchesByTeamAsync(teamId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partidos por equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los partidos de una liga
        /// </summary>
        [HttpGet("por-league/{leagueId}")]
        public async Task<ActionResult<IEnumerable<MatchResponseDTO>>> GetByLeague(int leagueId)
        {
            try
            {
                var list = await _handler.GetMatchesByLeagueAsync(leagueId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partidos por liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un partido por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                var ok = await _handler.DeleteMatchAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Partido con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Partido con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar partido");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Importa partidos vía scraping
        /// </summary>
        [HttpPost("importar/{leagueId}")]
        public async Task<IActionResult> ImportMatches(int leagueId, [FromQuery] string competitionId = null)
        {
            try
            {
                _logger.LogInformation($"Iniciando importación de partidos para liga ID {leagueId}");
                await _handler.ExecuteAsync(leagueId, competitionId);
                return Ok(new
                {
                    success = true,
                    message = "Importación de partidos completada correctamente"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Error de validación al importar partidos para liga ID {leagueId}");
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Error de operación al importar partidos para liga ID {leagueId}");
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al importar partidos para liga ID {leagueId}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno durante la importación de partidos",
                    error = ex.Message
                });
            }
        }
    }
}