using Domain.Entities.MatchEvents;
using Domain.Ports.MatchEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MatchEvents.UseCases.Delete
{
    public class DeleteMatchEventUseCase
    {
        private readonly IMatchEventRepository _repo;
        public DeleteMatchEventUseCase(IMatchEventRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id)
            => _repo.DeleteAsync(new MatchEventID(id));
    }
}
