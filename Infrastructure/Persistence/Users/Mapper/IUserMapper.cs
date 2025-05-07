using Domain.Entities.Users;
using Infrastructure.Persistence.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Users.Mapper
{
    public interface IUserMapper
    {
        UserEntity ToEntity(User domain);
        User ToDomain(UserEntity entity);
    }
}
