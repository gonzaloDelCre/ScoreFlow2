using Domain.Entities.Matches;
using Domain.Entities.MatchEvents;
using Domain.Entities.Players;
using Infrastructure.Persistence.MatchEvents.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.MatchEvents.Mapper
{
    public interface IMatchEventMapper
    {
        MatchEventEntity MapToEntity(MatchEvent domain);
        MatchEvent MapToDomain(MatchEventEntity entity, Match match, Player? player);
    }
}
