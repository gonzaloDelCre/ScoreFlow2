using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Ports.Players
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(PlayerID playerId);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<IEnumerable<Player>> GetByTeamIdAsync(TeamID teamId);
        Task<Player?> GetByNameAsync(string playerName);
        Task<Player> AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task<bool> DeleteAsync(PlayerID playerId);
    }
}
