using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers.PlayerStatistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatisticsController : ControllerBase
    {
        private readonly GeneralPlayerStatisticsUseCaseHandler _generalPlayerStatisticsUseCaseHandler;

        public PlayerStatisticsController(GeneralPlayerStatisticsUseCaseHandler generalPlayerStatisticsUseCaseHandler)
        {
            _generalPlayerStatisticsUseCaseHandler = generalPlayerStatisticsUseCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> HandlePlayerStatisticsAction([FromBody] PlayerStatisticActionDTO actionDTO)
        {
            try
            {
                var result = await _generalPlayerStatisticsUseCaseHandler.Execute(actionDTO);

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
