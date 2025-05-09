using Application.MatchEvents.DTOs;
using Domain.Entities.MatchEvents;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.Mappers
{
    public static class MatchEventMapper
    {
        public static MatchEventResponseDTO ToDTO(this MatchEvent e)
            => new MatchEventResponseDTO
            {
                ID = e.MatchEventID.Value,
                MatchID = e.MatchID.Value,
                PlayerID = e.PlayerID?.Value,
                PlayerName = e.Player?.Name.Value,
                EventType = e.EventType,
                Minute = e.Minute,
                CreatedAt = e.CreatedAt
            };

        public static MatchEvent ToDomain(this MatchEventRequestDTO dto)
            => new MatchEvent(
                matchEventID: new MatchEventID(dto.ID ?? 0),
                matchID: new MatchID(dto.MatchID),
                playerID: dto.PlayerID.HasValue ? new PlayerID(dto.PlayerID.Value) : null,
                eventType: dto.EventType,
                minute: dto.Minute,
                match: null!,
                player: null!
            );
    }
}
