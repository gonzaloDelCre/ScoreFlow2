using Application.Leagues.DTOs;
using Application.Leagues.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Leagues
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

        // POST api/league
        [HttpPost]
        public async Task<IActionResult> HandleLeagueAction([FromBody] LeagueActionDTO actionDTO)
        {
            try
            {
                // Llamar al UseCase Handler
                var result = await _GeneralLeagueUseCaseHandler.Execute(actionDTO);

                // Devolver respuesta con el resultado de la acción
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // Manejar excepciones personalizadas de validación
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                return StatusCode(500, new { message = "Ocurrió un error en el servidor", details = ex.Message });
            }
        }
    }
}
