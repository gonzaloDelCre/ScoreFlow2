using Application.Leagues.DTOs;
using Application.Leagues.UseCases;
using Application.Standings.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.Leagues
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly LeagueUseCaseHandler _handler;

        public LeagueController(
            ILogger<LeagueController> logger,
            LeagueUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Crea una nueva liga
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateLeague([FromBody] LeagueRequestDTO leagueRequestDTO)
        {
            if (leagueRequestDTO == null)
            {
                _logger.LogWarning("Petición de creación de liga con datos nulos");
                return BadRequest("Datos de la liga inválidos");
            }

            try
            {
                var result = await _handler.CreateLeagueAsync(leagueRequestDTO);
                return CreatedAtAction(nameof(GetLeagueById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza una liga existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateLeague(int id, [FromBody] LeagueRequestDTO leagueRequestDTO)
        {
            if (leagueRequestDTO == null || leagueRequestDTO.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID de la liga no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateLeagueAsync(leagueRequestDTO);
                if (updated == null)
                {
                    _logger.LogWarning($"Liga con ID {id} no encontrada");
                    return NotFound($"Liga con ID {id} no encontrada");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar liga");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todas las ligas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueResponseDTO>>> GetLeagues()
        {
            try
            {
                var list = await _handler.GetAllLeaguesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ligas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una liga por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueResponseDTO>> GetLeagueById(int id)
        {
            try
            {
                var league = await _handler.GetLeagueByIdAsync(id);
                if (league == null)
                {
                    _logger.LogWarning($"Liga con ID {id} no encontrada");
                    return NotFound($"Liga con ID {id} no encontrada");
                }
                return Ok(league);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liga por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una liga por su nombre
        /// </summary>
        [HttpGet("por-name/{name}")]
        public async Task<ActionResult<LeagueResponseDTO>> GetLeagueByName(string name)
        {
            try
            {
                var league = await _handler.GetLeagueByNameAsync(name);
                if (league == null)
                {
                    _logger.LogWarning($"Liga con nombre '{name}' no encontrada");
                    return NotFound($"Liga con nombre '{name}' no encontrada");
                }
                return Ok(league);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liga por nombre");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene la clasificación de una liga
        /// </summary>
        [HttpGet("clasificacion/{leagueId}")]
        public async Task<ActionResult<IEnumerable<StandingResponseDTO>>> GetStandings(int leagueId)
        {
            try
            {
                var standings = await _handler.GetStandingsAsync(leagueId);
                return Ok(standings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clasificación");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza la clasificación de una liga
        /// </summary>
        [HttpPut("actualizar-clasificacion/{leagueId}")]
        public async Task<IActionResult> UpdateStandings(int leagueId, [FromBody] List<StandingRequestDTO> standings)
        {
            if (standings == null)
            {
                _logger.LogWarning("Datos de clasificación nulos");
                return BadRequest("Datos de clasificación inválidos");
            }

            try
            {
                await _handler.UpdateStandingsAsync(leagueId, standings);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar clasificación");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina una liga por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            try
            {
                var ok = await _handler.DeleteLeagueAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Liga con ID {id} no encontrada o no pudo ser eliminada");
                    return NotFound($"Liga con ID {id} no encontrada");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar liga");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("importar/{competitionId}")]
        public async Task<IActionResult> ImportarLigas(string competitionId)
        {
            if (string.IsNullOrWhiteSpace(competitionId))
            {
                _logger.LogWarning("CompetitionId nulo o vacío");
                return BadRequest("ID de competición inválido");
            }

            try
            {
                await _handler.ImportLeaguesAsync(competitionId);
                return Ok($"Importación de ligas de competición {competitionId} completada con éxito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al importar ligas para competición {competitionId}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    }
}
