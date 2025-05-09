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
    public class GetByMatchIdUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public GetByMatchIdUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<List<PlayerStatisticResponseDTO>> ExecuteAsync(int matchId)
        {
            var list = await _repo.GetByMatchIdAsync(new MatchID(matchId));
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
