using Domain.Entities.Matches;

namespace Domain.Ports.Matches
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(int matchId);
        Task<IEnumerable<Match>> GetAllAsync();
        Task<IEnumerable<Match>> GetByTeamIdAsync(int teamId);
        Task<IEnumerable<Match>> GetByLeagueIdAsync(int leagueId);
        Task<Match> AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task<bool> DeleteAsync(int matchId);
    }
}
