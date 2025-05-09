using Application.Teams.DTOs;
using Application.Teams.UseCases;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace API3.Controllers.Teams
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;
        private readonly TeamUseCaseHandler _handler;

        public TeamController(
            ILogger<TeamController> logger,
            TeamUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Obtiene todos los equipos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAllTeams()
        {
            try
            {
                var list = await _handler.GetAllTeamsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener equipos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un equipo por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseDTO>> GetTeamById(int id)
        {
            try
            {
                var team = await _handler.GetTeamByIdAsync(id);
                if (team == null)
                {
                    _logger.LogWarning($"Equipo con ID {id} no encontrado");
                    return NotFound($"Equipo con ID {id} no encontrado");
                }
                return Ok(team);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener equipo por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo equipo
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de equipo con datos nulos");
                return BadRequest("Datos del equipo inválidos");
            }

            try
            {
                var result = await _handler.CreateTeamAsync(dto);
                return CreatedAtAction(nameof(GetTeamById), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un equipo existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID del equipo no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateTeamAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Equipo con ID {id} no encontrado");
                    return NotFound($"Equipo con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar equipo");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un equipo por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var ok = await _handler.DeleteTeamAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Equipo con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Equipo con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar equipo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarEquipos()
        {
            try
            {
                await _handler.ImportTeamsAsync();
                return Ok("Importación completada con éxito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar standings");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    }
}