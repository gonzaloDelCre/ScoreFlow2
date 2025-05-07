using Domain.Entities.PlayerStatistics;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.PlayerStatistics
{
    public interface IPlayerStatisticRepository
    {
        Task<PlayerStatistic?> GetByIdAsync(PlayerStatisticID statId);
        Task<IEnumerable<PlayerStatistic>> GetAllAsync();
        Task<IEnumerable<PlayerStatistic>> GetByPlayerIdAsync(PlayerID playerId);
        Task<IEnumerable<PlayerStatistic>> GetByMatchIdAsync(MatchID matchId);
        Task<IEnumerable<PlayerStatistic>> GetByGoalsRangeAsync(int minGoals, int maxGoals);
        Task<IEnumerable<PlayerStatistic>> GetByAssistsRangeAsync(int minAssists, int maxAssists);
        Task<PlayerStatistic> AddAsync(PlayerStatistic stat);
        Task UpdateAsync(PlayerStatistic stat);
        Task<bool> DeleteAsync(PlayerStatisticID statId);
    }
}
