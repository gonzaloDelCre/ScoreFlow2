using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Domain.Ports.Teams
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(TeamID teamId);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByExternalIdAsync(string externalId);
        Task<IEnumerable<Team>> GetByCategoryAsync(string category);
        Task<IEnumerable<Team>> SearchByNameAsync(string partialName);
        Task<Team> AddAsync(Team team);
        Task UpdateAsync(Team team);
        Task<bool> DeleteAsync(TeamID teamId);
        Task<IEnumerable<Player>> GetPlayersAsync(TeamID teamId);
        Task<IEnumerable<Standing>> GetStandingsAsync(TeamID teamId);
        Task<IEnumerable<Team>> GetByClubAsync(string club);
        Task<IEnumerable<Team>> GetByStadiumAsync(string stadium);

    }
}
