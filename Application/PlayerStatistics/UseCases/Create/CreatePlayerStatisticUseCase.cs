using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Domain.Entities.Players;
using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Create
{
    public class CreatePlayerStatisticUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public CreatePlayerStatisticUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<PlayerStatisticResponseDTO> ExecuteAsync(PlayerStatisticRequestDTO dto)
        {
            var stat = dto.ToDomain();
            var created = await _repo.AddAsync(stat);
            return created.ToDTO();
        }
    }
}
