using Domain.Entities.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.MatchEvents
{
    public interface IMatchEventRepository
    {
        Task<MatchEvent?> GetByIdAsync(int matchEventId);
        Task<IEnumerable<MatchEvent>> GetAllAsync();
        Task<IEnumerable<MatchEvent>> GetByMatchIdAsync(int matchId);
        Task<MatchEvent> AddAsync(MatchEvent matchEvent);
        Task UpdateAsync(MatchEvent matchEvent);
        Task<bool> DeleteAsync(int matchEventId);
    }
}
