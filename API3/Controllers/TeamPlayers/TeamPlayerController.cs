using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace API3.Controllers.TeamPlayers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamPlayerController : ControllerBase
    {
        private readonly ILogger<TeamPlayerController> _logger;
        private readonly TeamPlayerUseCaseHandler _handler;

        public TeamPlayerController(
            ILogger<TeamPlayerController> logger,
            TeamPlayerUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Añade un jugador a un equipo
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> AddPlayerToTeam([FromBody] TeamPlayerRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de añadir jugador con datos nulos");
                return BadRequest("Datos de TeamPlayer inválidos");
            }

            try
            {
                var result = await _handler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByIds), new { teamId = result.TeamID, playerId = result.PlayerID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al añadir jugador al equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene la relación equipo-jugador por IDs
        /// </summary>
        [HttpGet("{teamId}/{playerId}")]
        public async Task<IActionResult> GetByIds(int teamId, int playerId)
        {
            try
            {
                var item = await _handler.GetByIdsAsync(teamId, playerId);
                if (item == null)
                {
                    _logger.LogWarning($"No encontrada relación para TeamID {teamId} y PlayerID {playerId}");
                    return NotFound($"Relación para TeamID {teamId} y PlayerID {playerId} no encontrada");
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener relación equipo-jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todos los jugadores de un equipo
        /// </summary>
        [HttpGet("por-team/{teamId}")]
        public async Task<IActionResult> GetByTeam(int teamId)
        {
            try
            {
                var list = await _handler.GetByTeamAsync(teamId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugadores por equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene todos los equipos de un jugador
        /// </summary>
        [HttpGet("por-player/{playerId}")]
        public async Task<IActionResult> GetByPlayer(int playerId)
        {
            try
            {
                var list = await _handler.GetByPlayerAsync(playerId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener equipos por jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza la relación jugador-equipo
        /// </summary>
        [HttpPut("actualizar/{teamId}/{playerId}")]
        public async Task<IActionResult> UpdatePlayerInTeam(int teamId, int playerId, [FromBody] TeamPlayerRequestDTO dto)
        {
            if (dto == null || dto.TeamID != teamId || dto.PlayerID != playerId)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("IDs no coinciden o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Relación con TeamID {teamId} y PlayerID {playerId} no encontrada");
                    return NotFound($"Relación con TeamID {teamId} y PlayerID {playerId} no encontrada");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar relación");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar relación equipo-jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina la relación jugador-equipo
        /// </summary>
        [HttpDelete("eliminar/{teamId}/{playerId}")]
        public async Task<IActionResult> Delete(int teamId, int playerId)
        {
            try
            {
                var ok = await _handler.DeleteAsync(teamId, playerId);
                if (!ok)
                {
                    _logger.LogWarning($"No pudo eliminarse relación para TeamID {teamId} y PlayerID {playerId}");
                    return NotFound($"Relación para TeamID {teamId} y PlayerID {playerId} no encontrada");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar relación equipo-jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("vincular-masivo/{teamId}")]
        public async Task<IActionResult> LinkPlayersToTeam(int teamId)
        {
            try
            {
                var count = await _handler.LinkPlayersToTeamAsync(teamId);
                return Ok($"✅ Se vincularon {count} jugadores al equipo {teamId}.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al vincular jugadores");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
