using Application.Leagues.DTOs;
using Application.Leagues.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers.Leagues
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly GeneralLeagueUseCaseHandler _GeneralLeagueUseCaseHandler;

        public LeagueController(GeneralLeagueUseCaseHandler GeneralLeagueUseCaseHandler)
        {
            _GeneralLeagueUseCaseHandler = GeneralLeagueUseCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> HandleLeagueAction([FromBody] LeagueActionDTO actionDTO)
        {
            try
            {
                var result = await _GeneralLeagueUseCaseHandler.Execute(actionDTO);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error en el servidor", details = ex.Message });
            }
        }
    }
}
