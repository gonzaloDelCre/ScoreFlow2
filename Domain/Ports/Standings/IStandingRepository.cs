using Domain.Entities.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Standings
{
    public interface IStandingRepository
    {
        Task<Standing?> GetByIdAsync(int standingId);
        Task<IEnumerable<Standing>> GetAllAsync();
        Task<IEnumerable<Standing>> GetByLeagueIdAsync(int leagueId);
        Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(int leagueId);
        Task<Standing?> GetByTeamIdAndLeagueIdAsync(int teamId, int leagueId);
        Task<Standing> AddAsync(Standing standing);
        Task UpdateAsync(Standing standing);
        Task<bool> DeleteAsync(int standingId);
    }
}
