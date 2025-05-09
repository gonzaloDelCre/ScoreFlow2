using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;
using System;

namespace Application.Users.Mapper
{
    public static class UserMapper
    {
        public static UserResponseDTO ToDTO(this User u)
            => new UserResponseDTO
            {
                ID = u.UserID.Value,
                FullName = u.FullName.Value,
                Email = u.Email.Value,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            };

        public static User ToDomain(this UserRequestDTO dto)
            => new User(
                userID: new UserID(dto.ID ?? 0),
                fullName: new UserFullName(dto.FullName),
                email: new UserEmail(dto.Email),
                passwordHash: new UserPasswordHash(dto.PasswordHash),
                role: dto.Role,
                createdAt: DateTime.UtcNow
            );
    }
}
