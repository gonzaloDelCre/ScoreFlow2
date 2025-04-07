using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Users.Mapper
{
    public static class UserMapperExtensions
    {
        public static User MapToDomain(this UserEntity entity)
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
