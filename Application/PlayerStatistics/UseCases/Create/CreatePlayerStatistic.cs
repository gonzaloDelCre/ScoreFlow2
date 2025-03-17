using Application.PlayerStatistics.DTOs;
using Domain.Entities.Players;
using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Services.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Create
{
    public class CreatePlayerStatistic
    {
        private readonly PlayerStatisticService _playerStatisticService;

        public CreatePlayerStatistic(PlayerStatisticService playerStatisticService)
        {
            _playerStatisticService = playerStatisticService;
        }

        public async Task<PlayerStatisticResponseDTO> ExecuteAsync(PlayerStatisticRequestDTO playerStatisticDTO)
        {
            if (playerStatisticDTO == null)
                throw new ArgumentNullException(nameof(playerStatisticDTO), "Las estadísticas del jugador no pueden ser nulas.");

            int minutesPlayed = playerStatisticDTO.MinutesPlayed ?? 0;  

            var playerStatistic = new PlayerStatistic(
                new PlayerStatisticID(1),
                new PlayerID(playerStatisticDTO.PlayerID),
                new MatchID(playerStatisticDTO.MatchID), 
                new Goals(playerStatisticDTO.Goals),
                new Assists(playerStatisticDTO.Assists),
                new YellowCards(playerStatisticDTO.YellowCards),
                new RedCards(playerStatisticDTO.RedCards),
                playerStatisticDTO.CreatedAt,
                minutesPlayed 
            );

            var createdPlayerStatistic = await _playerStatisticService.CreatePlayerStatisticAsync(
                playerStatistic.PlayerID,
                playerStatistic.MatchID,  
                playerStatistic.Goals,
                playerStatistic.Assists,
                playerStatistic.YellowCards,
                playerStatistic.RedCards,
                minutesPlayed,  
                playerStatistic.CreatedAt
            );

            return new PlayerStatisticResponseDTO
            {
                PlayerStatisticID = createdPlayerStatistic.PlayerStatisticID.Value,
                PlayerID = createdPlayerStatistic.PlayerID.Value,
                MatchID = createdPlayerStatistic.MatchID.Value,
                Goals = createdPlayerStatistic.Goals.Value,
                Assists = createdPlayerStatistic.Assists.Value,
                YellowCards = createdPlayerStatistic.YellowCards.Value,
                RedCards = createdPlayerStatistic.RedCards.Value,
                MinutesPlayed = createdPlayerStatistic.MinutesPlayed,
                CreatedAt = createdPlayerStatistic.CreatedAt
            };
        }
    }


}
