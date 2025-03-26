using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;
using System;

namespace Application.Users.Mapper
{
    public static class UserMapper
    {
        public static User ToDomain(UserRequestDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO), "El DTO UserRequestDTO no puede ser nulo.");

            var role = Enum.TryParse(userDTO.Role, out UserRole parsedRole) ? parsedRole : UserRole.Espectador;

            return new User(
                new UserID(0),
                new UserFullName(userDTO.FullName),
                new UserEmail(userDTO.Email),
                new UserPasswordHash(userDTO.PasswordHash),
                role,
                DateTime.UtcNow
            );
        }

        public static UserResponseDTO ToResponseDTO(User user)
        {
            return new UserResponseDTO
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
