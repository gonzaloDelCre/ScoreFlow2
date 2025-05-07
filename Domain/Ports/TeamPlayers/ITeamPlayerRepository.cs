using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Ports.TeamPlayers
{
    public interface ITeamPlayerRepository
    {
        Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId);
        Task<IEnumerable<TeamPlayer>> GetAllAsync();
        Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId);
        Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId);
        Task<TeamPlayer> AddAsync(TeamPlayer teamPlayer);
        Task UpdateAsync(TeamPlayer teamPlayer);
        Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId);

        Task<IEnumerable<TeamPlayer>> GetByRoleAsync(RoleInTeam role);
        Task<IEnumerable<TeamPlayer>> GetByJoinDateRangeAsync(DateTime from, DateTime to);
    }
}
