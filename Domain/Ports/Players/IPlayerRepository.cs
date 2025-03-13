using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Players
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(int playerId);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId);
        Task<Player> AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task<bool> DeleteAsync(int playerId);
    }
}
