using Application.Users.DTOs;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Update;
using Domain.Enum;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases
{
    public class UserUseCaseHandler
    {
        private readonly CreateUserUseCase _create;
        private readonly RegisterUserUseCase _register;
        private readonly LoginUserUseCase _login;
        private readonly UpdateUserUseCase _update;
        private readonly GetAllUsersUseCase _getAll;
        private readonly GetUserByIdUseCase _getById;
        private readonly GetUsersByRoleUseCase _getByRole;
        private readonly ChangeUserPasswordUseCase _changePwd;
        private readonly DeleteUserUseCase _delete;

        public UserUseCaseHandler(
            CreateUserUseCase create,
            RegisterUserUseCase register,
            LoginUserUseCase login,
            UpdateUserUseCase update,
            GetAllUsersUseCase getAll,
            GetUserByIdUseCase getById,
            GetUsersByRoleUseCase getByRole,
            ChangeUserPasswordUseCase changePwd,
            DeleteUserUseCase delete)
        {
            _create = create;
            _register = register;
            _login = login;
            _update = update;
            _getAll = getAll;
            _getById = getById;
            _getByRole = getByRole;
            _changePwd = changePwd;
            _delete = delete;
        }

        public Task<UserResponseDTO> CreateUserAsync(UserRequestDTO dto) => _create.ExecuteAsync(dto);
        public Task<UserResponseDTO?> RegisterUserAsync(RegisterRequestDTO dto) => _register.ExecuteAsync(dto);
        public Task<UserResponseDTO?> LoginUserAsync(LoginRequestDTO dto) => _login.ExecuteAsync(dto);
        public Task<UserResponseDTO?> UpdateUserAsync(UserRequestDTO dto) => _update.ExecuteAsync(dto);
        public Task<List<UserResponseDTO>> GetAllUsersAsync() => _getAll.ExecuteAsync();
        public Task<UserResponseDTO?> GetUserByIdAsync(int id) => _getById.ExecuteAsync(id);
        public Task<List<UserResponseDTO>> GetUsersByRoleAsync(UserRole role) => _getByRole.ExecuteAsync(role);
        public Task<bool> ChangeUserPasswordAsync(int id, string currentPassword, string newPassword)
               => _changePwd.ExecuteAsync(id, currentPassword, newPassword); public Task<bool> DeleteUserAsync(int id) => _delete.ExecuteAsync(id);

    }
}
