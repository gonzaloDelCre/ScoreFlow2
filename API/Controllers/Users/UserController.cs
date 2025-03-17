using Application.Users.DTOs;
using Application.Users.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GeneralUserUseCaseHandler _useCaseHandler;

        public UserController(GeneralUserUseCaseHandler useCaseHandler)
        {
            _useCaseHandler = useCaseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteUserAction([FromBody] UserActionDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("User data is required.");
            }

            try
            {
                var result = await _useCaseHandler.ExecuteAsync(userDTO);

                if (userDTO.Action.ToLower() == "getall" || userDTO.Action.ToLower() == "getbyid")
                {
                    return Ok(result);
                }
                else if (userDTO.Action.ToLower() == "add" || userDTO.Action.ToLower() == "update")
                {
                    return CreatedAtAction(nameof(ExecuteUserAction), new { action = userDTO.Action }, result);
                }
                else if (userDTO.Action.ToLower() == "delete")
                {
                    return NoContent();
                }

                return BadRequest($"Action '{userDTO.Action}' is not supported.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
