using Application.Standings.DTOs;
using Application.Standings.Mappers;
using Domain.Ports.Standings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.UseCases.Get
{
    public class GetByGoalDifferenceRangeUseCase
    {
        private readonly IStandingRepository _repo;
        public GetByGoalDifferenceRangeUseCase(IStandingRepository repo) => _repo = repo;

        public async Task<List<StandingResponseDTO>> ExecuteAsync(int minGD, int maxGD)
        {
            var list = await _repo.GetByGoalDifferenceRangeAsync(minGD, maxGD);
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
