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
        public DeleteStandingUseCase(IStandingRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id)
            => _repo.DeleteAsync(new StandingID(id));
    }
}
