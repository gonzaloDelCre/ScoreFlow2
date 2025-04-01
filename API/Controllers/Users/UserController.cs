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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _useCaseHandler.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _useCaseHandler.GetUserByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _useCaseHandler.GetUserByEmailAsync(email);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            var result = await _useCaseHandler.CreateUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = result.UserID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            var result = await _useCaseHandler.UpdateUserAsync(id, userDTO);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _useCaseHandler.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null) return BadRequest("Invalid login request.");

            var userResponse = await _useCaseHandler.LoginUserAsync(loginRequest.Email, loginRequest.Password);

            return userResponse != null ? Ok(userResponse) : Unauthorized("Invalid email or password.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            try
            {
                var userResponse = await _useCaseHandler.RegisterUserAsync(registerRequest);
                return Ok(userResponse); // Usuario registrado con éxito
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Enviar mensaje de error
            }
        }

    }
}
