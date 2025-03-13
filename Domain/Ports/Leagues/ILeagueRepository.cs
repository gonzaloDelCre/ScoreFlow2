using Domain.Entities.Leagues;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Leagues
{
    public interface ILeagueRepository
    {
        Task<League?> GetByIdAsync(LeagueID leagueId);
        Task<IEnumerable<League>> GetAllAsync();
        Task<League> AddAsync(League league);
        Task UpdateAsync(League league);
        Task<bool> DeleteAsync(LeagueID leagueId);
    }
}
