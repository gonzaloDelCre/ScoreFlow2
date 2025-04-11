using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Ports.Teams
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(TeamID teamId);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task<bool> DeleteAsync(TeamID teamId);
    }
}
