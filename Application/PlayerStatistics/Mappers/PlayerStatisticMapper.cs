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
    public class PlayerStatisticMapper
    {
        public PlayerStatisticResponseDTO MapToDTO(PlayerStatistic playerStatistic)
        {
            if (playerStatistic == null)
                throw new ArgumentNullException(nameof(playerStatistic), "La entidad de dominio PlayerStatistic no puede ser nula.");

            return new PlayerStatisticResponseDTO
            {
                PlayerStatisticID = playerStatistic.PlayerStatisticID.Value,
                MatchID = playerStatistic.MatchID.Value,
                PlayerID = playerStatistic.PlayerID.Value,
                Goals = playerStatistic.Goals.Value,
                Assists = playerStatistic.Assists.Value,
                YellowCards = playerStatistic.YellowCards.Value,
                RedCards = playerStatistic.RedCards.Value,
                MinutesPlayed = playerStatistic.MinutesPlayed,
                CreatedAt = playerStatistic.CreatedAt
            };
        }

        public PlayerStatistic MapToDomain(PlayerStatisticRequestDTO playerStatisticDTO)
        {
            if (playerStatisticDTO == null)
                throw new ArgumentNullException(nameof(playerStatisticDTO), "El DTO PlayerStatisticRequestDTO no puede ser nulo.");

            var playerStatisticID = new PlayerStatisticID(1);  
            var matchID = new MatchID(playerStatisticDTO.MatchID);
            var playerID = new PlayerID(playerStatisticDTO.PlayerID);
            var goals = new Goals(playerStatisticDTO.Goals);
            var assists = new Assists(playerStatisticDTO.Assists);
            var yellowCards = new YellowCards(playerStatisticDTO.YellowCards);
            var redCards = new RedCards(playerStatisticDTO.RedCards);
            var createdAt = playerStatisticDTO.CreatedAt;

            return new PlayerStatistic(
                playerStatisticID,
                matchID,
                playerID,
                goals,
                assists,
                yellowCards,
                redCards,
                null,  
                null, 
                createdAt,
                playerStatisticDTO.MinutesPlayed
            );
        }
    }
}
