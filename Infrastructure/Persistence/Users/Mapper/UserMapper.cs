using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Users.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Users.Mapper
{
    public class UserMapper
    {
        public UserEntity MapToEntity(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (user.FullName == null || user.Email == null || user.PasswordHash == null)
            {
                throw new ArgumentException("One or more required properties are null.");
            }

            return new UserEntity
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                PasswordHash = user.PasswordHash.Value,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }

        public User MapToDomain(UserEntity entity)
        {
            return new User(
                new UserID(entity.UserID),                      
                new UserFullName(entity.FullName),             
                new UserEmail(entity.Email),                   
                new UserPasswordHash(entity.PasswordHash),     
                Enum.Parse<UserRole>(entity.Role),              
                entity.CreatedAt                              
            );
        }
    }
}
