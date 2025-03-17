using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.PlayerStatistics
{
    public class PlayerStatisticService
    {
        private readonly IPlayerStatisticRepository _playerStatisticRepository;
        private readonly ILogger<PlayerStatisticService> _logger;

        public PlayerStatisticService(IPlayerStatisticRepository playerStatisticRepository, ILogger<PlayerStatisticService> logger)
        {
            _playerStatisticRepository = playerStatisticRepository;
            _logger = logger;
        }

        public async Task<PlayerStatistic> CreatePlayerStatisticAsync(
            PlayerID playerId, MatchID matchId, Goals goals, Assists assists,
            YellowCards yellowCards, RedCards redCards, int minutesPlayed, DateTime createdAt)
        {
            if (createdAt == DateTime.MinValue)
                throw new ArgumentException("La fecha de creación es obligatoria.");

            var playerStatistic = new PlayerStatistic(
                new PlayerStatisticID(1), matchId, playerId, goals, assists, yellowCards, redCards, null, null, createdAt)
            {
                MinutesPlayed = minutesPlayed
            };

            await _playerStatisticRepository.AddAsync(playerStatistic);
            return playerStatistic;
        }

        public async Task<PlayerStatistic?> GetPlayerStatisticByIdAsync(PlayerStatisticID statId)
        {
            try
            {
                return await _playerStatisticRepository.GetByIdAsync(statId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la estadística del jugador con ID {StatID}.", statId.Value);
                throw;
            }
        }

        public async Task<IEnumerable<PlayerStatistic>> GetAllPlayerStatisticsAsync()
        {
            try
            {
                return await _playerStatisticRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las estadísticas de los jugadores.");
                throw;
            }
        }

        public async Task<IEnumerable<PlayerStatistic>> GetPlayerStatisticsByPlayerIdAsync(PlayerID playerId)
        {
            try
            {
                return await _playerStatisticRepository.GetByPlayerIdAsync(playerId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las estadísticas del jugador con ID {PlayerID}.", playerId.Value);
                throw;
            }
        }

        public async Task UpdatePlayerStatisticAsync(PlayerStatistic playerStatistic)
        {
            if (playerStatistic == null)
                throw new ArgumentNullException(nameof(playerStatistic), "La estadística del jugador no puede ser nula.");

            await _playerStatisticRepository.UpdateAsync(playerStatistic);
        }

        public async Task<bool> DeletePlayerStatisticAsync(PlayerStatisticID statId)
        {
            try
            {
                return await _playerStatisticRepository.DeleteAsync(statId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la estadística del jugador con ID {StatID}.", statId.Value);
                throw;
            }
        }
    }
}

