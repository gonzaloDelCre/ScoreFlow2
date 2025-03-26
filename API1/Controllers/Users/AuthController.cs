using Application.Users.DTOs;
using Domain.Enum;
using Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers.Users
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return BadRequest("Los datos de registro son obligatorios.");
            }

            if (string.IsNullOrEmpty(registerDTO.Password))
            {
                return BadRequest("La contraseña es obligatoria.");
            }

            try
            {
                UserRole role = UserRole.Espectador;
                if (!string.IsNullOrEmpty(registerDTO.Role) && Enum.TryParse(registerDTO.Role, out UserRole parsedRole))
                {
                    role = parsedRole;
                }

                var user = await _userService.RegisterUserAsync(
                    registerDTO.FullName,
                    registerDTO.Email,
                    registerDTO.Password,
                    role
                );

                var response = new AuthResponseDTO
                {
                    UserID = user.UserID.Value,
                    FullName = user.FullName.Value,
                    Email = user.Email.Value,
                    Role = user.Role.ToString(),
                    Token = "" // El registro no devuelve token, requiere login
                };

                return CreatedAtAction(nameof(Login), response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al registrar usuario: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Los datos de inicio de sesión son obligatorios.");
            }

            try
            {
                var (user, token) = await _userService.LoginAsync(loginDTO.Email, loginDTO.Password);

                var response = new AuthResponseDTO
                {
                    UserID = user.UserID.Value,
                    FullName = user.FullName.Value,
                    Email = user.Email.Value,
                    Role = user.Role.ToString(),
                    Token = token
                };

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al iniciar sesión: {ex.Message}");
            }
        }
    }
}
