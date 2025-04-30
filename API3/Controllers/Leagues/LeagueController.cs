using Application.Leagues.DTOs;
using Application.Leagues.UseCases;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leagues
{
    [ApiController]
    [Route("api/leagues")]
    public class LeagueController : ControllerBase
    {
        private readonly LeagueUseCaseHandler _useCaseHandler;

        public LeagueController(LeagueUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        /// <summary>
        /// Get All Leagues
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllLeagues()
        {
            var result = await _useCaseHandler.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get League By Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeagueById(int id)
        {
            var result = await _useCaseHandler.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create League
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateLeague([FromBody] LeagueRequestDTO leagueDTO)
        {
            if (leagueDTO == null) return BadRequest("League data is required.");

            var result = await _useCaseHandler.CreateAsync(leagueDTO);
            return CreatedAtAction(nameof(GetLeagueById), new { id = result.LeagueID }, result);
        }

        /// <summary>
        /// Update League
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeague(int id, [FromBody] LeagueRequestDTO leagueDTO)
        {
            if (leagueDTO == null) return BadRequest("League data is required.");

            // Aseguramos que el DTO lleva el mismo ID de ruta
            leagueDTO.LeagueID = id;
            var result = await _useCaseHandler.UpdateAsync(leagueDTO);
            return Ok(result);
        }

        /// <summary>
        /// Delete League
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            await _useCaseHandler.DeleteAsync(id);
            return NoContent();
        }
    }
}
