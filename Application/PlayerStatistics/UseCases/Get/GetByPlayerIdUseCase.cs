using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Get
{
    public class GetByPlayerIdUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public GetByPlayerIdUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<List<PlayerStatisticResponseDTO>> ExecuteAsync(int playerId)
        {
            var list = await _repo.GetByPlayerIdAsync(new PlayerID(playerId));
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
