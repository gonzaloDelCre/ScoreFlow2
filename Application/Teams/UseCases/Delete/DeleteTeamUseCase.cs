using Domain.Ports.Teams;
using Domain.Shared;

namespace Application.Teams.UseCases.Delete
{
    public class DeleteTeamUseCase
    {
        private readonly ITeamRepository _repo;
        public DeleteTeamUseCase(ITeamRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id) =>
            _repo.DeleteAsync(new TeamID(id));
    }
}
