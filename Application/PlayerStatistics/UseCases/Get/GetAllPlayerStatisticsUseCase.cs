using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Application.Playes.DTOs;
using Domain.Ports.PlayerStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Get
{
    public class GetAllPlayerStatisticsUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public GetAllPlayerStatisticsUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<List<PlayerStatisticResponseDTO>> ExecuteAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(s => s.ToDTO()).ToList();
        }
    }
}
