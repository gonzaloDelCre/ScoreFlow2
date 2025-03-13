using Domain.Entities.Referees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports.Referees
{
    public interface IRefereeRepository
    {
        Task<Referee?> GetByIdAsync(int refereeId);
        Task<IEnumerable<Referee>> GetAllAsync();
        Task<Referee> AddAsync(Referee referee);
        Task UpdateAsync(Referee referee);
        Task<bool> DeleteAsync(int refereeId);
    }
}
