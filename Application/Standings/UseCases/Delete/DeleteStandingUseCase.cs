using Domain.Ports.Standings;
using System.Threading.Tasks;
using Domain.Shared;

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
            var ok = await _repo.DeleteAsync(new Domain.Entities.Standings.StandingID(id));
            if (!ok) throw new KeyNotFoundException($"Standing {id} no existe");
        }
    }
}
