using Application.Matches.DTOs;
using Application.Matches.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MatchController
{
    [ApiController]
    [Route("api/match")]
    public class MatchController : ControllerBase
    {
        private readonly GeneralMatchUseCaseHandler _useCaseHandler;

        public MatchController(GeneralMatchUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteMatchAction([FromBody] MatchActionDTO actionDTO)
        {
            if (actionDTO == null)
                return BadRequest("La acción es necesaria.");

            try
            {
                var result = await _useCaseHandler.Execute(actionDTO);

                if (actionDTO.Action.ToLower() == "getall" || actionDTO.Action.ToLower() == "getbyid")
                {
                    return Ok(result);  
                }

                
                return CreatedAtAction(nameof(ExecuteMatchAction), new { action = actionDTO.Action }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

