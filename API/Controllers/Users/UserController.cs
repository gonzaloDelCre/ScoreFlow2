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
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly GetAllUsersUseCase _getAllUsersUseCase;
        private readonly GetUserByIdUseCase _getUserByIdUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;
        private readonly GeneralUserUseCase _generalUserUseCase;

        public UserController(
            CreateUserUseCase createUserUseCase,
            GetAllUsersUseCase getAllUsersUseCase,
            GetUserByIdUseCase getUserByIdUseCase,
            UpdateUserUseCase updateUserUseCase,
            DeleteUserUseCase deleteUserUseCase,
            GeneralUserUseCase generalUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _generalUserUseCase = generalUserUseCase;
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost("Crear Usuario")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("User data is required.");
            }

            var user = await _generalUserUseCase.ExecuteAsync(userDTO);

            return Ok(user);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recoger Usuarios")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _getAllUsersUseCase.ExecuteAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("Recoger Usuario por ID")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var userDTO = await _getUserByIdUseCase.ExecuteAsync(userId);
            if (userDTO == null)
            {
                return NotFound();
            }
            return Ok(userDTO);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPut("Actualizar Usuario")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDTO userDTO)
        {
            userDTO.UserID = userId;
            var updatedUser = await _updateUserUseCase.ExecuteAsync(userDTO);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("Eliminar un usuario")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _deleteUserUseCase.ExecuteAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
