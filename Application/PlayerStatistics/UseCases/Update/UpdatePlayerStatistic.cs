using Application.PlayerStatistics.DTOs;
using Domain.Entities.PlayerStatistics;
using Domain.Services.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Update
{
    public class UpdatePlayerStatistic
    {
        private readonly PlayerStatisticService _playerStatisticService;

        public UpdatePlayerStatistic(PlayerStatisticService playerStatisticService)
        {
            _playerStatisticService = playerStatisticService;
        }

        public async Task<PlayerStatisticResponseDTO> ExecuteAsync(PlayerStatisticRequestDTO playerStatisticDTO, int playerStatisticId)
        {
            if (playerStatisticDTO == null)
                throw new ArgumentNullException(nameof(playerStatisticDTO), "Las estadísticas del jugador no pueden ser nulas.");

            var existingPlayerStatistic = await _playerStatisticService.GetPlayerStatisticByIdAsync(new PlayerStatisticID(playerStatisticId));
            if (existingPlayerStatistic == null)
                throw new InvalidOperationException("Las estadísticas de este jugador no existen. No se pueden actualizar.");

            existingPlayerStatistic.Update(
                new Goals(playerStatisticDTO.Goals),
                new Assists(playerStatisticDTO.Assists),
                new YellowCards(playerStatisticDTO.YellowCards),
                new RedCards(playerStatisticDTO.RedCards),
                playerStatisticDTO.MinutesPlayed.Value
            );

            await _playerStatisticService.UpdatePlayerStatisticAsync(existingPlayerStatistic);

            return new PlayerStatisticResponseDTO
            {
                PlayerStatisticID = existingPlayerStatistic.PlayerStatisticID.Value,
                PlayerID = existingPlayerStatistic.PlayerID.Value,
                MatchID = existingPlayerStatistic.MatchID.Value,
                Goals = existingPlayerStatistic.Goals.Value,
                Assists = existingPlayerStatistic.Assists.Value,
                YellowCards = existingPlayerStatistic.YellowCards.Value,
                RedCards = existingPlayerStatistic.RedCards.Value,
                MinutesPlayed = existingPlayerStatistic.MinutesPlayed,
                CreatedAt = existingPlayerStatistic.CreatedAt
            };
        }
    }
}
