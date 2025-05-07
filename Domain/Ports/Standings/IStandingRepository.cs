using Domain.Entities.Standings;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Standings
{
    public interface IStandingRepository
    {
        Task<Standing?> GetByIdAsync(StandingID standingId);
        Task<IEnumerable<Standing>> GetAllAsync();
        Task<Standing> AddAsync(Standing standing);
        Task UpdateAsync(Standing standing);
        Task<bool> DeleteAsync(StandingID standingId);
        Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId);
        Task<Standing?> GetByTeamIdAndLeagueIdAsync(TeamID teamId, LeagueID leagueId);
        Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId);
        Task<IEnumerable<Standing>> GetTopByPointsAsync(int topN);
        Task<IEnumerable<Standing>> GetByGoalDifferenceRangeAsync(int minGD, int maxGD);
    }
}
