using Domain.Entities.TeamPlayers;
using Infrastructure.Persistence.TeamPlayers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.TeamPlayers.Mapper
{
    public interface ITeamPlayerMapper
    {
        TeamPlayerEntity MapToEntity(TeamPlayer domain);
        TeamPlayer MapToDomain(TeamPlayerEntity entity);
    }
}
