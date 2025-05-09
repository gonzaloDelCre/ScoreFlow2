using Application.Playes.DTOs;
using Application.Playes.UseCases;
using Domain.Enum;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace API3.Controllers.Players
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly PlayerUseCaseHandler _handler;

        public PlayerController(
            ILogger<PlayerController> logger,
            PlayerUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Obtiene todos los jugadores
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetAllPlayers()
        {
            try
            {
                var list = await _handler.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugadores");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un jugador por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerResponseDTO>> GetPlayerById(int id)
        {
            try
            {
                var player = await _handler.GetByIdAsync(id);
                if (player == null)
                {
                    _logger.LogWarning($"Jugador con ID {id} no encontrado");
                    return NotFound($"Jugador con ID {id} no encontrado");
                }
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugador por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo jugador
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de jugador con datos nulos");
                return BadRequest("Datos del jugador inválidos");
            }

            try
            {
                var result = await _handler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetPlayerById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un jugador existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID del jugador no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Jugador con ID {id} no encontrado");
                    return NotFound($"Jugador con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar jugador");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un jugador por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                var ok = await _handler.DeleteAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Jugador con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Jugador con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al eliminar jugador");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar jugador");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene jugadores por nombre
        /// </summary>
        [HttpGet("por-name/{name}")]
        public async Task<ActionResult<PlayerResponseDTO>> GetByName(string name)
        {
            try
            {
                var player = await _handler.GetByNameAsync(name);
                if (player == null)
                {
                    _logger.LogWarning($"Jugador con nombre '{name}' no encontrado");
                    return NotFound($"Jugador con nombre '{name}' no encontrado");
                }
                return Ok(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugador por nombre");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene jugadores de un equipo por ExternalId
        /// </summary>
        [HttpGet("por-team/{teamId}")]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByTeam(int teamId)
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
        /// Obtiene jugadores por posición
        /// </summary>
        [HttpGet("por-position/{position}")]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByPosition(PlayerPosition position)
        {
            try
            {
                var list = await _handler.GetByPositionAsync(position);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugadores por posición");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene jugadores por rango de edad
        /// </summary>
        [HttpGet("por-age-range")]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetByAgeRange([FromQuery] int minAge, [FromQuery] int maxAge)
        {
            try
            {
                var list = await _handler.GetByAgeRangeAsync(minAge, maxAge);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugadores por rango de edad");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los máximos goleadores
        /// </summary>
        [HttpGet("top-scorers/{topN}")]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> GetTopScorers(int topN)
        {
            try
            {
                var list = await _handler.GetTopScorersAsync(topN);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener máximos goleadores");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Busca jugadores por nombre parcial
        /// </summary>
        [HttpGet("search/{partialName}")]
        public async Task<ActionResult<IEnumerable<PlayerResponseDTO>>> SearchByName(string partialName)
        {
            try
            {
                var list = await _handler.SearchByNameAsync(partialName);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar jugadores por nombre");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Inicia el scraping/manual import de jugadores para un equipo
        /// </summary>
        [HttpPost("scrape/{teamId}")]
        public async Task<IActionResult> ScrapePlayers(int teamId)
        {
            try
            {
                await _handler.ImportByTeamExternalIdAsync(teamId);
                return Ok("Jugadores importados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar jugadores");
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}
