using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;

namespace Application.Users.Mapper
{
    public static class UserMapper
    {
        //Transform UserDTO to User domain
        public static User ToDomain(UserDTO userDTO)
        {
            return new User(
                new UserID(userDTO.UserID),
                new UserFullName(userDTO.FullName),
                new UserEmail(userDTO.Email),
                new UserPasswordHash(userDTO.PasswordHash),
                Enum.Parse<UserRole>(userDTO.Role), 
                DateTime.Now
            );
        }

        //Transform User domain to UserDTO
        public static UserDTO ToDTO(User user)
        {
            return new UserDTO
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                PasswordHash = user.PasswordHash.Value,
                Role = user.Role.ToString() 
            };
        }
    }

}
