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

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _useCaseHandler.GetAllUsersAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _useCaseHandler.GetUserByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _useCaseHandler.GetUserByEmailAsync(email);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            var result = await _useCaseHandler.CreateUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = result.UserID }, result);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null) return BadRequest("User data is required.");

            var result = await _useCaseHandler.UpdateUserAsync(id, userDTO);
            return Ok(result);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _useCaseHandler.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null)
                return BadRequest("Invalid login request.");

            try
            {
                var userResponse = await _useCaseHandler.LoginUserAsync(loginRequest.Email, loginRequest.Password);
                return Ok(userResponse);  
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            try
            {
                var userResponse = await _useCaseHandler.RegisterUserAsync(registerRequest);
                return Ok(userResponse); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
        }

    }
}
