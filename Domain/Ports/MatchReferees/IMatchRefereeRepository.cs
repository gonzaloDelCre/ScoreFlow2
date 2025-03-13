using Domain.Entities.MatchReferees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.MatchReferees
{
    public interface IMatchRefereeRepository
    {
        Task<MatchReferee?> GetByIdsAsync(int matchId, int refereeId);
        Task<IEnumerable<MatchReferee>> GetAllAsync();
        Task<MatchReferee> AddAsync(MatchReferee matchReferee);
        Task UpdateAsync(MatchReferee matchReferee);
        Task<bool> DeleteAsync(int matchId, int refereeId);
    }
}
