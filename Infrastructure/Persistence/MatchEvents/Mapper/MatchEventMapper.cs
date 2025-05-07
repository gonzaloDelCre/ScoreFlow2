using System;
using Domain.Entities.MatchEvents;
using Domain.Shared;
using Infrastructure.Persistence.MatchEvents.Entities;
using DEnum = Domain.Enum.EventType;

namespace Infrastructure.Persistence.MatchEvents.Mapper
{
    public class MatchEventMapper : IMatchEventMapper
    {
        public MatchEventEntity MapToEntity(MatchEvent domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new MatchEventEntity
            {
                ID = domain.MatchEventID.Value,
                MatchID = domain.MatchID.Value,
                PlayerID = domain.PlayerID?.Value,
                EventType = (DEnum)domain.EventType,
                Minute = domain.Minute,
                CreatedAt = domain.CreatedAt
            };
        }

        public MatchEvent MapToDomain(
            MatchEventEntity entity,
            Domain.Entities.Matches.Match match,
            Domain.Entities.Players.Player? player)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (match == null) throw new ArgumentNullException(nameof(match));

            return new MatchEvent(
                new MatchEventID(entity.ID),
                new MatchID(entity.MatchID),
                entity.PlayerID.HasValue
                    ? new PlayerID(entity.PlayerID.Value)
                    : (PlayerID?)null,
                (DEnum)entity.EventType,
                entity.Minute,
                match,
                player,
                entity.CreatedAt
            );
        }
    }
}
