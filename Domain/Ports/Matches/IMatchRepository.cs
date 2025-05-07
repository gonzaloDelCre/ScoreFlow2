using Domain.Entities.Matches;
using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Enum;
using Domain.Shared;

namespace Domain.Ports.Matches
{
    public interface IMatchRepository
    {
        // CRUD básico
        Task<Match?> GetByIdAsync(MatchID matchId);
        Task<IEnumerable<Match>> GetAllAsync();
        Task<Match> AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task<bool> DeleteAsync(MatchID matchId);
        Task<IEnumerable<Match>> GetByTeamIdAsync(int teamId);
        Task<IEnumerable<Match>> GetByLeagueIdAsync(int leagueId);
        Task<IEnumerable<MatchEvent>> GetEventsAsync(MatchID matchId);
        Task UpdateEventsAsync(MatchID matchId, IEnumerable<MatchEvent> events);
        Task<IEnumerable<PlayerStatistic>> GetStatisticsAsync(MatchID matchId);
        Task UpdateStatisticsAsync(MatchID matchId, IEnumerable<PlayerStatistic> stats);
    }
}
