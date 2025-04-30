using Domain.Entities.Standings;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Delete
{
    public class DeleteStandingUseCase
    {
        private readonly IStandingRepository _repo;

        public DeleteStandingUseCase(IStandingRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(new StandingID(id));
            if (!ok) throw new KeyNotFoundException($"Standing {id} no existe");
        }
    }
}
