using Domain.Entities.PlayerStatistics;
using Domain.Services.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Delete
{
    public class DeletePlayerStatistic
    {
        private readonly PlayerStatisticService _playerStatisticService;

        public DeletePlayerStatistic(PlayerStatisticService playerStatisticService)
        {
            _playerStatisticService = playerStatisticService;
        }

        public async Task<bool> ExecuteAsync(int playerStatisticId)
        {
            if (playerStatisticId <= 0)
                throw new ArgumentException("El ID de la estadística del jugador debe ser válido.");

            var playerStatistic = await _playerStatisticService.GetPlayerStatisticByIdAsync(new PlayerStatisticID(playerStatisticId));
            if (playerStatistic == null)
                throw new InvalidOperationException("Las estadísticas de este jugador no existen. No se pueden eliminar.");

            return await _playerStatisticService.DeletePlayerStatisticAsync(new PlayerStatisticID(playerStatisticId));
        }
    }
}
