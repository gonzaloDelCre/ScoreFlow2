using Application.Playes.DTOs;
using Application.Playes.UseCases;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace API3.Controllers.Players
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly GeneralPlayerUseCaseHandler _useCaseHandler;

        public PlayerController(GeneralPlayerUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        /// <summary>
        /// Get All Players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var result = await _useCaseHandler.GetAllPlayersAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get Player By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var result = await _useCaseHandler.GetPlayerByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create Player
        /// </summary>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerRequestDTO playerDTO)
        {
            if (playerDTO == null) return BadRequest("Player data is required.");

            var result = await _useCaseHandler.CreatePlayerAsync(playerDTO);
            return CreatedAtAction(nameof(GetPlayerById), new { id = result.PlayerID }, result);
        }

        /// <summary>
        /// Update Player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerRequestDTO playerDTO)
        {
            if (playerDTO == null) return BadRequest("Player data is required.");

            var result = await _useCaseHandler.UpdatePlayerAsync(id, playerDTO);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                await _useCaseHandler.DeletePlayerAsync(id);
                return Ok("Jugador eliminado exitosamente.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene los jugadores de un equipo dado su ExternalId
        /// </summary>
        [HttpGet("team/{externalId}")]
        public async Task<IActionResult> GetByTeam(int externalId)
        {
            var players = await _useCaseHandler.GetPlayersByTeamAsync(externalId);
            return Ok(players);
        }

        /// <summary>
        /// Inicia el scraping / import manual de jugadores para un equipo
        /// </summary>
        [HttpPost("scrape/{externalId}")]
        public async Task<IActionResult> ScrapePlayers(int externalId)
        {
            await _useCaseHandler.ScrapeAsync(externalId);
            return Ok("Jugadores importados correctamente");
        }
    }
}
