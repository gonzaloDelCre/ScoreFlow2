using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.UseCases;
using Application.Playes.DTOs;
using Application.Playes.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers.Players
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly GeneralPlayerStatisticsUseCaseHandler _generalPlayerUseCaseHandler;

        public PlayerController(GeneralPlayerStatisticsUseCaseHandler generalPlayerUseCaseHandler)
        {
            _generalPlayerUseCaseHandler = generalPlayerUseCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> HandlePlayerAction([FromBody] PlayerStatisticActionDTO actionDTO)
        {
            try
            {
                var result = await _generalPlayerUseCaseHandler.Execute(actionDTO);
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
