using Domain.Entities.PlayerStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.PlayerStatistics
{
    public interface IPlayerStatisticRepository
    {
        Task<PlayerStatistic?> GetByIdAsync(int playerStatisticId);
        Task<IEnumerable<PlayerStatistic>> GetAllAsync();
        Task<IEnumerable<PlayerStatistic>> GetByPlayerIdAsync(int playerId);
        Task<PlayerStatistic> AddAsync(PlayerStatistic playerStatistic);
        Task UpdateAsync(PlayerStatistic playerStatistic);
        Task<bool> DeleteAsync(int playerStatisticId);
    }
}
