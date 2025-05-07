using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Infrastructure.Persistence.Teams.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Teams.Mapper
{
    public interface ITeamMapper
    {
        TeamEntity ToEntity(Team domain);
        Team ToDomain(
                                    TeamEntity entity,
                                    IEnumerable<Player> players,
                                    IEnumerable<Standing> standings);
    }
}
