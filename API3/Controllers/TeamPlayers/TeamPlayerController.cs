using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace API3.Controllers.TeamPlayers
{
    [Route("api/team-players")]
    [ApiController]
    public class TeamPlayerController : ControllerBase
    {
        private readonly GeneralTeamPlayerUseCaseHandler _useCaseHandler;

        public TeamPlayerController(GeneralTeamPlayerUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        /// <summary>
        /// Add Player to Team
        /// </summary>
        /// <param name="teamPlayerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPlayerToTeam([FromBody] TeamPlayerRequestDTO teamPlayerDTO)
        {
            if (teamPlayerDTO == null) return BadRequest("TeamPlayer data is required.");

            var result = await _useCaseHandler.AddAsync(teamPlayerDTO);
            return CreatedAtAction(nameof(GetByIds), new { teamId = result.TeamID, playerId = result.PlayerID }, result);
        }

        /// <summary>
        /// Get Player by Team and Player Id
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{teamId}/{playerId}")]
        public async Task<IActionResult> GetByIds(int teamId, int playerId)
        {
            var result = await _useCaseHandler.GetByIdsAsync(teamId, playerId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Get All Players in a Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeamId(int teamId)
        {
            var result = await _useCaseHandler.GetByTeamIdAsync(teamId);
            return Ok(result);
        }

        /// <summary>
        /// Get All Teams for a Player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("player/{playerId}")]
        public async Task<IActionResult> GetByPlayerId(int playerId)
        {
            var result = await _useCaseHandler.GetByPlayerIdAsync(playerId);
            return Ok(result);
        }

        /// <summary>
        /// Update Player in Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <param name="teamPlayerDTO"></param>
        /// <returns></returns>
        [HttpPut("{teamId}/{playerId}")]
        public async Task<IActionResult> UpdatePlayerInTeam(int teamId, int playerId, [FromBody] TeamPlayerRequestDTO teamPlayerDTO)
        {
            if (teamPlayerDTO == null) return BadRequest("TeamPlayer data is required.");

            await _useCaseHandler.UpdateAsync(teamPlayerDTO, teamId, playerId);
            return NoContent();
        }

        /// <summary>
        /// Delete Player from Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpDelete("{teamId}/{playerId}")]
        public async Task<IActionResult> DeletePlayerFromTeam(int teamId, int playerId)
        {
            var result = await _useCaseHandler.DeleteAsync(teamId, playerId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
