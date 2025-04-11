using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.TeamPlayers;
using Domain.Shared;

namespace Domain.Ports.TeamPlayers
{
    public interface ITeamPlayerRepository
    {
        Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId);
        Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId);
        Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId);
        Task<TeamPlayer> AddAsync(TeamPlayer teamPlayer);
        Task UpdateAsync(TeamPlayer teamPlayer);
        Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId);
    }
}
