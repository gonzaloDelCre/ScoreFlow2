using Application.Users.DTOs;
using Domain.Services.Users;
using Domain.Shared;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Entities.Users;

namespace API1.Controllers.Users
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!int.TryParse(id, out int userIdInt))
            {
                return BadRequest("El ID del usuario debe ser un número entero válido.");
            }

            try
            {
                var userId = new UserID(userIdInt);
                var result = await _userService.GetUserByIdAsync(userId);
                return result != null ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Los datos del usuario son obligatorios.");
            }

            if (!Enum.TryParse(userDTO.Role, out UserRole role))
            {
                return BadRequest("El rol proporcionado no es válido.");
            }

            try
            {
                var result = await _userService.CreateUserAsync(userDTO.FullName, userDTO.Email, userDTO.PasswordHash, role);
                return CreatedAtAction(nameof(GetUserById), new { id = result.UserID.Value }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserRequestDTO userDTO)
        {
            if (!int.TryParse(id, out int userIdInt))
            {
                return BadRequest("El ID del usuario debe ser un número entero válido.");
            }

            if (!Enum.TryParse(userDTO.Role, out UserRole role))
            {
                return BadRequest("El rol proporcionado no es válido.");
            }

            try
            {
                var userId = new UserID(userIdInt);
                var existingUser = await _userService.GetUserByIdAsync(userId);
                if (existingUser == null) return NotFound("Usuario no encontrado.");

                var updatedUser = new User(
                    existingUser.UserID,
                    new UserFullName(userDTO.FullName),
                    new UserEmail(userDTO.Email),
                    new UserPasswordHash(userDTO.PasswordHash),
                    role,
                    existingUser.CreatedAt
                );

                await _userService.UpdateUserAsync(updatedUser);
                return Ok("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!int.TryParse(id, out int userIdInt))
            {
                return BadRequest("El ID del usuario debe ser un número entero válido.");
            }

            try
            {
                var userId = new UserID(userIdInt);
                var success = await _userService.DeleteUserAsync(userId);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error al comunicarse con la API: {ex.Message}");
            }
        }
    }
}
