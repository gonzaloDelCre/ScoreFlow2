using Domain.Ports.Leagues;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Leagues.UseCases.Delete
{
    public class DeleteLeagueUseCase
    {
        private readonly ILeagueRepository _repo;

        public DeleteLeagueUseCase(ILeagueRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id) =>
            _repo.DeleteAsync(new LeagueID(id));
    }
}
