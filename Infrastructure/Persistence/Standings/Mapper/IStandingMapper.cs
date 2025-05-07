using Domain.Entities.Standings;
using Infrastructure.Persistence.Standings.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Standings.Mapper
{
    public interface IStandingMapper
    {
        StandingEntity MapToEntity(Standing domain);
        Standing MapToDomain(StandingEntity entity, Domain.Entities.Leagues.League league, Domain.Entities.Teams.Team team);
    }
}
