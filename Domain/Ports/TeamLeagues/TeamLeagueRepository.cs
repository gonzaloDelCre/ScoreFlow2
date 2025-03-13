using Domain.Entities.TeamLeagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.TeamLeagues
{
    public interface ITeamLeagueRepository
    {
        Task<TeamLeague?> GetByIdsAsync(int teamId, int leagueId);
        Task<IEnumerable<TeamLeague>> GetAllAsync();
        Task<TeamLeague> AddAsync(TeamLeague teamLeague);
        Task UpdateAsync(TeamLeague teamLeague);
        Task<bool> DeleteAsync(int teamId, int leagueId);
    }
}
