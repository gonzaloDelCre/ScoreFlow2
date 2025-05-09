using Application.Users.DTOs;
using Application.Users.UseCases;
using Domain.Enum;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API3.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserUseCaseHandler _handler;

        public UserController(
            ILogger<UserController> logger,
            UserUseCaseHandler handler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            try
            {
                var list = await _handler.GetAllUsersAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _handler.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"Usuario con ID {id} no encontrado");
                    return NotFound($"Usuario con ID {id} no encontrado");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene usuarios por rol
        /// </summary>
        [HttpGet("por-role/{role}")]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsersByRole(UserRole role)
        {
            try
            {
                var list = await _handler.GetUsersByRoleAsync(role);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios por rol");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        [HttpPost("crear")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de creación de usuario con datos nulos");
                return BadRequest("Datos del usuario inválidos");
            }

            try
            {
                var created = await _handler.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = created.ID }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Registra (sign-up) un usuario
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de registro con datos nulos");
                return BadRequest("Datos de registro inválidos");
            }

            try
            {
                var registered = await _handler.RegisterUserAsync(dto);
                if (registered == null)
                {
                    _logger.LogWarning("Registro fallido: email ya existente");
                    return BadRequest("No se pudo registrar el usuario");
                }
                return Ok(registered);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al registrar usuario");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Inicia sesión de usuario
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de login con datos nulos");
                return BadRequest("Datos de login inválidos");
            }

            try
            {
                var user = await _handler.LoginUserAsync(dto);
                if (user == null)
                {
                    _logger.LogWarning("Credenciales inválidas");
                    return Unauthorized("Email o contraseña incorrectos");
                }
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDTO dto)
        {
            if (dto == null || dto.ID != id)
            {
                _logger.LogWarning("ID en ruta no coincide o datos nulos");
                return BadRequest("ID del usuario no coincide o datos inválidos");
            }

            try
            {
                var updated = await _handler.UpdateUserAsync(dto);
                if (updated == null)
                {
                    _logger.LogWarning($"Usuario con ID {id} no encontrado");
                    return NotFound($"Usuario con ID {id} no encontrado");
                }
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación al actualizar usuario");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequestDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Petición de cambio de contraseña con datos nulos");
                return BadRequest("Datos de cambio de contraseña inválidos");
            }

            try
            {
                var ok = await _handler.ChangeUserPasswordAsync(id, dto.CurrentPasswordHash, dto.NewPasswordHash);
                if (!ok)
                {
                    _logger.LogWarning($"Cambio de contraseña inválido para usuario {id}");
                    return BadRequest("No se pudo cambiar la contraseña");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un usuario por su ID
        /// </summary>
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var ok = await _handler.DeleteUserAsync(id);
                if (!ok)
                {
                    _logger.LogWarning($"Usuario con ID {id} no encontrado o no pudo ser eliminado");
                    return NotFound($"Usuario con ID {id} no encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
