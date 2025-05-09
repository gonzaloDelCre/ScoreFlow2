using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Ports.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Create
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _repo;
        public RegisterUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDTO?> ExecuteAsync(RegisterRequestDTO dto)
        {
            if (await _repo.ExistsByEmailAsync(dto.Email))
                return null;

            var userRequest = new UserRequestDTO
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                Role = dto.Role
            };

            var domain = userRequest.ToDomain();
            var registered = await _repo.AddAsync(domain);
            return registered.ToDTO();
        }
    }
}
