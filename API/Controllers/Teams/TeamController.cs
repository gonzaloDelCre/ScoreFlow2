using Application.Teams.DTOs;
using Application.Teams.UseCases;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Teams
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly GeneralTeamUseCaseHandler _useCaseHandler;

        public TeamController(GeneralTeamUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        /// <summary>
        /// Get All Teams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _useCaseHandler.GetAllTeamsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get Team By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var result = await _useCaseHandler.GetTeamByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create Team
        /// </summary>
        /// <param name="teamDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamRequestDTO teamDTO)
        {
            if (teamDTO == null) return BadRequest("Team data is required.");

            var result = await _useCaseHandler.CreateTeamAsync(teamDTO);
            return CreatedAtAction(nameof(GetTeamById), new { id = result.TeamID }, result);
        }

        /// <summary>
        /// Update Team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teamDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamRequestDTO teamDTO)
        {
            if (teamDTO == null) return BadRequest("Team data is required.");

            var result = await _useCaseHandler.UpdateTeamAsync(id, teamDTO);
            return Ok(result);
        }

        /// <summary>
        /// Delete Team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            await _useCaseHandler.DeleteTeamAsync(id);
            return NoContent();
        }
    }
}
