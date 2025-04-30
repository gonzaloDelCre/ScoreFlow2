using Application.Standings.DTOs;
using Application.Standings.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers.Standings
{
    [ApiController]
    [Route("api/standings")]
    public class StandingController : ControllerBase
    {
        private readonly StandingUseCaseHandler _useCaseHandler;

        public StandingController(StandingUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        /// <summary>
        /// Get all standings
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _useCaseHandler.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a single standing by its ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _useCaseHandler.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create a new standing
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StandingRequestDTO dto)
        {
            if (dto == null) return BadRequest("Standing data is required.");

            var created = await _useCaseHandler.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.StandingID }, created);
        }

        /// <summary>
        /// Update an existing standing
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StandingRequestDTO dto)
        {
            if (dto == null) return BadRequest("Standing data is required.");
            if (id != dto.StandingID) return BadRequest("ID in URL and payload do not match.");

            var updated = await _useCaseHandler.UpdateAsync(dto);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a standing
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _useCaseHandler.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get all standings for a given league
        /// </summary>
        [HttpGet("league/{leagueId}")]
        public async Task<IActionResult> GetByLeague(int leagueId)
        {
            var list = await _useCaseHandler.GetByLeagueAsync(leagueId);
            return Ok(list);
        }

        /// <summary>
        /// Get the classification (ordered standings) for a league
        /// </summary>
        [HttpGet("league/{leagueId}/classification")]
        public async Task<IActionResult> GetClassification(int leagueId)
        {
            var list = await _useCaseHandler.GetClassificationAsync(leagueId);
            return Ok(list);
        }

        /// <summary>
        /// Get the standing for a specific team in a specific league
        /// </summary>
        [HttpGet("team/{teamId}/league/{leagueId}")]
        public async Task<IActionResult> GetByTeamAndLeague(int teamId, int leagueId)
        {
            var result = await _useCaseHandler.GetByTeamAndLeagueAsync(teamId, leagueId);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
