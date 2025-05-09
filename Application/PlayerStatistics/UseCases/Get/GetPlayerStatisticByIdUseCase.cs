using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.Mappers;
using Application.Playes.DTOs;
using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PlayerStatistics.UseCases.Get
{
    public class GetPlayerStatisticByIdUseCase
    {
        private readonly IPlayerStatisticRepository _repo;
        public GetPlayerStatisticByIdUseCase(IPlayerStatisticRepository repo) => _repo = repo;

        public async Task<PlayerStatisticResponseDTO?> ExecuteAsync(int id)
        {
            var s = await _repo.GetByIdAsync(new PlayerStatisticID(id));
            return s is null ? null : s.ToDTO();
        }
    }
}
