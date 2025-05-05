using Application.Standings.DTOs;
using Application.Standings.UseCases;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                var result = await _useCaseHandler.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // logging omitted for brevity
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Create a new standing
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StandingRequestDTO dto)
        {
            if (dto == null)
                return BadRequest("Standing data is required.");

            try
            {
                var created = await _useCaseHandler.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.StandingID }, created);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (InvalidOperationException ioe)
            {
                return Conflict(ioe.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update an existing standing
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StandingRequestDTO dto)
        {
            if (dto == null)
                return BadRequest("Standing data is required.");
            if (id != dto.StandingID)
                return BadRequest("ID in URL and payload do not match.");

            try
            {
                var updated = await _useCaseHandler.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (InvalidOperationException ioe)
            {
                return Conflict(ioe.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete a standing
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _useCaseHandler.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all standings for a given league
        /// </summary>
        [HttpGet("league/{leagueId}")]
        public async Task<IActionResult> GetByLeague(int leagueId)
        {
            try
            {
                var list = await _useCaseHandler.GetByLeagueAsync(leagueId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get the classification (ordered standings) for a league
        /// </summary>
        [HttpGet("league/{leagueId}/classification")]
        public async Task<IActionResult> GetClassification(int leagueId)
        {
            try
            {
                var list = await _useCaseHandler.GetClassificationAsync(leagueId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get the standing for a specific team in a specific league
        /// </summary>
        [HttpGet("team/{teamId}/league/{leagueId}")]
        public async Task<IActionResult> GetByTeamAndLeague(int teamId, int leagueId)
        {
            try
            {
                var result = await _useCaseHandler.GetByTeamAndLeagueAsync(teamId, leagueId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
