using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Ports.Players
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(PlayerID playerId);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player?> GetByNameAsync(string name);
        Task<Player> AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task<bool> DeleteAsync(PlayerID playerId);

        Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId);

        Task<IEnumerable<Player>> GetByPositionAsync(PlayerPosition position);
        Task<IEnumerable<Player>> GetByAgeRangeAsync(int minAge, int maxAge);
        Task<IEnumerable<Player>> GetTopScorersAsync(int topN);
        Task<IEnumerable<Player>> SearchByNameAsync(string partialName);

    }
}
