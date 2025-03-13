using Domain.Entities.Users;
using Infrastructure.Persistence.Users.Entities;

namespace Infrastructure.Persistence.Users.Mapper
{
    public class UserMapper
    {
        public User MapToDomain(UserEntity entity)
        {
            return new User(
                new UserID(entity.UserID),
                new UserFullName(entity.FullName),
                new UserEmail(entity.Email),
                new UserPasswordHash(entity.PasswordHash),
                entity.CreatedAt
            );
        }

        public UserEntity MapToEntity(User domain)
        {
            return new UserEntity
            {
                UserID = domain.UserID.Value,
                FullName = domain.FullName.Value,
                Email = domain.Email.Value,
                PasswordHash = domain.PasswordHash.Value,
                CreatedAt = domain.CreatedAt
            };
        }
    }
}
