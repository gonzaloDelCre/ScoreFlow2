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
        private readonly ApiGatewayService _apiGatewayService;

        // Inyectamos el servicio ApiGatewayService
        public UserController(GeneralUserUseCaseHandler useCaseHandler, ApiGatewayService apiGatewayService)
        {
            _useCaseHandler = useCaseHandler;
            _apiGatewayService = apiGatewayService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // Llamada a la API Gateway (Lambda)
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "GET");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Llamada a la API Gateway (Lambda) para obtener un usuario por ID
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "GET", new { id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            // Llamada a la API Gateway (Lambda) para obtener un usuario por email
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "GET", new { email });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            // Llamada a la API Gateway (Lambda) para crear un usuario
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "POST", userDTO);

            return CreatedAtAction(nameof(GetUserById), new { id = userDTO.UserID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            // Llamada a la API Gateway (Lambda) para actualizar un usuario
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "PUT", new { id, userDTO });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Llamada a la API Gateway (Lambda) para eliminar un usuario
            var result = await _apiGatewayService.CallLambdaEndpointAsync("Users", "DELETE", new { id });
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }

            // Llamada a la API Gateway (Lambda) para el login
            var userResponse = await _apiGatewayService.CallLambdaEndpointAsync("Users", "POST", loginRequest);

            if (userResponse == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(userResponse);
        }
    }
}
