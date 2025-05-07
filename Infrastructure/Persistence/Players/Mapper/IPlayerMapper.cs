using Domain.Entities.Players;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Players.Mapper
{
    public interface IPlayerMapper
    {
        PlayerEntity ToEntity(Player domain);
        Player ToDomain(PlayerEntity entity, IEnumerable<TeamPlayerEntity> teamPlayers);
    }
}
