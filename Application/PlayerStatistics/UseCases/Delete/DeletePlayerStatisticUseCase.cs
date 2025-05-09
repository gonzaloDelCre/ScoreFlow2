using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Delete
{
    public class DeletePlayerStatisticUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public DeletePlayerStatisticUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public Task<bool> ExecuteAsync(int id)
            => _repo.DeleteAsync(new PlayerStatisticID(id));
    }
}
