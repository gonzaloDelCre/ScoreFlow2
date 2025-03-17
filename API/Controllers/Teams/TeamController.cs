using Application.Teams.DTOs;
using Application.Teams.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Teams
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly GeneralTeamUseCaseHandler _useCaseHandler;

        public TeamController(GeneralTeamUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteTeamAction([FromBody] TeamActionDTO actionDTO)
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

                return CreatedAtAction(nameof(ExecuteTeamAction), new { action = actionDTO.Action }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
