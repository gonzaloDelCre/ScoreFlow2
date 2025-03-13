using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Users.Entities;

namespace Infrastructure.Persistence.Users.Mapper
{
    public class UserMapper
    {
        //Transform User domain to UserEntity persistance
        public UserEntity MapToEntity(User user)
        {
            return new UserEntity
            {
                UserID = user.UserID.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                PasswordHash = user.PasswordHash.Value,
                CreatedAt = user.CreatedAt
            };
        }

        //Transform UserEntity persistance to User domain
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
