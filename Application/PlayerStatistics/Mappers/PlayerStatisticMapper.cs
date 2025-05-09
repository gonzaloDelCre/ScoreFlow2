using Application.PlayerStatistics.DTOs;
using Application.Playes.DTOs;
using Domain.Entities.Players;
using Domain.Entities.PlayerStatistics;
using Domain.Enum;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.Mappers
{
    public static class PlayerStatisticMapper
    {
        public static PlayerStatisticResponseDTO ToDTO(this PlayerStatistic s)
            => new PlayerStatisticResponseDTO
            {
                ID = s.PlayerStatisticID.Value,
                MatchID = s.MatchID.Value,
                PlayerID = s.PlayerID.Value,
                PlayerName = s.Player.Name.Value,
                Goals = s.Goals.Value,
                Assists = s.Assists.Value,
                YellowCards = s.YellowCards.Value,
                RedCards = s.RedCards.Value,
                MinutesPlayed = s.MinutesPlayed,
                CreatedAt = s.CreatedAt
            };

        public static PlayerStatistic ToDomain(this PlayerStatisticRequestDTO dto)
        {
            return new PlayerStatistic(
                playerStatisticID: new PlayerStatisticID(dto.ID ?? 0),
                matchID: new MatchID(dto.MatchID),
                playerID: new PlayerID(dto.PlayerID),
                goals: new Goals(dto.Goals),
                assists: new Assists(dto.Assists),
                yellowCards: new YellowCards(dto.YellowCards),
                redCards: new RedCards(dto.RedCards),
                match: null!,
                player: null!,
                createdAt: null,
                minutesPlayed: dto.MinutesPlayed
            );
        }
    }
}
