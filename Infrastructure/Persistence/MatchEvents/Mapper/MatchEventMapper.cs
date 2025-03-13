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
    public class MatchEventMapper
    {
        public MatchEventEntity MapToEntity(MatchEvent matchEvent)
        {
            return new MatchEventEntity
            {
                EventID = matchEvent.EventID,
                MatchID = matchEvent.Match.MatchID,
                PlayerID = matchEvent.Player?.PlayerID,
                EventType = matchEvent.EventType,
                Minute = matchEvent.Minute,
                CreatedAt = matchEvent.CreatedAt
            };
        }

        public MatchEvent MapToDomain(MatchEventEntity entity, Match match, Player? player)
        {
            return new MatchEvent(
                entity.EventID,
                match,
                player,
                entity.EventType,
                entity.Minute,
                entity.CreatedAt
            );
        }
    }
}
