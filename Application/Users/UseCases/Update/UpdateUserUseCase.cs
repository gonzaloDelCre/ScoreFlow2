using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Entities.Users;
using Domain.Ports.Users;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases.Update
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _repo;
        public UpdateUserUseCase(IUserRepository repo) => _repo = repo;

        public async Task<UserResponseDTO?> ExecuteAsync(UserRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio");

            var existing = await _repo.GetByIdAsync(new UserID(dto.ID.Value));
            if (existing == null) return null;

            existing.Update(
                fullName: new UserFullName(dto.FullName),
                email: new UserEmail(dto.Email),
                passwordHash: new UserPasswordHash(dto.PasswordHash),
                role: dto.Role
            );

            await _repo.UpdateAsync(existing);
            var updated = await _repo.GetByIdAsync(new UserID(dto.ID.Value));
            return updated?.ToDTO();
        }
    }
}
