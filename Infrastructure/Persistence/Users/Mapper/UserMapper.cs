using Domain.Entities.Users;
using Domain.Enum;
using Infrastructure.Persistence.Users.Entities;
using Domain.Shared;

namespace Infrastructure.Persistence.Users.Mapper
{
    public class UserMapper : IUserMapper
    {
        public UserEntity ToEntity(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

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

        public User ToDomain(UserEntity e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (!Enum.TryParse<UserRole>(e.Role, true, out var role))
                throw new ArgumentException("Rol almacenado inválido.", nameof(e.Role));

            return new User(
                new UserID(e.UserID),
                new UserFullName(e.FullName),
                new UserEmail(e.Email),
                new UserPasswordHash(e.PasswordHash),
                role,
                e.CreatedAt
            );
        }
    }

}
