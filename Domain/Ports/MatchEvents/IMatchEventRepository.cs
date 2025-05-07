using Domain.Entities.MatchEvents;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.MatchEvents
{
    public interface IMatchEventRepository
    {
        Task<MatchEvent?> GetByIdAsync(MatchEventID matchEventId);
        Task<IEnumerable<MatchEvent>> GetAllAsync();
        Task<IEnumerable<MatchEvent>> GetByMatchIdAsync(MatchID matchId);
        Task<IEnumerable<MatchEvent>> GetByPlayerIdAsync(PlayerID playerId);
        Task<IEnumerable<MatchEvent>> GetByTypeAsync(EventType eventType);
        Task<IEnumerable<MatchEvent>> GetByMinuteRangeAsync(int fromMinute, int toMinute);
        Task<MatchEvent> AddAsync(MatchEvent matchEvent);
        Task UpdateAsync(MatchEvent matchEvent);
        Task<bool> DeleteAsync(MatchEventID matchEventId);
    }
}
